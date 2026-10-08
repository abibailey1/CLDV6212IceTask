using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentApp.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : Controller {

        //Ensure that user is logged in and has the patient role
        [Authorize(Roles = "patient")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult PatientHome() {
            return StatusCode(200);
        }
    }
}
