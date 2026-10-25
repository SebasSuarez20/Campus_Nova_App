USE [CampusNovaDB]
GO

/* 03. Vista y procedimientos almacenados. Se puede ejecutar varias veces (CREATE OR ALTER). */

/****** Objeto: View [security].[vw_get_user_for_id] Fecha de script: 25/10/2026 1:32:38 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER VIEW [security].[vw_get_user_for_id]
AS
SELECT
    u.id_pk         AS Id,
    u.username      AS Identification,
    u.password_hash AS Password,
    s.first_name    AS FirstName,
    s.last_name     AS LastName,
    r.name          AS Rol,
    i.code          AS Institution_Code,
    s.email         AS Email
FROM security.users u
JOIN academic.institution i ON i.id_pk = u.id_fk_institution
JOIN security.role r        ON r.id_pk = u.id_fk_role
LEFT JOIN academic.student s
       ON s.id_fk_institution = u.id_fk_institution
      AND s.identification    = u.username
WHERE u.active = 1;
GO
/****** Objeto: StoredProcedure [academic].[usp_delete_student] Fecha de script: 25/10/2026 1:32:38 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [academic].[usp_delete_student]
    @InstitutionId         INT,
    @StudentIdentification BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @StudentId INT;
    DECLARE @OwnTran   BIT = 0;
    DECLARE @Json      NVARCHAR(MAX);

    SELECT @StudentId = st.id_pk
    FROM academic.student st
    WHERE st.identification    = @StudentIdentification
      AND st.id_fk_institution = @InstitutionId
      AND st.active = 1;

    IF @StudentId IS NULL
    BEGIN
        RAISERROR('El estudiante no existe o está inactivo.', 16, 1);
        RETURN;
    END

    BEGIN TRY
        IF @@TRANCOUNT = 0
        BEGIN
            BEGIN TRANSACTION;
            SET @OwnTran = 1;
        END

        SET @Json =
        (
            SELECT
                'student'              AS [table],
                'DELETE'               AS [action],
                @StudentIdentification AS deletedBy,
                SYSUTCDATETIME()       AS deletedAt,
                JSON_QUERY((
                    SELECT st.id_pk, st.identification, st.first_name, st.last_name, st.email
                    FROM academic.student st
                    WHERE st.id_pk = @StudentId
                    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
                )) AS [row],
                JSON_QUERY((
                    SELECT ss.id_fk_subject, ss.enrolled_at
                    FROM academic.student_subject ss
                    WHERE ss.id_fk_student = @StudentId
                      AND ss.id_fk_institution = @InstitutionId
                    FOR JSON PATH
                )) AS subjects
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        );

        DELETE FROM academic.student_subject
        WHERE id_fk_student     = @StudentId
          AND id_fk_institution = @InstitutionId;

        UPDATE academic.student
        SET active = 0
        WHERE id_pk = @StudentId;

        UPDATE security.users
        SET active = 0
        WHERE username          = @StudentIdentification
          AND id_fk_institution = @InstitutionId;

        INSERT INTO security.audit_log (data_json) VALUES (@Json);

        IF @OwnTran = 1
            COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @OwnTran = 1 AND @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
/****** Objeto: StoredProcedure [academic].[usp_delete_student_subject] Fecha de script: 25/10/2026 1:32:38 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [academic].[usp_delete_student_subject]
    @InstitutionCode       INT,
    @StudentIdentification INT,
    @SubjectCode           INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @IdPkStudent INT;
    DECLARE @OwnTran BIT = 0;
    DECLARE @Deleted TABLE
    (
        id_fk_institution INT,
        id_fk_student     INT,
        id_fk_subject     INT,
        enrolled_at       DATETIME2(3)
    );

    SELECT @IdPkStudent = s.id_pk
    FROM academic.student s
    WHERE s.identification = @StudentIdentification
      AND s.id_fk_institution = @InstitutionCode
      AND s.active = 1;

    IF @IdPkStudent IS NULL
    BEGIN
        RAISERROR('El estudiante no existe o está inactivo.', 16, 1);
        RETURN;
    END

    BEGIN TRY
        IF @@TRANCOUNT = 0
        BEGIN
            BEGIN TRANSACTION;
            SET @OwnTran = 1;
        END

        DELETE FROM academic.student_subject
        OUTPUT deleted.id_fk_institution, deleted.id_fk_student, deleted.id_fk_subject, deleted.enrolled_at
        INTO @Deleted
        WHERE id_fk_institution = @InstitutionCode
          AND id_fk_student     = @IdPkStudent
          AND id_fk_subject     = @SubjectCode;

        IF @@ROWCOUNT = 0
            THROW 50002, 'El estudiante no está inscrito en esa materia.', 1;

        INSERT INTO security.audit_log (data_json)
        SELECT
        (
            SELECT
                'student_subject'      AS [table],
                'DELETE'               AS [action],
                @StudentIdentification AS deletedBy,
                SYSUTCDATETIME()       AS deletedAt,
                JSON_QUERY((
                    SELECT d.id_fk_institution, d.id_fk_student, d.id_fk_subject, d.enrolled_at
                    FROM @Deleted d
                    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
                )) AS [row]
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        );

        IF @OwnTran = 1
            COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @OwnTran = 1 AND @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
/****** Objeto: StoredProcedure [academic].[usp_get_student_classes] Fecha de script: 25/10/2026 1:32:38 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [academic].[usp_get_student_classes]
    @InstitutionId         INT,
    @StudentIdentification BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @StudentId INT;

    SELECT @StudentId = st.id_pk
    FROM academic.student st
    WHERE st.identification = @StudentIdentification
      AND st.id_fk_institution = @InstitutionId;

    SELECT
        s.id_pk AS materiaId,
        s.name,
        s.code
    FROM academic.student_subject sb
    JOIN academic.subject s  ON s.id_pk = sb.id_fk_subject
                            AND s.active = 1
                            AND s.id_fk_institution = sb.id_fk_institution
    JOIN academic.student st ON st.id_pk = sb.id_fk_student
                            AND st.active = 1
                            AND st.id_fk_institution = sb.id_fk_institution
    WHERE sb.id_fk_institution = @InstitutionId
      AND sb.id_fk_student     = @StudentId
      ORDER BY s.id_pk;

    SELECT
        CONCAT_WS(' ', st.first_name, st.last_name) AS fullname,
        CONCAT_WS(' ', p.first_name, p.last_name)   AS fullnameTeacher,
        sb.id_fk_subject
    FROM academic.student_subject sb
    JOIN academic.student st   ON st.id_pk = sb.id_fk_student
                              AND st.active = 1
                              AND st.id_fk_institution = sb.id_fk_institution
    JOIN academic.subject sj   ON sj.id_pk = sb.id_fk_subject
                              AND sj.active = 1
                              AND sj.id_fk_institution = sb.id_fk_institution
    JOIN academic.professor p  ON p.id_pk = sj.id_fk_professor
                              AND p.active = 1
                              AND p.id_fk_institution = sb.id_fk_institution
    WHERE sb.id_fk_institution = @InstitutionId;
END
GO
/****** Objeto: StoredProcedure [academic].[usp_get_students] Fecha de script: 25/10/2026 1:32:38 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [academic].[usp_get_students]
    @InstitutionId         INT,
    @StudentIdentification BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        st.id_pk AS idStudent,
        CONCAT_WS(' ', st.first_name, st.last_name) AS fullName,
        st.identification,
        st.email,
        ISNULL(STRING_AGG(CAST(s.name AS NVARCHAR(MAX)), ', ') WITHIN GROUP (ORDER BY s.name), '') AS subjectInscription,
        COUNT(s.id_pk) * 3 AS credits,
        CAST(CASE WHEN st.identification = @StudentIdentification THEN 1 ELSE 0 END AS BIT) AS isMine
    FROM academic.student st
    LEFT JOIN academic.student_subject sb ON sb.id_fk_student = st.id_pk
                                         AND sb.id_fk_institution = st.id_fk_institution
    LEFT JOIN academic.subject s ON s.id_pk = sb.id_fk_subject
                                AND s.id_fk_institution = sb.id_fk_institution
                                AND s.active = 1
    WHERE st.id_fk_institution = @InstitutionId
      AND st.active = 1
    GROUP BY st.id_pk, st.first_name, st.last_name, st.identification, st.email;
END
GO
/****** Objeto: StoredProcedure [academic].[usp_get_subjects] Fecha de script: 25/10/2026 1:32:38 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [academic].[usp_get_subjects]
    @InstitutionId         INT,
    @StudentIdentification BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @StudentId INT;

    SELECT @StudentId = st.id_pk
    FROM academic.student st
    WHERE st.identification = @StudentIdentification
      AND st.id_fk_institution = @InstitutionId;

    WITH cte_unlock_subject AS (
        SELECT s.id_pk,
            CASE
                WHEN EXISTS (
                    SELECT 1
                    FROM academic.student_subject ss
                    WHERE ss.id_fk_student = @StudentId
                      AND ss.id_fk_subject = s.id_pk
                ) THEN 'Completado'
                WHEN EXISTS (
                    SELECT 1
                    FROM academic.student_subject ss
                    JOIN academic.subject s2 ON s2.id_pk = ss.id_fk_subject
                    WHERE ss.id_fk_student = @StudentId
                      AND s2.id_fk_professor = s.id_fk_professor
                ) THEN 'Bloqueado'
                ELSE 'Inscribir'
            END AS result
        FROM academic.subject s
        WHERE s.id_fk_institution = @InstitutionId
    )
    SELECT
        s.code,
        s.name,
        s.description,
        CONCAT_WS(' ', p.first_name, p.last_name) AS fullName,
        s.credits,
        sc.totalStudents,
        p.id_pk AS professorId,
        s.id_pk AS subjectId,
        cus.result
    FROM academic.subject s
    JOIN academic.professor p ON p.id_pk = s.id_fk_professor
    OUTER APPLY (
        SELECT COUNT(*) AS totalStudents
        FROM academic.student_subject sp
        WHERE sp.id_fk_subject = s.id_pk
          AND sp.id_fk_institution = s.id_fk_institution
    ) sc
    JOIN cte_unlock_subject cus ON cus.id_pk = s.id_pk
    WHERE s.id_fk_institution = @InstitutionId;

    SELECT
        p.id_pk AS professorId,
        CONCAT_WS(' ', p.first_name, p.last_name) AS full_name
    FROM academic.professor p
    WHERE p.id_fk_institution = @InstitutionId AND p.active = 1;
END
GO
/****** Objeto: StoredProcedure [academic].[usp_insert_student] Fecha de script: 25/10/2026 1:32:38 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [academic].[usp_insert_student]
    @InstitutionCode BIGINT,
    @FirstName       NVARCHAR(100),
    @LastName        NVARCHAR(100),
    @Email           NVARCHAR(256),
    @Identification  BIGINT,
    @PasswordHash    CHAR(64)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @InstitutionId INT;
    DECLARE @OwnTran BIT = 0;

    SELECT @InstitutionId = i.id_pk
    FROM academic.institution i
    WHERE i.code = @InstitutionCode
      AND i.active = 1;

    IF @InstitutionId IS NULL
    BEGIN
        RAISERROR('El código institucional no existe.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM academic.student
               WHERE id_fk_institution = @InstitutionId
                 AND identification    = @Identification)
       OR EXISTS (SELECT 1 FROM security.users
                  WHERE id_fk_institution = @InstitutionId
                    AND username          = @Identification)
    BEGIN
        RAISERROR('Ya existe un estudiante registrado con ese documento.', 16, 1);
        RETURN;
    END

    BEGIN TRY
        IF @@TRANCOUNT = 0
        BEGIN
            BEGIN TRANSACTION;
            SET @OwnTran = 1;
        END

        INSERT INTO academic.student (id_fk_institution, identification, first_name, last_name, email, created_at)
        VALUES (@InstitutionId, @Identification, @FirstName, @LastName, @Email, SYSDATETIME());

        INSERT INTO security.users (id_fk_institution, username, password_hash)
        VALUES (@InstitutionId, @Identification, @PasswordHash);

        IF @OwnTran = 1
            COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @OwnTran = 1 AND @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
/****** Objeto: StoredProcedure [academic].[usp_insert_student_subject] Fecha de script: 25/10/2026 1:32:38 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [academic].[usp_insert_student_subject]  
    @InstitutionCode INT,  
    @StudentIdentification     INT,  
    @SubjectCode     INT  
AS  
BEGIN  
    SET NOCOUNT ON;  
  
     DECLARE @IdPkUser BIGINT;  
  
    SELECT @IdPkUser = u.id_pk FROM academic.student u WHERE u.identification = @StudentIdentification AND u.active = 1 AND u.id_fk_institution = @InstitutionCode;  
  
    IF @IdPkUser IS NULL   
    BEGIN   
     RAISERROR('El estudiante no existe o está inactivo.', 16, 1);  
    RETURN;  
    END  
  
    select @IdPkUser;  
  
    INSERT INTO academic.student_subject (id_fk_institution, id_fk_student, id_fk_subject)  
    VALUES (@InstitutionCode, @IdPkUser, @SubjectCode);  
END  
GO
/****** Objeto: StoredProcedure [academic].[usp_update_student] Fecha de script: 25/10/2026 1:32:38 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [academic].[usp_update_student]
    @InstitutionId         INT,
    @StudentIdentification BIGINT,
    @FirstName             NVARCHAR(100),
    @LastName              NVARCHAR(100),
    @Email                 NVARCHAR(256),
    @PasswordHash          CHAR(64) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @OwnTran BIT = 0;

    BEGIN TRY
        IF @@TRANCOUNT = 0
        BEGIN
            BEGIN TRANSACTION;
            SET @OwnTran = 1;
        END

        UPDATE academic.student
        SET first_name = @FirstName,
            last_name  = @LastName,
            email      = @Email
        WHERE identification    = @StudentIdentification
          AND id_fk_institution = @InstitutionId
          AND active = 1;

        IF @@ROWCOUNT = 0
            THROW 50010, 'El estudiante no existe o está inactivo.', 1;

        IF @PasswordHash IS NOT NULL
        BEGIN
            UPDATE security.users
            SET password_hash = @PasswordHash
            WHERE username          = @StudentIdentification
              AND id_fk_institution = @InstitutionId;
        END

        IF @OwnTran = 1
            COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @OwnTran = 1 AND @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
/****** Objeto: StoredProcedure [security].[usp_get_access_to_system] Fecha de script: 25/10/2026 1:32:38 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [security].[usp_get_access_to_system]
    @Username        BIGINT,
    @InstitutionCode BIGINT,
    @Enabled         BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
        u.id_pk          AS Id,
        s.first_name     AS FirstName,
        s.last_name      AS LastName,
        u.username       AS Identification,
        u.password_hash AS Password,
        r.name           AS Rol,
        i.id_pk           AS Institution_Code,
        s.email          AS Email
    FROM security.users u
    JOIN academic.institution i ON i.id_pk = u.id_fk_institution
    JOIN security.role r        ON r.id_pk = u.id_fk_role
    JOIN academic.student s
           ON s.id_fk_institution = u.id_fk_institution
          AND s.identification    = u.username
    WHERE u.username = @Username
      AND i.code     = @InstitutionCode
      AND u.active   = @Enabled;
END
GO

USE [CampusNovaDB]
GO
