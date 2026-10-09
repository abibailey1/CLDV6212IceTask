using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AppointmentApp.Models;

public partial class User
{
    [Key]
    [Column(TypeName = "character varying")]
    public string Userid { get; set; } = null!;

    [Column(TypeName = "character varying")]
    public string FirstName { get; set; } = null!;

    [Column(TypeName = "character varying")]
    public string? LastName { get; set; }

    [Column(TypeName = "character varying")]
    public string? Email { get; set; }

    [Column(TypeName = "character varying")]
    public string? PasswordHash { get; set; }

    [Column(TypeName = "character varying")]
    public string? Role { get; set; }

    [JsonIgnore]
    [InverseProperty("User")]
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
