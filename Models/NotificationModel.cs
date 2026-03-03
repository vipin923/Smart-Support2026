namespace Smart_Support2026.Models
{
    public class NotificationModel
    {
        public int Id { get; set; }
        public string emp_Id { get; set; } = string.Empty;

        public string message { get; set; } = string.Empty;
        public bool IsRead { get; set; }

        public DateTime Notify_On { get; set; }

    }
}
