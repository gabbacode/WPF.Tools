using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities.Common.Redmine;

namespace Data.Sources.LocalStorage.Sqlite.Context
{
   public class SqliteIssue
    {
        public int Id { get; set; }

        public int? ProjectId { get; set; }
        public Project Project { get; set; }

        public int? StatusId { get; set; }
        public Status Status { get; set; }

        public int? PriorityId { get; set; }
        public Priority Priority { get; set; }

        public int? TrackerId { get; set; }
        public Tracker Tracker { get; set; }

        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CustomFields { get; set; }
    }
}
