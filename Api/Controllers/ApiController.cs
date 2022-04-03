using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class ApiController : ControllerBase
    {
        [Authorize]
        public IActionResult Secret()
        {
            return Ok("Api Secret");
        }

        public IActionResult Index()
        {
            return Ok("Index");
        }
    }
}
