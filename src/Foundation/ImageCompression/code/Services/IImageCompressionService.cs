namespace Sitecore.Foundation.ImageCompression.Services
{
    public interface IImageCompressionService
    {
        string CompressImage(Data.Items.Item currentItem);
        bool ShouldContinue { get; set; }
    }
}