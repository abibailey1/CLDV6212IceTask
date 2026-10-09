using System.ComponentModel.DataAnnotations;

namespace AppointmentApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required] public string FirstName { get; set; } = "";
        [Required] public string LastName { get; set; } = "";
        [Required, EmailAddress] public string Email { get; set; } = "";
        [Required, MinLength(6)] public string Password { get; set; } = "";
    }
}