USE [CampusNovaDB]
GO

/* 04. Sinonimos dbo.syn_* que usa la API (Dapper). Se puede ejecutar varias veces. */

IF OBJECT_ID('dbo.syn_GetForUserIdView', 'SN') IS NULL
    CREATE SYNONYM [dbo].[syn_GetForUserIdView] FOR [security].[vw_get_user_for_id];
GO
IF OBJECT_ID('dbo.syn_sp_DeleteStudent', 'SN') IS NULL
    CREATE SYNONYM [dbo].[syn_sp_DeleteStudent] FOR [academic].[usp_delete_student];
GO
IF OBJECT_ID('dbo.syn_sp_DeleteStudentSubject', 'SN') IS NULL
    CREATE SYNONYM [dbo].[syn_sp_DeleteStudentSubject] FOR [academic].[usp_delete_student_subject];
GO
IF OBJECT_ID('dbo.syn_sp_GetAccessToSystem', 'SN') IS NULL
    CREATE SYNONYM [dbo].[syn_sp_GetAccessToSystem] FOR [security].[usp_get_access_to_system];
GO
IF OBJECT_ID('dbo.syn_sp_GetStudentClasses', 'SN') IS NULL
    CREATE SYNONYM [dbo].[syn_sp_GetStudentClasses] FOR [academic].[usp_get_student_classes];
GO
IF OBJECT_ID('dbo.syn_sp_GetStudents', 'SN') IS NULL
    CREATE SYNONYM [dbo].[syn_sp_GetStudents] FOR [academic].[usp_get_students];
GO
IF OBJECT_ID('dbo.syn_sp_GetSubjects', 'SN') IS NULL
    CREATE SYNONYM [dbo].[syn_sp_GetSubjects] FOR [academic].[usp_get_subjects];
GO
IF OBJECT_ID('dbo.syn_sp_InsertStudent', 'SN') IS NULL
    CREATE SYNONYM [dbo].[syn_sp_InsertStudent] FOR [academic].[usp_insert_student];
GO
IF OBJECT_ID('dbo.syn_sp_InsertStudentSubject', 'SN') IS NULL
    CREATE SYNONYM [dbo].[syn_sp_InsertStudentSubject] FOR [academic].[usp_insert_student_subject];
GO
IF OBJECT_ID('dbo.syn_sp_UpdateStudent', 'SN') IS NULL
    CREATE SYNONYM [dbo].[syn_sp_UpdateStudent] FOR [academic].[usp_update_student];
GO
