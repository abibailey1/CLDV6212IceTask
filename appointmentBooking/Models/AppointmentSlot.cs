using System;
using System.Collections.Generic;

namespace appointmentBooking.Models;

public partial class AppointmentSlot
{
    public string SlotId { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public bool IsAvailable { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
