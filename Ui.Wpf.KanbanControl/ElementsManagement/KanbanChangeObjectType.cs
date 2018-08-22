using System;

namespace Ui.Wpf.KanbanControl.ElementsManagement
{
    [Flags]
    public enum KanbanChangeObjectType
    {
        VerticalCategories = 0,
        HorizontalCategories = 1,
        Cards = 2,
        ShowStrategy = 4,
        DispenseStrategy = 8,
        CardContentItems = 16,
        CardsColors = 32,
    }
}
