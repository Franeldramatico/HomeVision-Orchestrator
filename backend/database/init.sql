-- HomeVision Orchestrator Database Initialization
-- SQL Server 2022+

USE master;
GO

-- Create database if not exists
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'HomeVisionDB')
BEGIN
    CREATE DATABASE HomeVisionDB;
END
GO

USE HomeVisionDB;
GO

PRINT 'HomeVision Database initialized successfully';
GO
