USE [CampusNovaDB]
GO

/* 06. Datos iniciales: institucion, profesores, materias y rol Student.
   Ejecutar solo sobre tablas vacias (conserva los id_pk con IDENTITY_INSERT). */

SET IDENTITY_INSERT [academic].[institution] ON 

INSERT [academic].[institution] ([id_pk], [code], [name], [address], [city], [email], [phone], [active], [created_at]) VALUES (1, 900123456, N'Instituto Campus Nova', N'Calle 10 # 20-30', N'Medellín', N'contacto@campusnova.edu', N'3001234567', 1, CAST(N'2026-10-24T20:27:53.3220000' AS DateTime2))
SET IDENTITY_INSERT [academic].[institution] OFF
GO
SET IDENTITY_INSERT [academic].[professor] ON 

INSERT [academic].[professor] ([id_pk], [id_fk_institution], [identification], [first_name], [last_name], [email], [phone], [active], [created_at]) VALUES (1, 1, 1010000001, N'Marcela', N'Ríos', N'marcela.rios@campusnova.edu', N'3101000001', 1, CAST(N'2026-10-24T15:30:41.5680000' AS DateTime2))
INSERT [academic].[professor] ([id_pk], [id_fk_institution], [identification], [first_name], [last_name], [email], [phone], [active], [created_at]) VALUES (2, 1, 1010000002, N'Julián', N'Castaño', N'julian.castano@campusnova.edu', N'3101000002', 1, CAST(N'2026-10-24T15:30:41.5680000' AS DateTime2))
INSERT [academic].[professor] ([id_pk], [id_fk_institution], [identification], [first_name], [last_name], [email], [phone], [active], [created_at]) VALUES (3, 1, 1010000003, N'Paola', N'Quintero', N'paola.quintero@campusnova.edu', N'3101000003', 1, CAST(N'2026-10-24T15:30:41.5680000' AS DateTime2))
INSERT [academic].[professor] ([id_pk], [id_fk_institution], [identification], [first_name], [last_name], [email], [phone], [active], [created_at]) VALUES (4, 1, 1010000004, N'Ricardo', N'Salazar', N'ricardo.salazar@campusnova.edu', N'3101000004', 1, CAST(N'2026-10-24T15:30:41.5680000' AS DateTime2))
INSERT [academic].[professor] ([id_pk], [id_fk_institution], [identification], [first_name], [last_name], [email], [phone], [active], [created_at]) VALUES (5, 1, 1010000005, N'Natalia', N'Herrera', N'natalia.herrera@campusnova.edu', N'3101000005', 1, CAST(N'2026-10-24T15:30:41.5680000' AS DateTime2))
SET IDENTITY_INSERT [academic].[professor] OFF
GO
SET IDENTITY_INSERT [academic].[subject] ON 

INSERT [academic].[subject] ([id_pk], [id_fk_institution], [id_fk_professor], [code], [name], [description], [credits], [active], [created_at]) VALUES (1, 1, 1, N'MAT101', N'Cálculo I', N'Límites, derivadas e integrales de una variable.', 3, 1, CAST(N'2026-10-24T15:35:21.9250000' AS DateTime2))
INSERT [academic].[subject] ([id_pk], [id_fk_institution], [id_fk_professor], [code], [name], [description], [credits], [active], [created_at]) VALUES (2, 1, 1, N'MAT102', N'Álgebra Lineal', N'Matrices, vectores y espacios vectoriales.', 3, 1, CAST(N'2026-10-24T15:35:21.9250000' AS DateTime2))
INSERT [academic].[subject] ([id_pk], [id_fk_institution], [id_fk_professor], [code], [name], [description], [credits], [active], [created_at]) VALUES (3, 1, 2, N'SIS101', N'Programación I', N'Lógica, algoritmos y primeros programas.', 3, 1, CAST(N'2026-10-24T15:35:21.9250000' AS DateTime2))
INSERT [academic].[subject] ([id_pk], [id_fk_institution], [id_fk_professor], [code], [name], [description], [credits], [active], [created_at]) VALUES (4, 1, 2, N'SIS102', N'Estructuras de Datos', N'Listas, pilas, colas, árboles y grafos.', 3, 1, CAST(N'2026-10-24T15:35:21.9250000' AS DateTime2))
INSERT [academic].[subject] ([id_pk], [id_fk_institution], [id_fk_professor], [code], [name], [description], [credits], [active], [created_at]) VALUES (5, 1, 3, N'FIS101', N'Física Mecánica', N'Cinemática, dinámica y conservación de energía.', 3, 1, CAST(N'2026-10-24T15:35:21.9250000' AS DateTime2))
INSERT [academic].[subject] ([id_pk], [id_fk_institution], [id_fk_professor], [code], [name], [description], [credits], [active], [created_at]) VALUES (6, 1, 3, N'FIS102', N'Electromagnetismo', N'Campos eléctricos, magnéticos y circuitos.', 3, 1, CAST(N'2026-10-24T15:35:21.9250000' AS DateTime2))
INSERT [academic].[subject] ([id_pk], [id_fk_institution], [id_fk_professor], [code], [name], [description], [credits], [active], [created_at]) VALUES (7, 1, 4, N'SIS201', N'Bases de Datos', N'Modelo relacional, SQL y diseño de esquemas.', 3, 1, CAST(N'2026-10-24T15:35:21.9250000' AS DateTime2))
INSERT [academic].[subject] ([id_pk], [id_fk_institution], [id_fk_professor], [code], [name], [description], [credits], [active], [created_at]) VALUES (8, 1, 4, N'SIS202', N'Redes de Computadores', N'Protocolos, direccionamiento y arquitectura de red.', 3, 1, CAST(N'2026-10-24T15:35:21.9250000' AS DateTime2))
INSERT [academic].[subject] ([id_pk], [id_fk_institution], [id_fk_professor], [code], [name], [description], [credits], [active], [created_at]) VALUES (9, 1, 5, N'EST101', N'Estadística', N'Probabilidad, inferencia y análisis de datos.', 3, 1, CAST(N'2026-10-24T15:35:21.9250000' AS DateTime2))
INSERT [academic].[subject] ([id_pk], [id_fk_institution], [id_fk_professor], [code], [name], [description], [credits], [active], [created_at]) VALUES (10, 1, 5, N'COM101', N'Comunicación Profesional', N'Expresión oral y escrita en entornos laborales.', 3, 1, CAST(N'2026-10-24T15:35:21.9250000' AS DateTime2))
SET IDENTITY_INSERT [academic].[subject] OFF
GO
INSERT [security].[role] ([id_pk], [name]) VALUES (1, N'Student')
GO
