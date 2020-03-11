using Sitecore.Data.Items;

namespace Sitecore.Foundation.Speedy.Repositories
{
    public interface ICriticalCSSRepository
    {
        void UpdateCriticalCSS(Item item, string database);
    }
}
