-- Scenes and Automation Rules
USE HomeVisionDB;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Scenes')
BEGIN
    CREATE TABLE Scenes (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(255) NULL,
        Icon NVARCHAR(10) NOT NULL DEFAULT '🏠',
        IsEnabled BIT NOT NULL DEFAULT 1,
        TriggerType NVARCHAR(20) NULL, -- time, event, gps, manual
        TriggerConfig NVARCHAR(MAX) NULL, -- JSON configuration
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );

    CREATE TABLE SceneActions (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        SceneId INT NOT NULL FOREIGN KEY REFERENCES Scenes(Id) ON DELETE CASCADE,
        DeviceId INT NOT NULL FOREIGN KEY REFERENCES Devices(Id) ON DELETE CASCADE,
        Command NVARCHAR(50) NOT NULL,
        Parameters NVARCHAR(MAX) NULL, -- JSON parameters
        DelayMs INT NOT NULL DEFAULT 0,
        [Order] INT NOT NULL DEFAULT 0
    );

    PRINT 'Scenes and SceneActions tables created';

    -- Insert sample scenes
    DECLARE @SceneId INT;

    INSERT INTO Scenes (Name, Description, Icon, TriggerType, TriggerConfig)
    VALUES ('Hola Casa', 'Enciende luces, café y ajusta clima', '🏠', 'time', '{"hour": 7, "minute": 0}');
    SET @SceneId = SCOPE_IDENTITY();
    INSERT INTO SceneActions (SceneId, DeviceId, Command, Parameters, [Order])
    VALUES 
        (@SceneId, 1, 'turnOn', '{"brightness": 100}', 1),
        (@SceneId, 2, 'setTemperature', '{"temp": 22}', 2);

    INSERT INTO Scenes (Name, Description, Icon, TriggerType)
    VALUES ('Modo Cine', 'Luces bajas, TV a Netflix', '🎬', 'manual');
    SET @SceneId = SCOPE_IDENTITY();
    INSERT INTO SceneActions (SceneId, DeviceId, Command, Parameters, [Order])
    VALUES (@SceneId, 1, 'dim', '{"brightness": 20}', 1);

    INSERT INTO Scenes (Name, Description, Icon, TriggerType, TriggerConfig)
    VALUES ('Buenas Noches', 'Apaga todo, activa alarmas', '🌙', 'time', '{"hour": 23, "minute": 0}');
    SET @SceneId = SCOPE_IDENTITY();
    INSERT INTO SceneActions (SceneId, DeviceId, Command, Parameters, [Order])
    VALUES 
        (@SceneId, 1, 'turnOff', NULL, 1),
        (@SceneId, 4, 'turnOff', NULL, 2);

    INSERT INTO Scenes (Name, Description, Icon, TriggerType)
    VALUES ('Llegada a Casa', 'Luces on, música suave', '🚪', 'gps');

    PRINT 'Sample scenes and actions inserted';
END
GO
