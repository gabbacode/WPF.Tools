using System;
using Data.Entities.Common.LocalBase;

namespace Data.Sources.LocalStorage.Sqlite.Context
{
    public class SqliteIssue
    {
        public int Id { get; set; }
        public string Head { get; set; }
        public string Body { get; set; }

        public int? RowId { get; set; }
        public RowInfo Row { get; set; }

        public int? ColumnId { get; set; }
        public ColumnInfo Column { get; set; }

        public string Color { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
