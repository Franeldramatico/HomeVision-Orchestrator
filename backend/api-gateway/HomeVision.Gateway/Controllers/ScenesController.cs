using Microsoft.AspNetCore.Mvc;

namespace HomeVision.Gateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScenesController : ControllerBase
    {
        private readonly ILogger<ScenesController> _logger;

        public ScenesController(ILogger<ScenesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllScenes()
        {
            _logger.LogInformation("Fetching all scenes");

            var scenes = new[]
            {
                new 
                { 
                    Id = 1, 
                    Name = "Hola Casa", 
                    Icon = "🏠",
                    Description = "Enciende luces, café y ajusta clima",
                    Enabled = true,
                    Trigger = "7:00 AM"
                },
                new 
                { 
                    Id = 2, 
                    Name = "Modo Cine", 
                    Icon = "🎬",
                    Description = "Luces bajas, TV a Netflix",
                    Enabled = true,
                    Trigger = "Manual"
                },
                new 
                { 
                    Id = 3, 
                    Name = "Buenas Noches", 
                    Icon = "🌙",
                    Description = "Apaga todo, activa alarmas",
                    Enabled = true,
                    Trigger = "11:00 PM"
                },
                new 
                { 
                    Id = 4, 
                    Name = "Llegada a Casa", 
                    Icon = "🚪",
                    Description = "Luces on, música suave",
                    Enabled = true,
                    Trigger = "GPS Detection"
                }
            };

            return Ok(scenes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetScene(int id)
        {
            _logger.LogInformation("Fetching scene {SceneId}", id);
            return Ok(new { Id = id, Name = "Scene " + id, Actions = new string[] { } });
        }

        [HttpPost]
        public async Task<IActionResult> CreateScene([FromBody] CreateSceneRequest request)
        {
            _logger.LogInformation("Creating new scene: {SceneName}", request.Name);
            
            // TODO: Guardar en el servicio de orquestación
            return Created($"/api/scenes/{1}", new { Id = 1, Name = request.Name });
        }

        [HttpPost("{id}/execute")]
        public async Task<IActionResult> ExecuteScene(int id)
        {
            _logger.LogInformation("Executing scene {SceneId}", id);
            
            // TODO: Activar servicio de orquestación para ejecutar escena
            return Ok(new { SceneId = id, Executed = true, Timestamp = DateTime.UtcNow });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateScene(int id, [FromBody] UpdateSceneRequest request)
        {
            _logger.LogInformation("Updating scene {SceneId}", id);
            
            // TODO: Actualizar en el servicio de orquestación
            return Ok(new { Id = id, Updated = true });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScene(int id)
        {
            _logger.LogInformation("Deleting scene {SceneId}", id);
            
            // TODO: Eliminar del servicio de orquestación
            return Ok(new { Id = id, Deleted = true });
        }
    }

    public class CreateSceneRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public List<SceneAction>? Actions { get; set; }
    }

    public class UpdateSceneRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? Enabled { get; set; }
        public List<SceneAction>? Actions { get; set; }
    }

    public class SceneAction
    {
        public int DeviceId { get; set; }
        public string Action { get; set; } = string.Empty;
        public Dictionary<string, object>? Parameters { get; set; }
        public int DelayMs { get; set; }
    }
}
