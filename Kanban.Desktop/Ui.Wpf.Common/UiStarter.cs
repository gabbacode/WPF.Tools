namespace Ui.Wpf.Common
{
    public static class UiStarter
    {
        public static Shell Start<TStartWindow>(IBootstraper bootstraper, UiStartOptions options = null)
        {
            var shell = bootstraper.Init();

            shell.ShowStartView<TStartWindow>();

            return shell;
        }

        public static Shell Start<TStartWindow, TStartView>(IBootstraper bootstraper, UiStartOptions options = null)
        {
            var shell = bootstraper.Init();

            shell.ShowStartView<TStartWindow, TStartView>();

            return shell;
        }
    }
}
