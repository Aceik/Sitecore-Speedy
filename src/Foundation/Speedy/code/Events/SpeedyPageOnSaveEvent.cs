using Sitecore.Data.Items;
using Sitecore.Events;
using System;
using Sitecore.Foundation.Speedy.Repositories;
using Sitecore.Foundation.Speedy.Settings;

namespace Sitecore.Foundation.Speedy.Events
{
    public class SpeedyPageOnSaveEvent
    {
        private readonly ICriticalCSSRepository _criticalCSSRepository;

        public string Database
        {
            get;
            set;
        }

        public SpeedyPageOnSaveEvent(ICriticalCSSRepository criticalCSSRepository)
        {
            _criticalCSSRepository = criticalCSSRepository;
        }

        public void OnItemSaving(object sender, EventArgs args)
        {
            try
            {
                if(!SpeedyGenerationSettings.ShouldRegenerateOnEachSave())
                    return;

                Diagnostics.Log.Info("SpeedyPageOnSaveEvent running", this);

                var item = Event.ExtractParameter(args, 0) as Item;

                _criticalCSSRepository.UpdateCriticalCSS(item, Database);
            }
            catch (Exception ex)
            {
                Diagnostics.Log.Error("Speedy OnItemSaving is falling over", ex, this);
            }
        }
  
    }
}