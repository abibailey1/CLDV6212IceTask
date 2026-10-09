using AppointmentApp.Models;

namespace AppointmentApp.Services
{
    public interface IApiClient
    {
        Task<bool> RegisterAsync(RegistrationDTO dto);
        Task<string?> LoginAsync(LoginDTO dto);

        Task<List<Appointment>> GetMyAppointmentsAsync();
        Task<List<Appointment>> GetAllAppointmentsAsync();
        Task<Appointment?> GetAppointmentAsync(string id);
        Task<bool> CreateAppointmentAsync(AppointmentDTO dto);
        Task<bool> UpdateAppointmentAsync(string id, AppointmentDTO dto);
        Task<bool> CancelAppointmentAsync(string id);

        Task<List<AppointmentSlot>> GetAllSlotsAsync();
        Task<List<AppointmentSlot>> GetAvailableSlotsAsync();
        Task<bool> CreateSlotAsync(AppointmentSlotDTO dto);
        Task<bool> UpdateSlotAsync(string id, AppointmentSlotDTO dto);
        Task<bool> DeleteSlotAsync(string id);
    }
}