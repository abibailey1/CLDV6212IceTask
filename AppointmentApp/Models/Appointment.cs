using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AppointmentApp.Models
{
    public class Appointment
    {
        [Key]
        [Column(TypeName = "character varying")]
        public string AppointmentId { get; set; } = null!;

        [Column(TypeName = "character varying")]
        public string UserId { get; set; } = null!;

        [Column(TypeName = "character varying")]
        public string SlotId { get; set; } = null!;

        public DateOnly AppointmentDate { get; set; }

        [Column(TypeName = "character varying")]
        public string Status { get; set; } = null!;

        [Column(TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        [ForeignKey("SlotId")]
        public virtual AppointmentSlot Slot { get; set; } = null!;

        [JsonIgnore]
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}