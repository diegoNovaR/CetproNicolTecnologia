import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  // Solo adjunta el JWT propio a peticiones contra nuestra API. APIs externas
  // (p. ej. RENIEC) traen su propio header Authorization que no debe pisarse.
  if (!req.url.startsWith(environment.apiUrl)) return next(req);

  const authService = inject(AuthService);
  const token = authService.getToken();

  if (!token) return next(req);

  return next(
    req.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    })
  );
};
