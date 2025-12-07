-- Devices Table
USE HomeVisionDB;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Devices')
BEGIN
    CREATE TABLE Devices (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Type NVARCHAR(50) NOT NULL, -- Light, Thermostat, Camera, SmartPlug, Lock, Sensor
        Brand NVARCHAR(50) NULL,
        Model NVARCHAR(50) NULL,
        Room NVARCHAR(50) NOT NULL,
        Status NVARCHAR(20) NOT NULL DEFAULT 'Offline', -- Online, Offline, Error
        IsEnabled BIT NOT NULL DEFAULT 1,
        Properties NVARCHAR(MAX) NULL, -- JSON object for device-specific properties
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        LastSeenAt DATETIME2 NULL
    );

    PRINT 'Devices table created';

    -- Insert sample devices
    INSERT INTO Devices (Name, Type, Brand, Room, Status, Properties)
    VALUES 
        ('Living Room Light', 'Light', 'Philips Hue', 'Living Room', 'Online', '{"brightness": 80, "color": "#FFFFFF"}'),
        ('Bedroom Thermostat', 'Thermostat', 'Nest', 'Bedroom', 'Online', '{"temperature": 22, "mode": "auto"}'),
        ('Front Door Camera', 'Camera', 'Ring', 'Entrance', 'Online', '{"resolution": "1080p", "recording": true}'),
        ('Kitchen Smart Plug', 'SmartPlug', 'TP-Link', 'Kitchen', 'Online', '{"power": true, "watts": 45}');

    PRINT 'Sample devices inserted';
END
GO
