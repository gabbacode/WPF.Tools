namespace Data.Entities.Common.Redmine
{
    public class Issue
    {
        public int Id { get; set; }

        public User AssignedTo { get; set; }

        public Project Project { get; set; }

        public Status Status { get; set; }

        public Priority Priority { get; set; }

        public Tracker Tracker { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }
    }
}
