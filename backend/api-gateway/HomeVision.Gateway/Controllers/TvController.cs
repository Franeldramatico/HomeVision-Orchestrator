using Microsoft.AspNetCore.Mvc;

namespace HomeVision.Gateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TvController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<TvController> _logger;
        private readonly IConfiguration _configuration;

        public TvController(
            IHttpClientFactory httpClientFactory, 
            ILogger<TvController> logger,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetTvStatus()
        {
            _logger.LogInformation("Fetching TV status");

            // TODO: Llamar al servicio de integración TV
            var status = new
            {
                PowerState = "On",
                CurrentApp = "Netflix",
                Volume = 35,
                IsMuted = false,
                CurrentInput = "HDMI 1",
                Model = "Samsung UE32T430A",
                IpAddress = _configuration["SAMSUNG_TV_IP"] ?? "192.168.1.100"
            };

            return Ok(status);
        }

        [HttpPost("power")]
        public async Task<IActionResult> TogglePower([FromBody] PowerRequest request)
        {
            _logger.LogInformation("Toggle TV power: {PowerState}", request.PowerOn);

            // TODO: Llamar al servicio de integración TV
            var tvServiceUrl = _configuration["Services:TvIntegration"] ?? "http://tv-integration-service:3000";
            
            return Ok(new { Success = true, PowerState = request.PowerOn ? "On" : "Off" });
        }

        [HttpPost("volume")]
        public async Task<IActionResult> SetVolume([FromBody] VolumeRequest request)
        {
            _logger.LogInformation("Setting TV volume to {Volume}", request.Level);

            // TODO: Llamar al servicio de integración TV
            return Ok(new { Success = true, Volume = request.Level });
        }

        [HttpPost("launch-app")]
        public async Task<IActionResult> LaunchApp([FromBody] LaunchAppRequest request)
        {
            _logger.LogInformation("Launching app: {AppId}", request.AppId);

            // TODO: Llamar al servicio de integración TV para lanzar app
            var appMapping = new Dictionary<string, string>
            {
                { "netflix", "Netflix" },
                { "youtube", "YouTube" },
                { "homevision", "HomeVision Dashboard" }
            };

            var appName = appMapping.GetValueOrDefault(request.AppId, request.AppId);
            
            return Ok(new { Success = true, LaunchedApp = appName });
        }

        [HttpPost("display-notification")]
        public async Task<IActionResult> DisplayNotification([From Body] TvNotificationRequest request)
        {
            _logger.LogInformation("Displaying notification on TV: {Title}", request.Title);

            // TODO: Llamar al servicio de integración TV para mostrar notificación superpuesta
            return Ok(new { Success = true, DisplayedAt = DateTime.UtcNow });
        }

        [HttpPost("input")]
        public async Task<IActionResult> SwitchInput([FromBody] InputRequest request)
        {
            _logger.LogInformation("Switching TV input to: {Input}", request.InputSource);

            // TODO: Llamar al servicio de integración TV
            return Ok(new { Success = true, CurrentInput = request.InputSource });
        }
    }

    public class PowerRequest
    {
        public bool PowerOn { get; set; }
    }

    public class VolumeRequest
    {
        public int Level { get; set; } // 0-100
    }

    public class LaunchAppRequest
    {
        public string AppId { get; set; } = string.Empty;
    }

    public class TvNotificationRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int DurationSeconds { get; set; } = 5;
        public string Type { get; set; } = "info"; // info, warning, success, error
    }

    public class InputRequest
    {
        public string InputSource { get; set; } = string.Empty; // HDMI 1, HDMI 2, USB, etc.
    }
}
