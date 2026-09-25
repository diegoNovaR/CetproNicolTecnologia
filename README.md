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

## Consumo de la API RENIEC desde el frontend

La verificación de DNI en `SolicitarComponent` llama directo a la API de RENIEC **desde Angular**, sin pasar por el backend .NET.

- **Dónde:** `ReniecService.buscarPorDni()` (`cetproFront/src/app/core/services/reniec.service.ts`) hace `GET https://api.apis.net.pe/v2/reniec/dni?numero={dni}` con `Authorization: Bearer {environment.reniecToken}`.
- **Token:** se configura en `environment.ts` / `environment.prod.ts` (clave `reniecToken`); reemplazar el placeholder `TU_TOKEN_AQUI` por un token real de apis.net.pe.
- **Disparo:** en `SolicitarComponent` (`solicitar.ts`), el campo `dni` del formulario se escucha con `debounceTime(500)` + `distinctUntilChanged()` + `filter` (solo dispara con 8 dígitos exactos), y llama a `verificarDni()`.
- **Interceptor:** `authInterceptor` solo agrega el JWT propio a peticiones hacia `environment.apiUrl`; la llamada a RENIEC no lleva ese header, para no pisar el `Authorization` que necesita RENIEC.

**Posible falla por CORS:** la API de RENIEC puede rechazar la petición hecha directo desde el navegador con un error de CORS (`No 'Access-Control-Allow-Origin' header...`), independientemente de si el token es válido — es una restricción del lado del servidor de RENIEC, no del código de este proyecto. Cuando eso ocurre, `catchError` lo captura y la UI cae al estado `no-encontrado`, dejando los campos Nombre/Apellido habilitados para completarlos manualmente. Si se necesita evitar ese error de CORS, la única solución real sería proxear la llamada a través del backend, lo cual queda fuera del alcance actual (se pidió explícitamente consumir RENIEC solo desde el frontend).

