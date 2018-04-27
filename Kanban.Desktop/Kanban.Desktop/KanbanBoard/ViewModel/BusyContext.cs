using System;

namespace Kanban.Desktop.KanbanBoard.ViewModel
{
    public class BusyContext : IDisposable
    {
        public BusyContext(Action enter, Action exit)
        {
            enterAction = enter;
            exitAction = exit;

            enterAction();
        }

        public void Dispose()
        {
            exitAction();
        }

        private readonly Action enterAction;
        private readonly Action exitAction;
    }
}
