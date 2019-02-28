using System.Linq;
using System.Windows;
using Ui.Wpf.Common.AttachedProperties;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;

namespace Ui.Wpf.Common.Extensions
{
    public static class DockingManagerExtensions
    {
        public static T FindObjectByName<T>(this DockingManager dm, string name)
            where T : DependencyObject
        {
            if (dm == null)
                return null;

            return new DependencyObject[]
                {
                    dm.Layout,
                    dm.LeftSidePanel,
                    dm.TopSidePanel,
                    dm.RightSidePanel,
                    dm.BottomSidePanel
                }
                .Concat(dm.FloatingWindows)
                .Select(x => x.FindChildByName<T>(name))
                .FirstOrDefault(x => x != null);
        }

        public static T FindChildByName<T>(this DependencyObject root, string name)
            where T : DependencyObject
        {
            if (root == null || string.IsNullOrEmpty(name))
                return null;

            if (LogicalTreeHelper.FindLogicalNode(root, name) is T logicalTreeResult)
                return logicalTreeResult;

            if (DockContainer.GetName(root) == name && root is T rootResult)
                return rootResult;

            if (root is ILayoutContainer container)
            {
                foreach (var child in container.Children.OfType<DependencyObject>())
                {
                    var childResult = FindChildByName<T>(child, name);
                    if (childResult != null)
                        return childResult;
                }
            }

            return null;
        }

        public static T FindByViewRequest<T>(this ILayoutContainer root, ViewRequest viewRequest)
            where T : LayoutContent
        {
            return viewRequest != null ? root.FindByViewId<T>(viewRequest.ViewId) : null;
        }

        public static T FindByViewId<T>(this ILayoutContainer root, string viewId)
            where T : LayoutContent
        {
            if (string.IsNullOrEmpty(viewId))
                return null;

            foreach (var child in root.Children)
            {
                if (child is T childResult && childResult.ContentId == viewId)
                    return childResult;

                if (child is ILayoutContainer container)
                {
                    var childChildResult = container.FindByViewId<T>(viewId);
                    if (childChildResult != null)
                        return childChildResult;
                }
            }

            return null;
        }
    }
}