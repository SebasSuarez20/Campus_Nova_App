# Campus Nova

Aplicación web para el registro de estudiantes en un programa de créditos.
Derivada de EnterprisePOS: se conservó la autenticación, el manejo de sesión y
el panel de administrador.

```
Campus-Nova/
├─ backend/    API .NET 8 — Clean Architecture + MediatR + Dapper
└─ frontend/   Angular 17.3 standalone
```

## Reglas del negocio (prueba técnica)

1. CRUD para que el estudiante se registre en línea.
2. El estudiante se adhiere a un programa de créditos.
3. Hay 10 materias de 3 créditos cada una.
4. Solo puede elegir 3 materias.
5. Hay 5 profesores, cada uno dicta 2 materias.
6. No puede tener clases con el mismo profesor.
7. Puede ver los registros de otros estudiantes.
8. De sus compañeros de clase solo ve el nombre.

## Backend

```bash
cd backend
dotnet build CampusNova.sln
dotnet run --project src/CampusNova.Api
```

Arranca en `https://localhost:7013`, con Swagger en `/swagger`. Antes hay que
montar la base de datos (siguiente seccion) y definir `Jwt:Key`:

```bash
cd backend/src/CampusNova.Api
dotnet user-secrets set "Jwt:Key" "<clave de al menos 32 caracteres>"
```

## Base de datos

SQL Server (`CampusNovaDB`, esquemas `academic` y `security`). Los scripts estan en
`backend/db/sqlserver` y se ejecutan en orden `01` a `06` (ver el README de esa carpeta):
crean las tablas, procedimientos, sinonimos `syn_*`, el trigger de maximo 3 materias y
los datos iniciales (institucion `900123456`, 5 profesores, 10 materias).

La cadena de conexion esta en `ConnectionStrings:CampusNova` y `Jwt:Key` se configura con
`dotnet user-secrets` (no se versiona).

## Frontend

```bash
cd frontend
npm install
npm start
```

Arranca en `http://localhost:4200`.

### Pantallas

- `/login` y `/registro` (públicas).
- `/pages/subjects` (Materias), `/pages/my-classes` (Mis clases) y `/pages/students` (Estudiantes), protegidas por sesión y por rol.

Todas las rutas cargan con lazy loading.

### Reporte en PDF

En `/pages/students` el botón **Descargar PDF** genera el reporte de estudiantes del
programa de créditos (resumen, materias inscritas y créditos por estudiante). Respeta
el filtro activo en la tabla y se arma en el navegador con `jspdf` y `jspdf-autotable`,
sin pasar por el backend. El archivo se descarga como `estudiantes-AAAA-MM-DD.pdf`.

### Identidad visual

Violeta `#5B3DF5` y coral `#FF7A59` sobre azul noche `#14123A`. Fuentes Sora y
Plus Jakarta Sans (Google Fonts). Los tokens viven en `src/styles.css`.
