/* ============================================================================
   CampusNovaDB - 01. Crear la base de datos
   Ejecutar primero, conectado a master. Ejemplo:
   sqlcmd -S .\SQLEXPRESS -E -C -f 65001 -i 01_create_database.sql
   ========================================================================== */
IF DB_ID('CampusNovaDB') IS NULL
    CREATE DATABASE [CampusNovaDB];
GO