using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Data;

namespace Sitecore.Foundation.Speedy.Scheduler
{
    public class SyncCritical
    {
        public virtual void RunSync()
        {
            using (var context = ContentSearchManager.GetIndex("sitecore_master").CreateSearchContext())
            {
                Expression<Func<SearchResultItem, bool>> predicate = PredicateBuilder.True<SearchResultItem>();
                predicate = predicate.Or(p => p.TemplateName.Equals("_SpeedyPage"));

                IEnumerable<SearchResultItem> results = context
                    .GetQueryable<SearchResultItem>()
                    .Where(predicate);

                foreach (var result in results)
                {
                    var item = result.GetItem();
                }
            }
        }
    }
}