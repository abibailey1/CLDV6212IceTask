namespace AppointmentApp.Models
{
    public class AppointmentSlotDTO
    {
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public bool IsAvailable { get; set; }
    }
}