using System;
using System.Collections.Generic;

namespace appointmentBooking.Models;

public partial class Appointment
{
    public string AppointmentId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string SlotId { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual AppointmentSlot Slot { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
