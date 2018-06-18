using System;

namespace Data.Entities.Common.Redmine
{
    public class Status : IEquatable<Status>, IComparable, IComparable<Status>
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public int CompareTo(Status other)
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
            return CompareTo((Status)obj);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Status);
        }

        public bool Equals(Status other)
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
            return Name;
        }
    }
}
