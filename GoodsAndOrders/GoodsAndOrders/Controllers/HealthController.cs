using Microsoft.AspNetCore.Mvc;

namespace GoodsAndOrders.Controllers
{
    [Route("api/health")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            //Console.WriteLine("Health endpoint hit!");
            //Response.Headers.Remove("ETag");
            //Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            //Response.Headers.Add("Pragma", "no-cache");
            //Response.Headers.Add("Expires", "0");
            return Ok(new { status = "ok" });
        }
    }
}
