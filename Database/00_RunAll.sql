/* ================================================================
   UHabitacional - 00_RunAll.sql
   Script maestro que crea la base de datos, todas las tablas
   (incluida la tabla Sesion para el manejo de sesiones JWT) y
   los datos iniciales (catálogos + usuario Administrador).

   Tablas creadas por el script 02:
     1. Identificacion
     2. TipoUsuario
     3. Edificio
     4. Departamento
     5. Usuario
     6. Inquilino
     7. BitacoraVigilante
     8. BitacoraVisitante
     9. Sesion                  <-- manejo de sesiones (JWT jti)

   Para ejecutar paso a paso, utilice los archivos individuales:
     01_CreateDatabase.sql
     02_CreateTables.sql       (incluye la tabla Sesion)
     03_SeedData.sql

   Migración independiente (solo agrega tabla Sesion en BD existentes):
     05_AddSesionTable.sql

   Ejecución desde sqlcmd:
     sqlcmd -S localhost\SQLEXPRESS -E -i 00_RunAll.sql
   ================================================================ */

-- 1) Crear base de datos
:r .\01_CreateDatabase.sql
GO

-- 2) Crear tablas (incluye Sesion)
:r .\02_CreateTables.sql
GO

-- 3) Datos iniciales
:r .\03_SeedData.sql
GO

-- 4) Crear tabla de sesion
:r .\04_AddSesionTable.sql
GO

PRINT '==========================================================';
PRINT 'Instalacion completa: BD, tablas (con Sesion) y datos.';
PRINT '==========================================================';
GO
