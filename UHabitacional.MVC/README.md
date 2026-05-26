# UHabitacional.MVC

Front-end web del sistema **UHabitacional**, construido con **ASP.NET Core MVC (.NET 10)**. Consume directamente la API `UHabitacional.API` y entrega una experiencia diferenciada por rol (Administrador, Inquilino, Vigilante).

La identidad gráfica está basada en la guía **"Bosque Urbano"** (paleta verde profundo + cobre vibrante, logotipo de bloques residenciales y tipografía Plus Jakarta Sans + Inter), trasladada desde los mocks de diseño a Razor.

## Tecnologías

- .NET 10 (ASP.NET Core MVC)
- Razor Views + Tag Helpers
- HttpClient tipado para consumir la API
- Autenticación basada en sesión cookie + JWT obtenido de la API
- Identidad gráfica propia (sin frameworks CSS): CSS modular con variables de marca
- Plus Jakarta Sans + Inter (Google Fonts)

## Estructura

```
UHabitacional.MVC/
├── UHabitacional.MVC.csproj
├── Program.cs
├── appsettings.json
├── Controllers/
│   ├── AuthController.cs            (Login / Logout)
│   ├── HomeController.cs
│   ├── VigilantesController.cs      (Administrador)
│   ├── InquilinosController.cs      (Administrador)
│   ├── DepartamentosController.cs   (Administrador)
│   ├── EdificiosController.cs       (Administrador)
│   ├── PerfilesController.cs        (Administrador → Tipos de usuario)
│   ├── IdentificacionesController.cs(Administrador)
│   ├── VisitantesController.cs      (Inquilino)
│   ├── ChecadorController.cs        (Vigilante)
│   └── RegistroVisitanteController.cs (Vigilante)
├── Models/
│   └── ApiDtos.cs                   (DTOs alineados con UHabitacional.API)
├── ViewModels/
│   ├── BreadcrumbViewModel.cs
│   ├── ChecadorViewModel.cs
│   ├── InquilinoFormViewModel.cs
│   └── RegistroVisitanteViewModel.cs
├── Services/
│   ├── ApiClient.cs                 (HttpClient tipado + manejo de errores)
│   ├── CurrentUser.cs               (acceso a la sesión)
│   └── AuthorizeRoleAttribute.cs    (filtro de autorización por rol)
├── Views/
│   ├── _ViewImports.cshtml
│   ├── _ViewStart.cshtml
│   ├── Shared/
│   │   ├── _Layout.cshtml
│   │   ├── _Sidebar.cshtml
│   │   └── _Icons.cshtml            (íconos SVG inline reutilizables)
│   ├── Auth/Login.cshtml, AccessDenied.cshtml
│   ├── Home/Index.cshtml
│   ├── Vigilantes/ Index, Form
│   ├── Inquilinos/ Index, Form
│   ├── Departamentos/ Index, Form
│   ├── Edificios/ Index, Form
│   ├── Perfiles/ Index, Form
│   ├── Identificaciones/ Index, Form
│   ├── Visitantes/ Index, Form, Detail
│   ├── Checador/Index.cshtml
│   └── RegistroVisitante/Index.cshtml
└── wwwroot/
    ├── css/site.css
    └── js/site.js
```

## Identidad gráfica (Bosque Urbano)

| Token | Hex | Uso |
|-------|-----|-----|
| `--primary` | `#2D4A3E` | Color principal, botones primarios, navegación activa |
| `--primary-700` | `#1F3528` | Hover de primary y degradados |
| `--primary-50/100` | `#EAF0E8 / #D6E1D7` | Fondos sutiles, pills |
| `--accent` | `#CB6843` | Color de acento (cobre), CTAs secundarios, "U" del logo |
| `--soft` | `#A8C09A` | Verde suave decorativo |
| `--bg` | `#FBFAF7` | Fondo base |
| `--surface` | `#FFFFFF` | Superficies de tarjetas |
| `--ink` | `#1A1A1A` | Texto principal |
| `--muted` | `#6E6E6E` | Texto secundario |

