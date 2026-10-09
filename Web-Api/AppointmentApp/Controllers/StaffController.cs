using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentApp.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class StaffController : Controller {

        //Ensure that user is logged in and has the staff role
        [Authorize(Roles = "staff")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult StaffHome() {
            return StatusCode(200);
        }
    }
}
