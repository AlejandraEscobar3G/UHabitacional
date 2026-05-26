/* ================================================================
   UHabitacional - 05_AddSesionTable.sql
   Migración: agrega la tabla Sesion para el manejo de sesiones JWT
   (validación contra BD mediante claim 'jti').

   Idempotente: puede ejecutarse varias veces sin error.
   ================================================================ */

USE [UnidadHabitacionalDB];
GO

IF OBJECT_ID('dbo.Sesion', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Sesion (
        IdSesion              INT IDENTITY(1,1) NOT NULL,
        IdUsuario             INT               NOT NULL,
        Jti                   NVARCHAR(64)      NOT NULL,
        FechaInicio           DATETIME2         NOT NULL CONSTRAINT DF_Sesion_FechaInicio DEFAULT (SYSUTCDATETIME()),
        FechaExpiracion       DATETIME2         NOT NULL,
        FechaUltimaActividad  DATETIME2         NOT NULL CONSTRAINT DF_Sesion_UltimaAct DEFAULT (SYSUTCDATETIME()),
        FechaCierre           DATETIME2         NULL,
        DireccionIP           NVARCHAR(45)      NULL,
        UserAgent             NVARCHAR(500)     NULL,
        Activa                BIT               NOT NULL CONSTRAINT DF_Sesion_Activa DEFAULT (1),
        CONSTRAINT PK_Sesion PRIMARY KEY CLUSTERED (IdSesion ASC),
        CONSTRAINT FK_Sesion_Usuario FOREIGN KEY (IdUsuario)
            REFERENCES dbo.Usuario(IdUsuario)
            ON DELETE CASCADE
    );

    PRINT 'Tabla Sesion creada.';
END
ELSE
BEGIN
    PRINT 'Tabla Sesion ya existe, no se realizan cambios en estructura.';
END
GO

-- Índices (también idempotentes)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_Sesion_Jti' AND object_id = OBJECT_ID('dbo.Sesion'))
BEGIN
    CREATE UNIQUE INDEX UX_Sesion_Jti ON dbo.Sesion(Jti);
    PRINT 'Indice UX_Sesion_Jti creado.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Sesion_Usuario_Activa' AND object_id = OBJECT_ID('dbo.Sesion'))
BEGIN
    CREATE INDEX IX_Sesion_Usuario_Activa ON dbo.Sesion(IdUsuario, Activa);
    PRINT 'Indice IX_Sesion_Usuario_Activa creado.';
END
GO

PRINT '==========================================================';
PRINT 'Migración 05_AddSesionTable.sql ejecutada correctamente.';
PRINT '==========================================================';
GO
