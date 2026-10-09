using System.Net.Http.Json;
using AppointmentApp.Models;

namespace AppointmentApp.Services
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _accessor;

        public ApiClient(HttpClient http, IHttpContextAccessor accessor)
        {
            _http = http;
            _accessor = accessor;
        }

        private void AttachCookie()
        {
            _http.DefaultRequestHeaders.Remove("Cookie");
            var cookie = _accessor.HttpContext?.Request.Headers["Cookie"].ToString();
            if (!string.IsNullOrEmpty(cookie))
                _http.DefaultRequestHeaders.Add("Cookie", cookie);
        }

        public async Task<bool> RegisterAsync(RegistrationDTO dto)
        {
            var resp = await _http.PostAsJsonAsync("api/authentication/register", dto);
            return resp.IsSuccessStatusCode;
        }

        public async Task<string?> LoginAsync(LoginDTO dto)
        {
            var resp = await _http.PostAsJsonAsync("api/authentication/login", dto);
            if (!resp.IsSuccessStatusCode) return null;

            var setCookie = resp.Headers
                .FirstOrDefault(h => h.Key.Equals("Set-Cookie", StringComparison.OrdinalIgnoreCase))
                .Value?.FirstOrDefault();

            if (setCookie is null) return null;

            _accessor.HttpContext?.Response.Headers.Append("Set-Cookie", setCookie);

            _http.DefaultRequestHeaders.Remove("Cookie");
            _http.DefaultRequestHeaders.Add("Cookie", setCookie);

            var asPatient = await _http.GetAsync("api/patient");
            if (asPatient.IsSuccessStatusCode) return "patient";

            var asStaff = await _http.GetAsync("api/staff");
            if (asStaff.IsSuccessStatusCode) return "staff";

            return null;
        }

        public async Task<List<Appointment>> GetMyAppointmentsAsync()
        {
            AttachCookie();
            return await _http.GetFromJsonAsync<List<Appointment>>("api/appointment/patient") ?? new();
        }

        public async Task<List<Appointment>> GetAllAppointmentsAsync()
        {
            AttachCookie();
            return await _http.GetFromJsonAsync<List<Appointment>>("api/appointment") ?? new();
        }

        public async Task<Appointment?> GetAppointmentAsync(string id)
        {
            AttachCookie();
            return await _http.GetFromJsonAsync<Appointment>($"api/appointment/{id}");
        }

        public async Task<bool> CreateAppointmentAsync(AppointmentDTO dto)
        {
            AttachCookie();
            var resp = await _http.PostAsJsonAsync("api/appointment", dto);
            return resp.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAppointmentAsync(string id, AppointmentDTO dto)
        {
            AttachCookie();
            var resp = await _http.PutAsJsonAsync($"api/appointment/{id}", dto);
            return resp.IsSuccessStatusCode;
        }

        public async Task<bool> CancelAppointmentAsync(string id)
        {
            AttachCookie();
            var resp = await _http.DeleteAsync($"api/appointment/{id}");
            return resp.IsSuccessStatusCode;
        }

        public async Task<List<AppointmentSlot>> GetAllSlotsAsync()
        {
            AttachCookie();
            return await _http.GetFromJsonAsync<List<AppointmentSlot>>("api/appointmentslot") ?? new();
        }

        public async Task<List<AppointmentSlot>> GetAvailableSlotsAsync()
        {
            var all = await GetAllSlotsAsync();
            return all.Where(s => s.IsAvailable == true).ToList();
        }

        public async Task<bool> CreateSlotAsync(AppointmentSlotDTO dto)
        {
            AttachCookie();
            var resp = await _http.PostAsJsonAsync("api/appointmentslot", dto);
            return resp.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateSlotAsync(string id, AppointmentSlotDTO dto)
        {
            AttachCookie();
            var resp = await _http.PutAsJsonAsync($"api/appointmentslot/{id}", dto);
            return resp.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteSlotAsync(string id)
        {
            AttachCookie();
            var resp = await _http.DeleteAsync($"api/appointmentslot/{id}");
            return resp.IsSuccessStatusCode;
        }
    }
}