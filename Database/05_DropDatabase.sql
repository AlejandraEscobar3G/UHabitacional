/* ================================================================
   UHabitacional - 04_DropDatabase.sql
   Script de utilidad para eliminar la base de datos (USAR CON CUIDADO).
   ================================================================ */

USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'UnidadHabitacionalDB')
BEGIN
    ALTER DATABASE [UnidadHabitacionalDB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [UnidadHabitacionalDB];
    PRINT 'Base de datos UnidadHabitacionalDB eliminada.';
END
GO
