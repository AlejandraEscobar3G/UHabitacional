/* ================================================================
   UHabitacional - 01_CreateDatabase.sql
   Crea la base de datos UnidadHabitacionalDB si no existe.
   ================================================================ */

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'UnidadHabitacionalDB')
BEGIN
    CREATE DATABASE [UnidadHabitacionalDB];
    PRINT 'Base de datos UnidadHabitacionalDB creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La base de datos UnidadHabitacionalDB ya existe.';
END
GO
