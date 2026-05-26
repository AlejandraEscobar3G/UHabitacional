# UHabitacional API

API para la gestión integral de accesos en una unidad habitacional. Permite el registro de visitantes, la consulta de información por parte de inquilinos y la verificación rápida por parte de vigilantes.

## Tecnologías

- .NET 10
- Entity Framework Core 10
- SQL Server
- Autenticación JWT (Bearer) con validación de sesión contra base de datos
- Swagger / OpenAPI
- BCrypt (hash de contraseñas)
- Arquitectura limpia (Domain / Application / Infrastructure / API)

## Estructura

```
UHabitacional.slnx
├── UHabitacional.API/
│   ├── UHabitacional.API.csproj
│   ├── Program.cs
│   ├── appsettings.json
│   ├── Domain/
│   │   ├── Entities/       (Identificacion, TipoUsuario, Edificio, Departamento,
│   │   │                    Usuario, Inquilino, BitacoraVigilante, BitacoraVisitante,
│   │   │                    Sesion)
│   │   ├── Exceptions/     (NotFound, Forbidden, BusinessRule, Validation)
│   │   └── Constants/      (RolesUsuario)
│   ├── Application/
│   │   ├── DTOs/
│   │   ├── Helpers/
│   │   ├── Interfaces/
│   │   │   ├── Repositories/
│   │   │   └── Services/
│   │   └── Services/       (incluye AuthService, SesionService)
│   ├── Infrastructure/
│   │   ├── Data/           (ApplicationDbContext)
│   │   ├── Repositories/
│   │   └── Security/       (JwtSettings, CurrentUserService)
│   ├── Controllers/        (incluye AuthController, SesionController)
│   └── Middleware/
│       ├── ExceptionHandlingMiddleware.cs
│       └── SessionValidationMiddleware.cs
└── Database/
    ├── 00_RunAll.sql              (instalación completa)
    ├── 01_CreateDatabase.sql
    ├── 02_CreateTables.sql        (incluye tabla Sesion)
    ├── 03_SeedData.sql
    ├── 04_DropDatabase.sql
    └── 05_AddSesionTable.sql      (migración de Sesion para BD existentes)
```

## Configuración

La cadena de conexión predeterminada está en `UHabitacional.API/appsettings.json`:

```
Server=localhost\SQLEXPRESS;Database=UnidadHabitacionalDB;Trusted_Connection=True;TrustServerCertificate=True;
```

## Base de datos

### Instalación nueva (primera vez)

Ejecuta `00_RunAll.sql` desde SSMS / Azure Data Studio, o desde la línea de comandos:

```bash
sqlcmd -S localhost\SQLEXPRESS -E -i Database\00_RunAll.sql
```

Esto crea la base `UnidadHabitacionalDB`, las nueve tablas (incluyendo `Sesion`) y los datos iniciales.

También puedes ejecutar paso a paso:

1. `Database/01_CreateDatabase.sql`
2. `Database/02_CreateTables.sql` (incluye `Sesion`)
3. `Database/03_SeedData.sql`

### Actualización de una BD ya existente

Si ya tienes la BD creada con la versión anterior (sin tabla `Sesion`), ejecuta solo:

```bash
sqlcmd -S localhost\SQLEXPRESS -E -d UnidadHabitacionalDB -i Database\05_AddSesionTable.sql
```

## Ejecutar la API

```bash
cd UHabitacional.API
dotnet restore
dotnet run
```

Luego abre `https://localhost:5001/swagger` (o `http://localhost:5000/swagger`).

## Usuario inicial

- Email: `admin@uhabitacional.com`
- Password: `Admin123!`

> Cambia esta contraseña tras el primer login.

## Roles

| Rol            | Permisos clave                                                                                                  |
|----------------|------------------------------------------------------------------------------------------------------------------|
| Administrador  | CRUD sobre catálogos (Identificacion, TipoUsuario, Edificio, Departamento, Usuario, Inquilino, FechaFin)        |
| Vigilante      | CU sobre BitacoraVigilante; PATCH de entrada/salida en BitacoraVisitante                                         |
| Inquilino      | C/U/Delete sobre sus propios registros en BitacoraVisitante                                                      |

## Autenticación y sesiones

### Login

`POST /api/auth/login`

```json
{ "email": "admin@uhabitacional.com", "password": "Admin123!" }
```

Respuesta:

```json
{
  "idUsuario": 1,
  "email": "admin@uhabitacional.com",
  "nombre": "Administrador del Sistema",
  "tipoUsuario": "Administrador",
  "token": "eyJhbGciOi...",
  "expira": "2026-05-25T17:00:00Z"
}
```

Internamente:
- Se valida el password con BCrypt.
- Se genera un JWT firmado con un claim `jti` único (GUID).
- Se persiste un registro en la tabla `Sesion` con `Jti`, `FechaInicio`, `FechaExpiracion`, `DireccionIP`, `UserAgent` y `Activa=1`.

### Validación de sesión en cada request

Todas las requests (excepto `/api/auth/login` y Swagger) pasan por el `SessionValidationMiddleware`, que:

1. Toma el JWT del header `Authorization: Bearer ...`.
2. Verifica firma y expiración criptográfica (esto lo hace `UseAuthentication`).
3. Extrae el `jti` y consulta la tabla `Sesion`:
   - Si no existe → 401.
   - Si `Activa = 0` o `FechaCierre != null` (logout previo) → 401.
   - Si `FechaExpiracion <= ahora` → marca la fila como inactiva y devuelve 401.
4. Actualiza `FechaUltimaActividad = ahora` y deja pasar la request.

Esto permite invalidar sesiones aunque el token JWT aún sea criptográficamente válido.

### Endpoints de sesión

| Método | Ruta                              | Descripción                                                          |
|--------|-----------------------------------|----------------------------------------------------------------------|
| GET    | `/api/sesion/activas`             | Lista las sesiones activas del usuario actual (marca la actual).     |
| POST   | `/api/sesion/logout`              | Cierra la sesión actual (invalida el JWT en la siguiente request).   |
| POST   | `/api/sesion/logout-todas`        | Cierra todas las sesiones del usuario.                               |

## Manejo de errores

| Excepción                       | HTTP |
|----------------------------------|------|
| `NotFoundException`              | 404  |
| `ForbiddenOperationException`    | 403  |
| `BusinessRuleException`          | 400  |
| `ValidationException`            | 400  |
| Sesión inválida / expirada       | 401  |
| Cualquier otra                   | 500  |

## Flujo end-to-end

1. Cliente: `POST /api/auth/login` con email + password.
2. API: valida BCrypt, genera JWT con `jti`, persiste `Sesion` en BD.
3. Cliente envía requests con `Authorization: Bearer <jwt>`.
4. `UseAuthentication()` valida firma y `exp` del JWT.
5. `SessionValidationMiddleware` valida que el `jti` exista como sesión activa y no expirada.
6. Si todo OK → llega al controller; si no → 401.
7. `POST /api/sesion/logout`: marca `Activa=0`, `FechaCierre=ahora`. El JWT físicamente sigue válido por criptografía hasta su `exp`, pero el middleware lo rechaza desde la próxima request.
