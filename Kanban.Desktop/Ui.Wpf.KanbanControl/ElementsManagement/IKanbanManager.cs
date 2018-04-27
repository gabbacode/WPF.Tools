using Ui.Wpf.KanbanControl.Expressions;

namespace Ui.Wpf.KanbanControl.ElementsManagement
{
    public interface IKanbanManager
    {
        void AddActionsToShow(KanbanChangeObjectType changeObjectType, PropertyAccessorsExpressionCreator propertyAccessors);
    }
}