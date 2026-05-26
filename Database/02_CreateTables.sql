/* ================================================================
   UHabitacional - 02_CreateTables.sql
   Crea todas las tablas, claves primarias, foráneas e índices.
   ================================================================ */

USE [UnidadHabitacionalDB];
GO

-- ============================================================
-- Tabla: Identificacion
-- ============================================================
IF OBJECT_ID('dbo.Identificacion', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Identificacion (
        IdIdentificacion   INT IDENTITY(1,1) NOT NULL,
        Nombre             NVARCHAR(100)     NOT NULL,
        Descripcion        NVARCHAR(250)     NULL,
        Activo             BIT               NOT NULL CONSTRAINT DF_Identificacion_Activo DEFAULT (1),
        FechaCreacion      DATETIME2         NOT NULL CONSTRAINT DF_Identificacion_FechaCreacion DEFAULT (SYSUTCDATETIME()),
        FechaModificacion  DATETIME2         NULL,
        CONSTRAINT PK_Identificacion PRIMARY KEY CLUSTERED (IdIdentificacion ASC)
    );

    CREATE UNIQUE INDEX UX_Identificacion_Nombre ON dbo.Identificacion(Nombre);
    PRINT 'Tabla Identificacion creada.';
END
GO

-- ============================================================
-- Tabla: TipoUsuario
-- ============================================================
IF OBJECT_ID('dbo.TipoUsuario', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.TipoUsuario (
        IdTipoUsuario      INT IDENTITY(1,1) NOT NULL,
        Nombre             NVARCHAR(50)      NOT NULL,
        Descripcion        NVARCHAR(250)     NULL,
        Activo             BIT               NOT NULL CONSTRAINT DF_TipoUsuario_Activo DEFAULT (1),
        FechaCreacion      DATETIME2         NOT NULL CONSTRAINT DF_TipoUsuario_FechaCreacion DEFAULT (SYSUTCDATETIME()),
        FechaModificacion  DATETIME2         NULL,
        CONSTRAINT PK_TipoUsuario PRIMARY KEY CLUSTERED (IdTipoUsuario ASC)
    );

    CREATE UNIQUE INDEX UX_TipoUsuario_Nombre ON dbo.TipoUsuario(Nombre);
    PRINT 'Tabla TipoUsuario creada.';
END
GO

-- ============================================================
-- Tabla: Edificio
-- ============================================================
IF OBJECT_ID('dbo.Edificio', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Edificio (
        IdEdificio         INT IDENTITY(1,1) NOT NULL,
        Nombre             NVARCHAR(100)     NOT NULL,
        Descripcion        NVARCHAR(250)     NULL,
        NumeroPisos        INT               NOT NULL,
        TotalDeptos        INT               NOT NULL,
        FechaCreacion      DATETIME2         NOT NULL CONSTRAINT DF_Edificio_FechaCreacion DEFAULT (SYSUTCDATETIME()),
        FechaModificacion  DATETIME2         NULL,
        CONSTRAINT PK_Edificio PRIMARY KEY CLUSTERED (IdEdificio ASC),
        CONSTRAINT CK_Edificio_NumeroPisos CHECK (NumeroPisos >= 1),
        CONSTRAINT CK_Edificio_TotalDeptos CHECK (TotalDeptos >= 1)
    );

    CREATE UNIQUE INDEX UX_Edificio_Nombre ON dbo.Edificio(Nombre);
    PRINT 'Tabla Edificio creada.';
END
GO

-- ============================================================
-- Tabla: Departamento
-- ============================================================
IF OBJECT_ID('dbo.Departamento', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Departamento (
        IdDepartamento       INT IDENTITY(1,1) NOT NULL,
        IdEdificio           INT               NOT NULL,
        NumeroDepartamento   NVARCHAR(20)      NOT NULL,
        Piso                 INT               NOT NULL,
        FechaCreacion        DATETIME2         NOT NULL CONSTRAINT DF_Departamento_FechaCreacion DEFAULT (SYSUTCDATETIME()),
        FechaModificacion    DATETIME2         NULL,
        CONSTRAINT PK_Departamento PRIMARY KEY CLUSTERED (IdDepartamento ASC),
        CONSTRAINT FK_Departamento_Edificio FOREIGN KEY (IdEdificio)
            REFERENCES dbo.Edificio(IdEdificio),
        CONSTRAINT CK_Departamento_Piso CHECK (Piso >= 1)
    );

    CREATE UNIQUE INDEX UX_Departamento_Edificio_Numero
        ON dbo.Departamento(IdEdificio, NumeroDepartamento);
    PRINT 'Tabla Departamento creada.';
END
GO

-- ============================================================
-- Tabla: Usuario
-- ============================================================
IF OBJECT_ID('dbo.Usuario', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuario (
        IdUsuario             INT IDENTITY(1,1) NOT NULL,
        IdTipoUsuario         INT               NOT NULL,
        IdIdentificacion      INT               NOT NULL,
        NumeroIdentificacion  NVARCHAR(50)      NOT NULL,
        Nombre                NVARCHAR(100)     NOT NULL,
        Apellidos             NVARCHAR(150)     NOT NULL,
        Email                 NVARCHAR(150)     NOT NULL,
        PasswordHash          NVARCHAR(255)     NOT NULL,
        Telefono              NVARCHAR(20)      NULL,
        Activo                BIT               NOT NULL CONSTRAINT DF_Usuario_Activo DEFAULT (1),
        FechaCreacion         DATETIME2         NOT NULL CONSTRAINT DF_Usuario_FechaCreacion DEFAULT (SYSUTCDATETIME()),
        FechaModificacion     DATETIME2         NULL,
        CONSTRAINT PK_Usuario PRIMARY KEY CLUSTERED (IdUsuario ASC),
        CONSTRAINT FK_Usuario_TipoUsuario FOREIGN KEY (IdTipoUsuario)
            REFERENCES dbo.TipoUsuario(IdTipoUsuario),
        CONSTRAINT FK_Usuario_Identificacion FOREIGN KEY (IdIdentificacion)
            REFERENCES dbo.Identificacion(IdIdentificacion)
    );

    CREATE UNIQUE INDEX UX_Usuario_Email ON dbo.Usuario(Email);
    CREATE UNIQUE INDEX UX_Usuario_Identificacion
        ON dbo.Usuario(IdIdentificacion, NumeroIdentificacion);
    PRINT 'Tabla Usuario creada.';
END
GO

-- ============================================================
-- Tabla: Inquilino
-- ============================================================
IF OBJECT_ID('dbo.Inquilino', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Inquilino (
        IdInquilino        INT IDENTITY(1,1) NOT NULL,
        IdUsuario          INT               NOT NULL,
        IdDepartamento     INT               NOT NULL,
        FechaInicio        DATETIME2         NOT NULL CONSTRAINT DF_Inquilino_FechaInicio DEFAULT (SYSUTCDATETIME()),
        FechaFin           DATETIME2         NULL,
        Activo             BIT               NOT NULL CONSTRAINT DF_Inquilino_Activo DEFAULT (1),
        FechaCreacion      DATETIME2         NOT NULL CONSTRAINT DF_Inquilino_FechaCreacion DEFAULT (SYSUTCDATETIME()),
        FechaModificacion  DATETIME2         NULL,
        CONSTRAINT PK_Inquilino PRIMARY KEY CLUSTERED (IdInquilino ASC),
        CONSTRAINT FK_Inquilino_Usuario FOREIGN KEY (IdUsuario)
            REFERENCES dbo.Usuario(IdUsuario),
        CONSTRAINT FK_Inquilino_Departamento FOREIGN KEY (IdDepartamento)
            REFERENCES dbo.Departamento(IdDepartamento)
    );

    CREATE UNIQUE INDEX UX_Inquilino_Usuario ON dbo.Inquilino(IdUsuario);
    CREATE INDEX IX_Inquilino_Departamento ON dbo.Inquilino(IdDepartamento);
    PRINT 'Tabla Inquilino creada.';
END
GO

-- ============================================================
-- Tabla: BitacoraVigilante
-- ============================================================
IF OBJECT_ID('dbo.BitacoraVigilante', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.BitacoraVigilante (
        IdBitacoraVigilante INT IDENTITY(1,1) NOT NULL,
        IdUsuario           INT               NOT NULL,
        FechaHoraEntrada    DATETIME2         NOT NULL,
        FechaHoraSalida     DATETIME2         NULL,
        Observaciones       NVARCHAR(500)     NULL,
        FechaCreacion       DATETIME2         NOT NULL CONSTRAINT DF_BitVigilante_FechaCreacion DEFAULT (SYSUTCDATETIME()),
        FechaModificacion   DATETIME2         NULL,
        CONSTRAINT PK_BitacoraVigilante PRIMARY KEY CLUSTERED (IdBitacoraVigilante ASC),
        CONSTRAINT FK_BitVigilante_Usuario FOREIGN KEY (IdUsuario)
            REFERENCES dbo.Usuario(IdUsuario)
    );

    CREATE INDEX IX_BitVigilante_Usuario_Fecha
        ON dbo.BitacoraVigilante(IdUsuario, FechaHoraEntrada DESC);
    PRINT 'Tabla BitacoraVigilante creada.';
END
GO

-- ============================================================
-- Tabla: BitacoraVisitante
-- ============================================================
IF OBJECT_ID('dbo.BitacoraVisitante', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.BitacoraVisitante (
        IdBitacoraVisitante INT IDENTITY(1,1) NOT NULL,
        IdInquilino         INT               NOT NULL,
        NombreVisitante     NVARCHAR(150)     NOT NULL,
        IdIdentificacion    INT               NOT NULL,
        NumeroIdentificacion NVARCHAR(50)     NOT NULL,
        CodigoVisita        NVARCHAR(6)       NOT NULL,
        FechaHoraLlegada    DATETIME2         NULL,
        FechaHoraSalida     DATETIME2         NULL,
        Observaciones       NVARCHAR(500)     NULL,
        IdVigilanteEntrada  INT               NULL,
        IdVigilanteSalida   INT               NULL,
        Activo              BIT               NOT NULL CONSTRAINT DF_BitVisitante_Activo DEFAULT (1),
        FechaCreacion       DATETIME2         NOT NULL CONSTRAINT DF_BitVisitante_FechaCreacion DEFAULT (SYSUTCDATETIME()),
        FechaModificacion   DATETIME2         NULL,
        CONSTRAINT PK_BitacoraVisitante PRIMARY KEY CLUSTERED (IdBitacoraVisitante ASC),
        CONSTRAINT FK_BitVisitante_Inquilino FOREIGN KEY (IdInquilino)
            REFERENCES dbo.Inquilino(IdInquilino),
        CONSTRAINT FK_BitVisitante_Identificacion FOREIGN KEY (IdIdentificacion)
            REFERENCES dbo.Identificacion(IdIdentificacion),
        CONSTRAINT FK_BitVisitante_VigilanteEntrada FOREIGN KEY (IdVigilanteEntrada)
            REFERENCES dbo.Usuario(IdUsuario),
        CONSTRAINT FK_BitVisitante_VigilanteSalida FOREIGN KEY (IdVigilanteSalida)
            REFERENCES dbo.Usuario(IdUsuario)
    );

    CREATE INDEX IX_BitVisitante_Codigo ON dbo.BitacoraVisitante(CodigoVisita);
    CREATE INDEX IX_BitVisitante_Inquilino ON dbo.BitacoraVisitante(IdInquilino);
    PRINT 'Tabla BitacoraVisitante creada.';
END
GO

-- ============================================================
-- Tabla: Sesion
-- Rastrea sesiones activas asociadas a JWTs (claim 'jti').
-- ============================================================
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

    CREATE UNIQUE INDEX UX_Sesion_Jti ON dbo.Sesion(Jti);
    CREATE INDEX IX_Sesion_Usuario_Activa ON dbo.Sesion(IdUsuario, Activa);
    PRINT 'Tabla Sesion creada.';
END
GO

PRINT '==========================================================';
PRINT 'Script 02_CreateTables.sql ejecutado correctamente.';
PRINT '==========================================================';
GO
