using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;

namespace Hosting.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VerificationCodeController : ControllerBase
    {

        private readonly ILogger<VerificationCodeController> _logger;

        public VerificationCodeController(ILogger<VerificationCodeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Get()
        {
            using (var ms = new MemoryStream())
            {
                var code = VerificationCode.Generate(ms);
                var bytes = ms.GetBuffer();
                Response.Headers.Add("token", Guid.NewGuid().ToString("N"));
                return File(bytes, "image/png");
            }
        }
    }
}