using AppointmentApp.Data;
using AppointmentApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApp.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AppointmentSlotController : Controller {

        private readonly ApplicationDbContext _context;

        public AppointmentSlotController(ApplicationDbContext context) {
            _context = context;
        }

        //Get all appointment slots
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AppointmentSlot>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAppointmentSlots() {
            try {
                var slots = await _context.AppointmentSlots.ToListAsync();

                if (slots != null) {
                    return Ok(slots);
                }
            }
            catch {
                return StatusCode(400);
            }

            return StatusCode(404);
        }

        //Get a single appointment slot by its id
        [HttpGet("{slotId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AppointmentSlot))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAppointmentSlot(string slotId) {
            AppointmentSlot? slot;

            try {
                slot = await _context.AppointmentSlots.FirstOrDefaultAsync(s => s.SlotId == slotId);

                if (slot != null) {
                    return Ok(slot);
                }
            }
            catch {
                return StatusCode(400);
            }

            return StatusCode(404);
        }

        //Create an appointment slot
        [Authorize(Roles = "staff")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAppointmentSlot([FromBody] AppointmentSlotDTO dto) {
            var newSlot = new AppointmentSlot {
                SlotId = Guid.NewGuid().ToString(),
                Date = dto.Date,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                IsAvailable = dto.IsAvailable
            };

            try {
                //Store new slot
                _context.AppointmentSlots.Add(newSlot);
                await _context.SaveChangesAsync();

                return StatusCode(201);
            }
            catch {
                return StatusCode(400);
            }
        }

        //Update an existing appointment slot
        [Authorize(Roles = "staff")]
        [HttpPut("{slotId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAppointmentSlot(string slotId, [FromBody] AppointmentSlotDTO dto) {

            try {
                //Try to find the existing appointment slot in the database and then update it
                var slot = await _context.AppointmentSlots.FirstOrDefaultAsync(s => s.SlotId == slotId);

                if (slot != null) {
                    slot.Date = dto.Date;
                    slot.StartTime = dto.StartTime;
                    slot.EndTime = dto.EndTime;
                    slot.IsAvailable = dto.IsAvailable;

                    _context.AppointmentSlots.Update(slot);
                    await _context.SaveChangesAsync();

                    return StatusCode(200);
                }
            }
            catch {
                return StatusCode(400);
            }

            return StatusCode(404);
        }

        //Delete an existing appointment slot
        [Authorize(Roles = "staff")]
        [HttpDelete("{slotId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAppointmentSlot(string slotId) {

            try {
                //Try to find the existing appointment slot in the database and then delete it
                var slot = await _context.AppointmentSlots.FirstOrDefaultAsync(s => s.SlotId == slotId);

                if (slot != null) {
                    _context.AppointmentSlots.Remove(slot);
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