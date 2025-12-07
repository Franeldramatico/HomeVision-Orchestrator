using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HomeVision.Gateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(ILogger<NotificationsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications([FromQuery] int limit = 20)
        {
            _logger.LogInformation("Fetching notifications (limit: {Limit})", limit);

            var notifications = new[]
            {
                new 
                { 
                    Id = 1, 
                    Type = "security",
                    Title = "🚪 Puerta Principal Abierta",
                    Message = "La puerta frontal fue abierta a las 18:32",
                    Timestamp = DateTime.UtcNow.AddMinutes(-15),
                    Priority = "high",
                    Read = false
                },
                new 
                { 
                    Id = 2, 
                    Type = "delivery",
                    Title = "📦 Delivery Llegó",
                    Message = "Paquete detectado en la entrada principal",
                    Timestamp = DateTime.UtcNow.AddHours(-2),
                    Priority = "medium",
                    Read = false
                },
                new 
                { 
                    Id = 3, 
                    Type = "reminder",
                    Title = "⏰ Recordatorio",
                    Message = "Regar las plantas del jardín",
                    Timestamp = DateTime.UtcNow.AddMinutes(-45),
                    Priority = "low",
                    Read = true
                },
                new 
                { 
                    Id = 4, 
                    Type = "climate",
                    Title = "🌡️ Temperatura Alta",
                    Message = "Dormitorio alcanzó 26°C, ajustando termostato",
                    Timestamp = DateTime.UtcNow.AddHours(-1),
                    Priority = "medium",
                    Read = true
                }
            };

            return Ok(notifications.Take(limit));
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationRequest request)
        {
            _logger.LogInformation("Creating notification: {Title}", request.Title);
            
            // TODO: Guardar en base de datos y transmitir vía SignalR
            var notification = new 
            { 
                Id = new Random().Next(1000, 9999),
                Type = request.Type,
                Title = request.Title,
                Message = request.Message,
                Timestamp = DateTime.UtcNow,
                Priority = request.Priority ?? "medium",
                Read = false
            };

            // TODO: Transmitir a clientes conectados (TV, web, móvil)
            // await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);

            return Created($"/api/notifications/{notification.Id}", notification);
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            _logger.LogInformation("Marking notification {NotificationId} as read", id);
            
            // TODO: Actualizar en base de datos
            return Ok(new { Id = id, Read = true });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            _logger.LogInformation("Deleting notification {NotificationId}", id);
            
            // TODO: Eliminar de base de datos
            return Ok(new { Id = id, Deleted = true });
        }
    }

    public class CreateNotificationRequest
    {
        public string Type { get; set; } = "info"; // info, security, delivery, reminder, climate
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Priority { get; set; } // low, medium, high
        public bool SendToTv { get; set; } = true;
    }
}
