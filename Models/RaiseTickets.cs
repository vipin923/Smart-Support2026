namespace Smart_Support2026.Models
{
    public class RaiseTickets
    {
        public int Id { get; set; }
        public string Employee_Id { get; set; }  = string.Empty;
        public DateTime Created_On { get; set; }
        public string Query { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Answer {  get; set; } = string.Empty;
        public string Type {  get; set; } = string.Empty;

        public string Ans_Date { get; set; } = string.Empty;
    }
}
