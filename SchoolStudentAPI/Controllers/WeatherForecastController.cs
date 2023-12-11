using Microsoft.AspNetCore.Mvc;

namespace SchoolStudentApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public IActionResult Index()
        {
            return new ContentResult()
            {
                Content = "<h1>School Application WebAPI</h1>",
                ContentType = "text/html",
            };
        }
    }
}
