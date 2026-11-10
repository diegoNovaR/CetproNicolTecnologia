import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { catchError, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  // Solo adjunta el JWT propio a peticiones contra nuestra API. APIs externas
  // (p. ej. RENIEC) traen su propio header Authorization que no debe pisarse.
  if (!req.url.startsWith(environment.apiUrl)) return next(req);

  const authService = inject(AuthService);
  const router = inject(Router);
  const snackBar = inject(MatSnackBar);
  const translate = inject(TranslateService);

  const token = authService.getToken();
  const esEndpointDeAuth = req.url.startsWith(`${environment.apiUrl}/auth/`);

  // get() espera a que el JSON de idioma esté cargado (instant() devolvería la clave si el
  // error llega justo al recargar la página).
  const avisar = (clave: string) =>
    translate.get([clave, 'AUTH.CLOSE']).subscribe((t) => snackBar.open(t[clave], t['AUTH.CLOSE'], { duration: 5000 }));

  const peticion = token ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } }) : req;

  return next(peticion).pipe(
    catchError((error: HttpErrorResponse) => {
      // 401 en login/registro = credenciales inválidas: lo maneja el propio formulario.
      if (error.status === 401 && !esEndpointDeAuth) {
        // Token vencido o rechazado por la API: se cierra la sesión y se vuelve al login.
        authService.logout();
        avisar('AUTH.SESSION_EXPIRED');
        void router.navigateByUrl('/login');
      } else if (error.status === 403) {
        // Sesión válida pero sin permiso para ese recurso (p. ej. estudiante en ruta de admin).
        avisar('AUTH.FORBIDDEN');
      }
      return throwError(() => error);
    })
  );
};
