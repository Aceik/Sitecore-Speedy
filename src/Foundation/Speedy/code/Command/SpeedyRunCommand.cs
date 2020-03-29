using Sitecore.Diagnostics;
using Sitecore.Foundation.Speedy.Repositories;
using Sitecore.Foundation.Speedy.Settings;
using Sitecore.Shell.Framework.Commands;
using static Sitecore.Foundation.Speedy.SpeedyConstants;


namespace Sitecore.Foundation.Speedy.Command
{
    /// <summary>
    /// https://community.sitecore.net/technical_blogs/b/sitecore_what39s_new/posts/adding-a-custom-button-to-the-ribbon
    /// </summary>
    public class SpeedyRunCommand  : Sitecore.Shell.Framework.Commands.Command
    {
        private readonly ICriticalCSSRepository _criticalCSSRepository;

        public SpeedyRunCommand(ICriticalCSSRepository criticalCSSRepository)
        {
            _criticalCSSRepository = criticalCSSRepository;
        }

        public override void Execute(CommandContext context)
        {
            if (context.Items.Length != 1)
                return;

            if (!SpeedyGenerationSettings.IsPublicFacingEnvironment() && !SpeedyGenerationSettings.UseLocalCriticalCssGenerator())
                return;

            //var criticalCSSRepository = ServiceLocator.ServiceProvider.GetService<ICriticalCSSRepository>();

            var currentItem = context.Items[0];
            currentItem.Editing.BeginEdit();
            _criticalCSSRepository.UpdateCriticalCSS(currentItem, GlobalSettings.Database.Master);
            currentItem.Editing.EndEdit();
        }

        /// <summary>
        /// Queries the state of the command.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The state of the command.</returns>
        public override CommandState QueryState(CommandContext context)
        {
            Assert.ArgumentNotNull(context, "context");

            if (!SpeedyGenerationSettings.IsPublicFacingEnvironment() && !SpeedyGenerationSettings.UseLocalCriticalCssGenerator())
                return CommandState.Hidden;

            if (context.Items.Length != 1)
            {
                return CommandState.Hidden;
            }

            var item = context.Items[0];

            return item.IsSpeedyEnabledForPage() ? CommandState.Enabled : CommandState.Disabled;
        }
    }
}