Tipografía:
- **Plus Jakarta Sans** — interfaz, títulos, navegación
- **Inter** — datos tabulares, códigos monoespaciados (`code-display`, `cell-id`)

## Configuración

`appsettings.json`:

```json
{
  "ApiSettings": {
    "BaseUrl": "http://localhost:5000/"
  }
}
```

> Si la API está en otro host (Docker, IIS, etc.) actualiza `ApiSettings:BaseUrl`. El proyecto MVC ignora la base de datos: toda la información viaja por HTTP hacia la API.

## Cómo correr

1. Asegúrate de tener corriendo la API:

   ```bash
   cd ../UHabitacional.API
   dotnet run
   ```

2. En otra terminal:

   ```bash
   cd UHabitacional.MVC
   dotnet restore
   dotnet run
   ```

3. Abre `http://localhost:5080` (HTTP) o `https://localhost:7080` (HTTPS).

## Usuario inicial

Las credenciales sembradas por la API también sirven para entrar al MVC:

- Email: `admin@uhabitacional.com`
- Password: `Admin123!`

## Flujos por rol

### Administrador
- Vigilantes — alta, edición y baja de personal de seguridad.
- Inquilinos — gestión integral (usuario base + asignación de departamento).
- Departamentos / Edificios — catálogo físico de la unidad habitacional.
- Perfiles — tipos de usuario y permisos.
- Identificaciones — catálogo de documentos válidos.

### Inquilino
- **Mis visitantes** — registra un visitante, el sistema genera un **código único de visita** y lo muestra para compartirlo con el invitado.

### Vigilante
- **Checador** — entrada y salida de turno con reloj en vivo e historial.
- **Registro de visitante** — valida un código de visita y, si es correcto, registra entrada y posteriormente salida del visitante.

## Conexión con la API

| Endpoint | Uso en MVC |
|----------|------------|
| `POST /api/auth/login` | Login y obtención de JWT |
| `POST /api/sesion/logout` | Logout |
| `GET/POST/PUT/DELETE /api/edificios` | Módulo Edificios |
| `GET/POST/PUT/DELETE /api/departamentos` | Módulo Departamentos |
| `GET/POST/PUT/DELETE /api/usuarios` | Vigilantes (filtrado por rol) e Inquilinos (usuario base) |
| `GET/POST/PUT/DELETE /api/inquilinos` | Módulo Inquilinos |
| `PATCH /api/inquilinos/{id}/fecha-fin` | Baja de inquilino |
| `GET/POST/PUT/DELETE /api/tiposusuario` | Catálogo de perfiles |
| `GET/POST/PUT/DELETE /api/identificaciones` | Catálogo de identificaciones |
| `GET/POST/PUT/DELETE /api/bitacora-visitante` | Visitantes (inquilino) |
| `PATCH /api/bitacora-visitante/{id}/registro` | Entrada/salida desde el vigilante |
| `GET/POST/PUT /api/bitacora-vigilante` | Checador de turno |

El `ApiClient` adjunta automáticamente el JWT desde la sesión y traduce errores de la API (`ApiException`) a mensajes amigables que se muestran como `alert-danger` en las vistas.

## Notas

- Los iconos son SVG inline definidos en `Views/Shared/_Icons.cshtml`. Se invocan con `@@await Html.PartialAsync("_Icons", "nombre")`.
- El filtro `AuthorizeRoleAttribute` redirige a `/Auth/Login` si no hay sesión y a `/Auth/AccessDenied` si el rol no coincide.
- La vista `_Sidebar` solo muestra los módulos del rol activo (Administrador / Inquilino / Vigilante).
- Para extender el sistema (p. ej. añadir nuevos módulos), sigue el mismo patrón Controller + ViewModel + Form/Index Razor.
