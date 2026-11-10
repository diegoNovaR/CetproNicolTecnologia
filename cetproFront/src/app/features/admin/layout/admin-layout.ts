import { Component, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatBadgeModule } from '@angular/material/badge';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Header } from '../../../shared/header/header';
import { NotificacionesService } from '../../../core/services/notificaciones.service';

const RUTA_MATRICULAS = '/admin/matriculas';

@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [
    RouterLink,
    RouterLinkActive,
    RouterOutlet,
    MatSidenavModule,
    MatListModule,
    MatIconModule,
    MatBadgeModule,
    Header
  ],
  templateUrl: './admin-layout.html',
  styleUrl: './admin-layout.css'
})
export class AdminLayout {
  private readonly notificaciones = inject(NotificacionesService);
  private readonly snackBar = inject(MatSnackBar);
  private readonly router = inject(Router);

  readonly matriculasNuevas = signal(0);

  readonly opciones = [
    { ruta: '/admin/dashboard', icono: 'dashboard', label: 'Dashboard' },
    { ruta: '/admin/areas', icono: 'category', label: 'Áreas' },
    { ruta: '/admin/cursos', icono: 'menu_book', label: 'Cursos' },
    { ruta: '/admin/planes', icono: 'assignment', label: 'Planes' },
    { ruta: RUTA_MATRICULAS, icono: 'how_to_reg', label: 'Matrículas' },
    { ruta: '/admin/pagos', icono: 'payments', label: 'Pagos' },
    { ruta: '/paises', icono: 'public', label: 'Países' }
  ];

  constructor() {
    this.notificaciones.nuevaMatricula$.pipe(takeUntilDestroyed()).subscribe((matricula) => {
      this.snackBar.open(
        `Nueva solicitud de matrícula de ${matricula.usuarioNombreCompleto} — ${matricula.cursoNombre}`,
        'Cerrar',
        { duration: 6000 }
      );

      if (!this.router.url.startsWith(RUTA_MATRICULAS)) {
        this.matriculasNuevas.update((n) => n + 1);
      }
    });
  }

  badgeDe(ruta: string): number {
    return ruta === RUTA_MATRICULAS ? this.matriculasNuevas() : 0;
  }

  alNavegar(ruta: string): void {
    if (ruta === RUTA_MATRICULAS) this.matriculasNuevas.set(0);
  }
}
