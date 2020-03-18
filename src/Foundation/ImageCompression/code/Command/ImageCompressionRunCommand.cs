using Sitecore.Diagnostics;
using Sitecore.Foundation.ImageCompression.Services;
using Sitecore.Foundation.ImageCompression.Settings;
using Sitecore.Shell.Framework.Commands;                


namespace Sitecore.Foundation.ImageCompression.Command
{
    /// <summary>
    /// https://community.sitecore.net/technical_blogs/b/sitecore_what39s_new/posts/adding-a-custom-button-to-the-ribbon
    /// </summary>
    public class ImageCompressionRunCommand : Sitecore.Shell.Framework.Commands.Command
    {
        private readonly IImageCompressionService _imageCompressionService;

        public ImageCompressionRunCommand(IImageCompressionService imageCompressionService)
        {
            _imageCompressionService = imageCompressionService;
        }

        public override void Execute(CommandContext context)
        {
            if (context.Items.Length != 1)
                return;

            if (!ImageCompressionSettings.IsImageCompressionButtonEnabled())
                return;

            var currentItem = context.Items[0];
            currentItem.Editing.BeginEdit();
            _imageCompressionService.CompressImage(currentItem);
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

            if (!ImageCompressionSettings.IsImageCompressionButtonEnabled())
                return CommandState.Hidden;

            if (context.Items.Length != 1)
            {
                return CommandState.Hidden;
            }

            var item = context.Items[0];

            return CommandState.Enabled;
        }
    }
}