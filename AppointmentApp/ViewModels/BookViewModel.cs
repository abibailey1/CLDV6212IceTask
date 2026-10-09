using System.ComponentModel.DataAnnotations;

namespace AppointmentApp.ViewModels
{
    public class BookViewModel
    {
        [Required] public string SlotId { get; set; } = "";
    }
}