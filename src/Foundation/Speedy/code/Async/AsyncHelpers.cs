using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sitecore.Foundation.Speedy.Async
{
    public static class AsyncHelpers
    {
        public static void RunSync(Func<Task> task)
        {
            var oldContext = SynchronizationContext.Current;
            var synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
            synch.Post(
                async _ =>
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
                {
                    try
                    {
                        await task?.Invoke();
                    }
                    catch (Exception e)
                    {
                        synch.InnerException = e;
                        throw;
                    }
                    finally
                    {
                        synch.EndMessageLoop();
                    }
                },
                null);
            synch.BeginMessageLoop();

            SynchronizationContext.SetSynchronizationContext(oldContext);
        }

        public static T RunSync<T>(Func<Task<T>> task)
        {
            var oldContext = SynchronizationContext.Current;
            var synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
            var ret = default(T);
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
            synch.Post(
                async _ =>
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
                {
                    try
                    {
                        ret = await task?.Invoke();
                    }
                    catch (Exception e)
                    {
                        synch.InnerException = e;
                        throw;
                    }
                    finally
                    {
                        synch.EndMessageLoop();
                    }
                },
                null);
            synch.BeginMessageLoop();
            SynchronizationContext.SetSynchronizationContext(oldContext);
            return ret;
        }

        private class ExclusiveSynchronizationContext : SynchronizationContext, IDisposable
        {
            private readonly Queue<Tuple<SendOrPostCallback, object>> items =
                new Queue<Tuple<SendOrPostCallback, object>>();

            private readonly AutoResetEvent workItemsWaiting = new AutoResetEvent(false);

            private bool done;

            public Exception InnerException { get; set; }

            public void Dispose()
            {
                workItemsWaiting.Dispose();
            }

            public void BeginMessageLoop()
            {
                while (!done)
                {
                    Tuple<SendOrPostCallback, object> task = null;
                    lock (items)
                    {
                        if (items.Count > 0) task = items.Dequeue();
                    }

                    if (task != null)
                    {
                        task.Item1(task.Item2);
                        if (InnerException != null)
                            throw new AggregateException(
                                "AsyncHelpers.Run method threw an exception.",
                                InnerException);
                    }
                    else
                    {
                        workItemsWaiting.WaitOne();
                    }
                }
            }

            public override SynchronizationContext CreateCopy()
            {
                return this;
            }

            public void EndMessageLoop()
            {
                Post(_ => done = true, null);
            }

            public override void Post(SendOrPostCallback d, object state)
            {
                lock (items)
                {
                    items.Enqueue(Tuple.Create(d, state));
                }

                workItemsWaiting.Set();
            }

            public override void Send(SendOrPostCallback d, object state)
            {
                throw new NotSupportedException("We cannot send to our same thread");
            }
        }
    }
}