namespace HomeVision.Common.Models
{
    public enum DeviceType
    {
        Light,
        Thermostat,
        Camera,
        SmartPlug,
        Lock,
        Sensor,
        Other
    }

    public enum DeviceStatus
    {
        Online,
        Offline,
        Error
    }

    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DeviceType Type { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Room { get; set; } = string.Empty;
        public DeviceStatus Status { get; set; }
        public bool IsEnabled { get; set; } = true;
        public Dictionary<string, object> Properties { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastSeenAt { get; set; }
    }

    public class Scene
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = "🏠";
        public bool IsEnabled { get; set; } = true;
        public List<SceneAction> Actions { get; set; } = new();
        public SceneTrigger? Trigger { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class SceneAction
    {
        public int DeviceId { get; set; }
        public string Command { get; set; } = string.Empty;
        public Dictionary<string, object> Parameters { get; set; } = new();
        public int DelayMs { get; set; } = 0;
        public int Order { get; set; }
    }

    public class SceneTrigger
    {
        public string Type { get; set; } = string.Empty; // "time", "event", "gps", "manual"
        public Dictionary<string, object> Config { get; set; } = new();
    }

    public class Notification
    {
        public int Id { get; set; }
        public string Type { get; set; } = "info"; // info, security, delivery, reminder, climate
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Priority { get; set; } = "medium"; // low, medium, high
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReadAt { get; set; }
        public Dictionary<string, object>? Metadata { get; set; }
    }
}
