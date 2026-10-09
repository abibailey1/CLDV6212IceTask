using AppointmentApp.Models;
using AppointmentApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentApp.Controllers
{
    [Authorize(Roles = "staff")]
    public class StaffController : Controller
    {
        private readonly IApiClient _api;

        public StaffController(IApiClient api) => _api = api;

        public async Task<IActionResult> Dashboard()
        {
            var appts = await _api.GetAllAppointmentsAsync();
            return View(appts);
        }

        [HttpGet]
        public async Task<IActionResult> EditAppointment(string id)
        {
            var appt = await _api.GetAppointmentAsync(id);
            if (appt is null) return NotFound();

            ViewBag.Slots = await _api.GetAllSlotsAsync();
            return View(appt);
        }

        [HttpPost]
        public async Task<IActionResult> EditAppointment(string id, string slotId, string status)
        {
            await _api.UpdateAppointmentAsync(id, new AppointmentDTO
            {
                SlotId = slotId,
                Status = status
            });

            return RedirectToAction(nameof(Dashboard));
        }

        public async Task<IActionResult> Slots()
        {
            var slots = await _api.GetAllSlotsAsync();
            return View(slots);
        }

        [HttpGet]
        public IActionResult CreateSlot() => View();

        [HttpPost]
        public async Task<IActionResult> CreateSlot(AppointmentSlotDTO dto)
        {
            await _api.CreateSlotAsync(dto);
            return RedirectToAction(nameof(Slots));
        }

        [HttpGet]
        public async Task<IActionResult> EditSlot(string id)
        {
            var slots = await _api.GetAllSlotsAsync();
            var slot = slots.FirstOrDefault(s => s.SlotId == id);
            if (slot is null) return NotFound();
            return View(slot);
        }

        [HttpPost]
        public async Task<IActionResult> EditSlot(string id, AppointmentSlotDTO dto)
        {
            await _api.UpdateSlotAsync(id, dto);
            return RedirectToAction(nameof(Slots));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSlot(string id)
        {
            await _api.DeleteSlotAsync(id);
            return RedirectToAction(nameof(Slots));
        }
    }
}