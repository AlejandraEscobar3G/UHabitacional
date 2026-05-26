/* ================================================================
   UHabitacional - 03_SeedData.sql
   Datos iniciales: catálogos, usuario Administrador, etc.

   IMPORTANTE:
   El PasswordHash debe ser generado con BCrypt (BCrypt.Net-Next).
   El hash incluido a continuación corresponde a la contraseña
   "Admin123!" - DEBE CAMBIARSE DESPUÉS DEL PRIMER LOGIN.

   Hash generado con BCrypt: workFactor=11
   ================================================================ */

USE [UnidadHabitacionalDB];
GO

-- ============================================================
-- Tipos de Usuario
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM dbo.TipoUsuario WHERE Nombre = N'Administrador')
    INSERT INTO dbo.TipoUsuario (Nombre, Descripcion, Activo)
    VALUES (N'Administrador', N'Usuario con permisos completos sobre el sistema.', 1);

IF NOT EXISTS (SELECT 1 FROM dbo.TipoUsuario WHERE Nombre = N'Vigilante')
    INSERT INTO dbo.TipoUsuario (Nombre, Descripcion, Activo)
    VALUES (N'Vigilante', N'Personal encargado de registrar entradas y salidas.', 1);

IF NOT EXISTS (SELECT 1 FROM dbo.TipoUsuario WHERE Nombre = N'Inquilino')
    INSERT INTO dbo.TipoUsuario (Nombre, Descripcion, Activo)
    VALUES (N'Inquilino', N'Residente de un departamento de la unidad habitacional.', 1);

PRINT 'Tipos de Usuario inicializados.';
GO

-- ============================================================
-- Identificaciones
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM dbo.Identificacion WHERE Nombre = N'INE')
    INSERT INTO dbo.Identificacion (Nombre, Descripcion, Activo)
    VALUES (N'INE', N'Credencial para votar emitida por el INE.', 1);

IF NOT EXISTS (SELECT 1 FROM dbo.Identificacion WHERE Nombre = N'Pasaporte')
    INSERT INTO dbo.Identificacion (Nombre, Descripcion, Activo)
    VALUES (N'Pasaporte', N'Pasaporte vigente.', 1);

IF NOT EXISTS (SELECT 1 FROM dbo.Identificacion WHERE Nombre = N'Licencia de Conducir')
    INSERT INTO dbo.Identificacion (Nombre, Descripcion, Activo)
    VALUES (N'Licencia de Conducir', N'Licencia de conducir vigente.', 1);

IF NOT EXISTS (SELECT 1 FROM dbo.Identificacion WHERE Nombre = N'Cedula Profesional')
    INSERT INTO dbo.Identificacion (Nombre, Descripcion, Activo)
    VALUES (N'Cedula Profesional', N'Cédula profesional emitida por la SEP.', 1);

PRINT 'Identificaciones inicializadas.';
GO

-- ============================================================
-- Usuario Administrador inicial
-- Email:    admin@uhabitacional.com
-- Password: Admin123!
-- ============================================================
DECLARE @IdTipoAdmin INT = (SELECT IdTipoUsuario FROM dbo.TipoUsuario WHERE Nombre = N'Administrador');
DECLARE @IdIdent INT     = (SELECT IdIdentificacion FROM dbo.Identificacion WHERE Nombre = N'INE');

IF NOT EXISTS (SELECT 1 FROM dbo.Usuario WHERE Email = N'admin@uhabitacional.com')
BEGIN
    INSERT INTO dbo.Usuario (
        IdTipoUsuario,
        IdIdentificacion,
        NumeroIdentificacion,
        Nombre,
        Apellidos,
        Email,
        PasswordHash,
        Telefono,
        Activo
    )
    VALUES (
        @IdTipoAdmin,
        @IdIdent,
        N'ADMIN-0001',
        N'Administrador',
        N'del Sistema',
        N'admin@uhabitacional.com',
        N'$2b$11$2tO.REDYAJ7Pnp9zuv73wOkJ9dapOL.9p9EzsqF/2dquLVnjcg/6K',  -- BCrypt para "Admin123!"
        N'5555555555',
        1
    );

    PRINT 'Usuario Administrador creado: admin@uhabitacional.com / Admin123!';
END
ELSE
BEGIN
    PRINT 'Usuario Administrador ya existe.';
END
GO

PRINT '==========================================================';
PRINT 'Script 03_SeedData.sql ejecutado correctamente.';
PRINT '==========================================================';
GO
