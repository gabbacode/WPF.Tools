using Ui.Wpf.KanbanControl.Behaviours;

namespace Ui.Wpf.KanbanControl
{
    public interface IShowKanbanStrategy
    {
        void Show(KanbanChangeObjectType changeObjectType);
    }
}