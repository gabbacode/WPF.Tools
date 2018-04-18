namespace Ui.Wpf.KanbanControl.Elements
{
    public class Cell
    {
        public Cell(CellView cellView)
        {
            View = cellView;
        }

        public CellView View { get; private set; }
    }
}