USE [CampusNovaDB]
GO

/* 05. Trigger: un estudiante no puede inscribir mas de 3 materias. Se puede ejecutar varias veces. */
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER TRIGGER [academic].[trg_student_subject_max_3]
ON [academic].[student_subject]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @InstitutionId INT;
    DECLARE @StudentId     INT;
    DECLARE @Total         INT;

    SELECT TOP (1)
        @InstitutionId = i.id_fk_institution,
        @StudentId     = i.id_fk_student
    FROM inserted i;

    SELECT @Total = COUNT(*)
    FROM academic.student_subject ss
    WHERE ss.id_fk_institution = @InstitutionId
      AND ss.id_fk_student     = @StudentId;

    IF @Total > 3
        THROW 50003, 'El estudiante no puede inscribir más de 3 materias.', 1;
END
GO

ALTER TABLE [academic].[student_subject] ENABLE TRIGGER [trg_student_subject_max_3]
GO