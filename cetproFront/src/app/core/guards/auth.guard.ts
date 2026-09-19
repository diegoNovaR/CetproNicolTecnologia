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

  const rolRequerido = route.data['rol'] as string | undefined;
  if (rolRequerido && authService.getRolDesdeToken() !== rolRequerido) {
    return router.createUrlTree(['/login']);
  }

  return true;
};
