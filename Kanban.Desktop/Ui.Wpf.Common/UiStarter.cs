namespace Ui.Wpf.Common
{
    public static class UiStarter
    {
        public static IShell Start<TStartWindow>(IBootstraper bootstraper, UiShowStartWindowOptions options = null)
            where TStartWindow : class 
        {
            var shell = bootstraper.Init();

            shell.ShowStartView<TStartWindow>(options);

            return shell;
        }

        public static IShell Start<TStartWindow, TStartView>(IBootstraper bootstraper, UiShowStartWindowOptions options = null)
            where TStartWindow : class
            where TStartView : class, IView
        {
            var shell = bootstraper.Init();

            shell.ShowStartView<TStartWindow, TStartView>(options);

            return shell;
        }
    }
}
