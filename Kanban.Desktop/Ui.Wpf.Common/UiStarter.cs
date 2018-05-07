namespace Ui.Wpf.Common
{
    public static class UiStarter
    {
        public static IShell Start<TStartWindow>(IBootstraper bootstraper, UiShowOptions options = null)
            where TStartWindow : class 
        {
            var shell = bootstraper.Init();

            shell.ShowStartView<TStartWindow>();

            return shell;
        }

        public static IShell Start<TStartWindow, TStartView>(IBootstraper bootstraper, UiShowOptions options = null)
            where TStartWindow : class
            where TStartView : class, IView
        {
            var shell = bootstraper.Init();

            shell.ShowStartView<TStartWindow, TStartView>();

            return shell;
        }
    }
}
