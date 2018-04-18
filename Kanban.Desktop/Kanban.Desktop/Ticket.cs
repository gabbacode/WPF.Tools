namespace Kanban.Desktop
{
    internal class Ticket
    {
        public Ticket(string status, string state)
        {
            Status = status;
            State = state;
        }

        public string Status { get; set; }

        public string State { get; set; }
    }
}