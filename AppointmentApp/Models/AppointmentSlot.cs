using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApp.Models;

public partial class AppointmentSlot
{
    [Key]
    [Column(TypeName = "character varying")]
    public string SlotId { get; set; } = null!;

    public DateOnly Date { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public bool? IsAvailable { get; set; }

    [InverseProperty("Slot")]
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
