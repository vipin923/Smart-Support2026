namespace Smart_Support2026.Models
{
    public class ReplyModel
    {
        public int Ticket_Id { get; set; }
        public string Reply_By { get; set; } = string.Empty;
        public string Reply {  get; set; } = string.Empty;
        public DateTime Answer_Date { get; set; }
        public List<RaiseTickets> RaiseTickets { get; set; } = new(); 
    }
}
