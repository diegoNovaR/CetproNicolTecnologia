import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (!authService.tieneSesionValida()) {
    authService.logout();
    return router.createUrlTree(['/login']);
  }

  const rolActual = authService.getRolDesdeToken();
  const rolRequerido = route.data['rol'] as string | undefined;
  if (rolRequerido && rolActual !== rolRequerido) {
    // Sesión válida pero rol equivocado: se devuelve a su propia zona en vez de cerrar sesión.
    return router.createUrlTree([rolActual === 'admin' ? '/admin' : '/estudiante']);
  }

  return true;
};
