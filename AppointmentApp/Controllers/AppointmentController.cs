using AppointmentApp.Data;
using AppointmentApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AppointmentApp.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AppointmentController : Controller {

        private readonly ApplicationDbContext _context;

        public AppointmentController(ApplicationDbContext context) {
            _context = context;
        }

        //Get all appointments
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Appointment>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAppointments() {
            try {
                var appointments = await _context.Appointments.ToListAsync();

                if (appointments != null) {
                    return Ok(appointments);
                }
            }
            catch {
                return StatusCode(400);
            }

            return StatusCode(404);
        }

        //Get all appointments made by a specific patient
        [Authorize(Roles = "patient")]
        [HttpGet("patient")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Appointment>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPatientAppointments() {

            //Get logged in patient's email to identify them
            string? email = User.FindFirstValue(ClaimTypes.Name);

            if (email == null) {
                return StatusCode(400);
            }

            //Get the user object
            User? user;

            try {
                user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

                if (user == null) {
                    return StatusCode(400);
                }
            }
            catch {
                return StatusCode(400);
            }

            var patientAppointments = await _context.Appointments.Where(a => a.UserId == user.Userid).ToListAsync();

            return Ok(patientAppointments);
        }

        //Get a single appointment by appointment id
        [HttpGet("{appointmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Appointment))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAppointment(string appointmentId) {
            try {
                var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

                if (appointment != null) {
                    return Ok(appointment);
                }
            }
            catch {
                return StatusCode(400);
            }

            return StatusCode(404);
        }

        //Allows a patient to create an appointment
        [Authorize(Roles = "patient")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentDTO dto) {

            //Get logged in patient's email to identify them
            string? email = User.FindFirstValue(ClaimTypes.Name);
            User? user;
            AppointmentSlot? slot;

            if (email == null) {
                return StatusCode(400);
            }

            //Get userId and appointmentSlotId from database
            try {
                user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
                slot = await _context.AppointmentSlots.FirstOrDefaultAsync(s  => s.SlotId == dto.SlotId);

                if (user == null || slot == null) {
                    return StatusCode(400);
                }
            } catch {
                return StatusCode(400);
            }

            //Create new appointment
            var newAppointment = new Appointment {  AppointmentId = Guid.NewGuid().ToString(),
                                                    UserId = user.Userid,
                                                    SlotId = slot.SlotId,
                                                    AppointmentDate = slot.Date,
                                                    Status = "Booked"};

            try {
                //Store new appointment
                _context.Appointments.Add(newAppointment);
                await _context.SaveChangesAsync();

                return StatusCode(201);
            }
            catch {
                return StatusCode(400);
            }
        }

        //Allows a patient to update an appointment
        [Authorize(Roles = "patient")]
        [HttpPut("{appointmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAppointment(string appointmentId, [FromBody] AppointmentDTO dto) {
            try {
                //Try to find the existing appointment in the database and then update it
                var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

                if (appointment != null) {
                    //If updating timeslot
                    if (!string.IsNullOrEmpty(dto.SlotId)) {
                        var slot = await _context.AppointmentSlots.FirstOrDefaultAsync(s => s.SlotId == dto.SlotId);

                        if (slot == null) {
                            return StatusCode(400);
                        }

                        appointment.SlotId = dto.SlotId;
                        appointment.AppointmentDate = slot.Date;
                    }

                    //If updating status
                    if (!string.IsNullOrEmpty(dto.Status)) {
                        appointment.Status = dto.Status;
                    }

                    _context.Appointments.Update(appointment);
                    await _context.SaveChangesAsync();

                    return StatusCode(200);
                }
            }
            catch {
                return StatusCode(400);
            }

            return StatusCode(404);
        }

        //Allows both patient and staff to cancel an appointment
        [HttpDelete("{appointmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAppointment(string appointmentId) {
            try {
                //Try to find the existing appointment in the database and then delete it
                var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

                if (appointment != null) {
                    _context.Appointments.Remove(appointment);
                    await _context.SaveChangesAsync();

                    return StatusCode(200);
                }
            }
            catch {
                return StatusCode(400);
            }

            return StatusCode(404);
        }
    }
}