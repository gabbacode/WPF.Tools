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
            return dm?.Layout?.Descendents()
                .OfType<DependencyObject>()
                .Where(x => DockContainer.GetName(x) == name)
                .OfType<T>()
                .FirstOrDefault();
        }

        public static T FindByViewRequest<T>(this ILayoutContainer root, ViewRequest viewRequest)
            where T : LayoutContent
        {
            return viewRequest != null ? root.FindByViewId<T>(viewRequest.ViewId) : null;
        }

        public static T FindByViewId<T>(this ILayoutContainer root, string viewId)
            where T : LayoutContent
        {
            return root.Descendents()
                .OfType<T>()
                .FirstOrDefault(x => x.ContentId == viewId);
        }
    }
}