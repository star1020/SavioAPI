using Microsoft.AspNetCore.Mvc;

namespace Savio.API.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult HandleError() =>
            Problem(); 
    }
}
