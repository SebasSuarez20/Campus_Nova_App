# Base de datos CampusNovaDB (SQL Server)

Scripts generados a partir del backup de la base real. Ejecutar en este orden:

| # | Archivo | Contenido | Re-ejecutable |
|---|---|---|---|
| 1 | 01_create_database.sql | Crea la base CampusNovaDB | Si |
| 2 | 02_tables.sql | Esquemas academic y security, 8 tablas, llaves, defaults y CHECK | No (base vacia) |
| 3 | 03_views_procedures.sql | Vista vw_get_user_for_id y 9 procedimientos usp_* | Si |
| 4 | 04_synonyms.sql | 10 sinonimos dbo.syn_* que usa la API | Si |
| 5 | 05_triggers.sql | trg_student_subject_max_3 (maximo 3 materias) | Si |
| 6 | 06_seed_data.sql | Institucion, 5 profesores, 10 materias y rol Student | No (tablas vacias) |

Ejemplo (PowerShell, desde esta carpeta):

    Get-ChildItem 0*.sql | Sort-Object Name | ForEach-Object { sqlcmd -S .\SQLEXPRESS -E -C -f 65001 -b -i $_.FullName }

Los estudiantes, usuarios (security.users), inscripciones (student_subject) y audit_log
los crea la aplicacion al usarse; no se incluyen.