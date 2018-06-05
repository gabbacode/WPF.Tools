using System;

namespace Data.Entities.Common.Redmine
{
    public class Tracker : IEquatable<Tracker>, IComparable, IComparable<Tracker>
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public int CompareTo(object obj)
        {
            return CompareTo((Tracker)obj);
        }

        public int CompareTo(Tracker other)
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

        public override bool Equals(object obj)
        {
            return Equals(obj as Tracker);
        }

        public bool Equals(Tracker other)
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
