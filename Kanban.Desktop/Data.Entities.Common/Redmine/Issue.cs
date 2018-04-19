namespace Data.Entities.Common.Redmine
{
    public class Issue
    {
        public string AssignedTo { get; set; }

        public string Project { get; set; }

        public string Status { get; set; }

        public string Priority { get; set; }

        public string Tracker { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }
    }
}
