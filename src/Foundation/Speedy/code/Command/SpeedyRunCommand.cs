using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Foundation.Speedy.Events;
using Sitecore.Foundation.Speedy.Settings;
using Sitecore.Shell.Framework.Commands;
using static Sitecore.Foundation.Speedy.SpeedyConstants;

/// <summary>
/// https://community.sitecore.net/technical_blogs/b/sitecore_what39s_new/posts/adding-a-custom-button-to-the-ribbon
/// </summary>
namespace Sitecore.Foundation.Speedy.Command
{
    public class SpeedyRunCommand  : Sitecore.Shell.Framework.Commands.Command
    {
        public override void Execute(CommandContext context)
        {
            if (context.Items.Length == 1)
            {
                if (!SpeedyGenerationSettings.IsPublicFacingEnvironment())
                    return;

                var speedyPageEvt = new SpeedyPageOnSaveEvent();
                speedyPageEvt.Database = GlobalSettings.Database.Master;

                var currentItem = context.Items[0];
                currentItem.Editing.BeginEdit();
                speedyPageEvt.UpdateCritical(currentItem);
                currentItem.Editing.EndEdit();
            }
        }
    }
}