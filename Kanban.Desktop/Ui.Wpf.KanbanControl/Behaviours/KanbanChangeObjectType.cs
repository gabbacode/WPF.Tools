using System;

namespace Ui.Wpf.KanbanControl.Behaviours
{
    [Flags]
    public enum KanbanChangeObjectType
    {
        VerticalCategories = 0,
        HorizontalCategories = 1,
        Items = 2,
        ShowStrategy = 4,
        DispenseStrategy = 8
    }
}
