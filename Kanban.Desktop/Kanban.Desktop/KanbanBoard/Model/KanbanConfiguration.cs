using System.IO;
using System.Xml.Serialization;

namespace Kanban.Desktop.KanbanBoard.Model
{
    public class KanbanConfiguration
    {
        public int? ProjectId { get; set; }

        public DimensionConfiguration HorizontalDimension { get; set; }

        public DimensionConfiguration VerticalDimension { get; set; }

        public CardItemsConfiguration CardItemsConfiguration { get; set; }

        public static KanbanConfiguration Parse(string configurationData)
        {
            var serializer = new XmlSerializer(typeof(KanbanConfiguration));
            using (var r = new StringReader(configurationData))
            {
                return (KanbanConfiguration)serializer.Deserialize(r);
            }
        }

        public override string ToString()
        {
            var serializer = new XmlSerializer(typeof(KanbanConfiguration));
            using (var w = new StringWriter())
            {
                serializer.Serialize(w, this);

                return w.ToString();
            }

        }

    }
}
