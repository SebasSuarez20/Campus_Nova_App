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

## Docker

`docker-compose.yml` levanta todo el sistema con un solo comando:

| Servicio | Imagen | Función |
|---|---|---|
| `db` | SQL Server 2022 (Express) | Base de datos, con volumen `sqldata` |
| `db-init` | SQL Server 2022 | Un solo uso: ejecuta `backend/db/sqlserver/01..06` si la base está vacía |
| `backend` | .NET 8 (`backend/Dockerfile`) | API en el puerto interno 8080 |
| `frontend` | nginx (`frontend/Dockerfile`) | Sirve Angular y reenvía `/v1/api` y `/hubs` (SignalR) al backend |

```bash
cp .env.example .env        # edita MSSQL_SA_PASSWORD y JWT_KEY
docker compose up -d --build
```

La aplicación queda en `http://localhost:8080`. Como nginx sirve el frontend y la API
bajo el mismo origen, no hace falta tocar CORS ni las URLs del frontend de producción.

```bash
docker compose logs -f backend   # ver logs
docker compose down              # detener (conserva los datos)
docker compose down -v           # detener y borrar la base (se recarga desde los scripts)
```

El archivo `.env` contiene secretos y no se sube al repositorio; solo `.env.example`.

### Desplegar en la nube

Las imágenes son estándar, así que el mismo `docker-compose.yml` se puede correr en
cualquier servidor o servicio que soporte contenedores:

- **Una VM** (Azure, AWS EC2, DigitalOcean, etc.): instalar Docker, clonar el
  repositorio, crear el `.env` y ejecutar `docker compose up -d --build`.
- **Servicios de contenedores** (Azure Container Apps, AWS ECS, Google Cloud Run): subir
  las imágenes `backend` y `frontend` a un registro (`docker compose build` y
  `docker push`) y apuntar `ConnectionStrings__CampusNova` a una base administrada
  (por ejemplo Azure SQL), en lugar del contenedor `db`.

Antes de publicarlo:

1. **HTTPS es obligatorio.** Las cookies de sesión se emiten con `Secure` y
   `SameSite=None`, por lo que solo funcionan sobre HTTPS (en `localhost` el navegador
   lo permite). Pon un proxy con certificado delante del contenedor `frontend`
   (Caddy, Traefik, nginx con Let's Encrypt o el balanceador de la nube).
2. Usa contraseñas largas y únicas en `MSSQL_SA_PASSWORD` y `JWT_KEY`, y guárdalas como
   secretos de la plataforma, no en el repositorio.
3. Define `CORS_ORIGINS` con tu dominio público (`https://tu-dominio.com`) si el
   frontend se sirve desde otro origen.
4. No expongas el puerto 1433 de SQL Server a internet; en este compose no se publica.

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
