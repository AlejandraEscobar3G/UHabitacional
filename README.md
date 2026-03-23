# 🏢 UHabitacional

Aplicación de gestión para unidades habitacionales, enfocada en la administración de accesos de visitas, inquilinos, vigilantes y administradores.  
Este sistema busca ofrecer un control claro y seguro de la información relacionada con residentes y visitantes, apoyándose en un modelo de datos robusto.

---

## 🚀 Características principales

- **Gestión de usuarios**: administración de inquilinos, vigilantes y administradores con diferentes tipos de roles.
- **Control de accesos**: registro de visitas con bitácoras detalladas (entrada, salida, identificación).
- **Administración de edificios y departamentos**: relación clara entre edificios, departamentos e inquilinos.
- **Bitácoras**:
  - Vigilantes: registro de actividades.
  - Visitantes: control de accesos con identificación y horarios.
- **Seguridad y trazabilidad**: cada entidad incluye campos de auditoría (`CreatedAt`, `CreatedBy`, `ModifyAt`, `ModifyBy`).

---

## 🗂 Modelo de datos (ERD)

El sistema se basa en el siguiente **Diagrama Entidad-Relación (ERD)**:

- **Usuarios**: información básica de personas con rol en el sistema.
- **TiposUsuario**: define los roles (administrador, vigilante, inquilino, visitante).
- **BitacoraVigilante**: registro de actividades de los vigilantes.
- **Identificacion**: catálogo de tipos de identificación para visitantes.
- **BitacoraVisitante**: registro de accesos de visitantes a departamentos.
- **Edificio**: información de cada edificio dentro de la unidad habitacional.
- **Departamento**: departamentos asociados a un edificio.
- **Inquilino**: relación entre usuarios y departamentos, con fechas de inicio y fin de estancia.

---

## 🛠 Tecnologías utilizadas

- **Backend**: ASP.NET Core.
- **Frontend**: Razor Pages + jQuery para interacción dinámica.
- **Base de datos**: SQL Server.
- **Despliegue**: IIS.
- **Arquitectura**: Microservicios y modelos de entidades escalables.

---

## 📌 Instalación y ejecución

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/tuusuario/UHabitacional.git

2. Configurar la base de datos:
    ```bash
    appsettings.json

3. Ejecutar migraciones para crear las tablas.

    ```bash
    Update-Database

4. Levantar el proyecto en IIS o mediante `dotnet run`.

---

## 👩‍💻 Autor
**Alejandra Escobar Rodríguez.**

Fullstack Developer especializada en ASP.NET Core, Java Spring Boot y arquitecturas escalables.
