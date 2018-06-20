using System;
using System.Collections.Generic;

namespace Data.Entities.Common.Redmine
{
    public class Journal : IEquatable<Journal>, IComparable, IComparable<Journal>
    {
        public int? Id { get; set; }

        public User User { get; set; }

        public string Notes { get; set; }

        public DateTime? CreatedOn { get; set; }

        public bool PrivateNotes { get; set; }

        public IList<Detail> Details { get; set; }


        public int CompareTo(Journal other)
        {
            if (other == null) return 1;

            if (Id == null)
            {
                if (other.Id == null)
                    return 0;

                return -1;
            }

            return Id.Value.CompareTo(other.Id.Value);
        }

        public int CompareTo(object obj)
        {
            return CompareTo((Journal)obj);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Journal);
        }

        public bool Equals(Journal other)
        {
            return other != null &&
                   Id == other.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"Id: {Id} - { User?.Name } - { Notes }";
        }
    }
}
