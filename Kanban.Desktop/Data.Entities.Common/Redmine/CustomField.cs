using System.Collections.Generic;

namespace Data.Entities.Common.Redmine
{
    public class CustomField
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<CustomFieldValue> Values { get; set; }
    }
}
