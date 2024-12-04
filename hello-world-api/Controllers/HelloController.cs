using Microsoft.AspNetCore.Mvc;

namespace hello_world_api.Controllers
{
    [Route("/")]
    public class HelloController : Controller
    {
        // GET /
        [HttpGet]
        public string Get()
        {
            return "Hello world!!!!!!";
        }
    }
}

