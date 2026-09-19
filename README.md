# CetproNicolTecnologia
## Código Supabase: iMryYfJelP5TmyOc

## Cómo correr el proyecto

**Requisitos:** .NET 8 SDK, Node.js 18+ y Angular CLI (`npx` es suficiente, no hace falta instalarlo global).

**1. Backend (API .NET)**
```bash
cd cetproBack/src/CetproNicol.API
dotnet run --launch-profile https
```
Queda en `https://localhost:7260` (Swagger en `https://localhost:7260/swagger`). La primera vez que el navegador llame a esa URL puede advertir por el certificado de desarrollo; entra una vez a Swagger y acepta la excepción antes de usar el frontend.

**2. Frontend (Angular)**
```bash
cd cetproFront
npm install   # solo la primera vez
npx ng serve --port 4200
```
Queda en `http://localhost:4200`.

**3. Probarlo**
Usuario admin semilla: `admin@cetpronicol.com` / `Admin123!`. También puedes registrar un estudiante nuevo desde `/registro`. El connection string de Supabase usa el puerto 5432 (session pooler); si necesitas correr una migración nueva, usa ese puerto y no el 6543 (transaction pooler, no soporta bien las migraciones de EF Core).

## Dónde están las funciones stateless / stateful / i18n

**Stateful (modifican datos, con transacción):**
- `MatriculaService.MatricularEstudianteAsync` (`cetproBack/src/CetproNicol.Application/Services/MatriculaService.cs`) — `POST /api/matriculas`, UI en `SolicitarComponent`.
- `MatriculaService.AprobarPagoAsync` (mismo archivo) — `PUT /api/pagos/{id}/aprobar`, UI en `PagosComponent` (admin).

**Stateless (solo consulta/cálculo):**
- `MatriculaService.CalcularDeudaEstudianteAsync` — `GET /api/matriculas/{id}/deuda`, UI en el diálogo "Ver deuda" de `MisCursosComponent`.
- `ContenidoService.VerificarAccesoContenidoAsync` (`ContenidoService.cs`) — `GET /api/contenidos/verificar-acceso`, UI en el chip "Verificar acceso" de `MisCursosComponent`.

**i18n (ngx-translate):**
- Archivos de traducción: `cetproFront/src/assets/i18n/{es,en}.json`.
- Configuración: `cetproFront/src/app/app.config.ts` (`provideTranslateService`).
- Selector de idioma: `shared/header/header.ts`.
- Textos traducidos: `Header`, `Login`, `Registro` y `SolicitarComponent` (el resto de pantallas admin quedó en español fijo, fuera del alcance pedido).

