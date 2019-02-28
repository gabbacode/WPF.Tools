using System.Windows;
using Ui.Wpf.Common.Extensions;
using Xceed.Wpf.AvalonDock.Layout;

namespace Ui.Wpf.Common
{
    public partial class Shell
    {
        public void SetContainerWidth(string containerName, GridLength width)
        {
            SetContainerSizeInternal(containerName, width, null);
        }

        public void SetContainerHeight(string containerName, GridLength height)
        {
            SetContainerSizeInternal(containerName, null, height);
        }

        public void SetContainerSize(string containerName, GridLength width, GridLength height)
        {
            SetContainerSizeInternal(containerName, width, height);
        }

        private void SetContainerSizeInternal(string containerName, GridLength? width, GridLength? height)
        {
            var toolContainer = DockingManager.FindObjectByName<LayoutAnchorablePane>(containerName);
            var viewContainer = DockingManager.FindObjectByName<LayoutDocumentPane>(containerName);

            if (width.HasValue)
            {
                if (toolContainer != null)
                    toolContainer.DockWidth = width.Value;
                if (viewContainer != null)
                    viewContainer.DockWidth = width.Value;
            }

            if (height.HasValue)
            {
                if (toolContainer != null)
                    toolContainer.DockHeight = height.Value;
                if (viewContainer != null)
                    viewContainer.DockHeight = height.Value;
            }
        }
    }
}