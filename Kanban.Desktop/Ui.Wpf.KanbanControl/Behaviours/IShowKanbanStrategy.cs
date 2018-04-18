using Ui.Wpf.KanbanControl.Behaviours;

namespace Ui.Wpf.KanbanControl
{
    public interface IShowKanbanStrategy
    {
        void AddActionsToShow(KanbanChangeObjectType changeObjectType);
    }
}