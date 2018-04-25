namespace Kanban.Desktop.KanbanBoard.Model
{
    public class ColorMap
    {
        public string Value { get; set; }

        public CardColors CardColors { get; set; }

        public ColorMap()
        {

        }

        public ColorMap(string value, CardColors colors)
        {
            Value = value;
            CardColors = colors;
        }
    }
}