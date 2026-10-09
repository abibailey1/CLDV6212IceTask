using AppointmentApp.Models;
using AppointmentApp.Services;
using AppointmentApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentApp.Controllers
{
    [Authorize(Roles = "patient")]
    public class PatientController : Controller
    {
        private readonly IApiClient _api;

        public PatientController(IApiClient api) => _api = api;

        public async Task<IActionResult> Dashboard()
        {
            var appts = await _api.GetMyAppointmentsAsync();
            return View(appts);
        }

        [HttpGet]
        public async Task<IActionResult> Book()
        {
            ViewBag.Slots = await _api.GetAvailableSlotsAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Book(BookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Slots = await _api.GetAvailableSlotsAsync();
                return View(model);
            }

            var ok = await _api.CreateAppointmentAsync(new AppointmentDTO
            {
                SlotId = model.SlotId,
                Status = "Booked"
            });

            if (!ok)
            {
                ModelState.AddModelError("", "Booking failed. The slot may no longer be available.");
                ViewBag.Slots = await _api.GetAvailableSlotsAsync();
                return View(model);
            }

            return RedirectToAction(nameof(Dashboard));
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(string id)
        {
            await _api.CancelAppointmentAsync(id);
            return RedirectToAction(nameof(Dashboard));
        }
    }
}