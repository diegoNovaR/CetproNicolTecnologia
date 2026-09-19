import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { Header } from '../../../shared/header/header';

@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, RouterOutlet, MatSidenavModule, MatListModule, MatIconModule, Header],
  templateUrl: './admin-layout.html',
  styleUrl: './admin-layout.css'
})
export class AdminLayout {
  readonly opciones = [
    { ruta: '/admin/dashboard', icono: 'dashboard', label: 'Dashboard' },
    { ruta: '/admin/areas', icono: 'category', label: 'Áreas' },
    { ruta: '/admin/cursos', icono: 'menu_book', label: 'Cursos' },
    { ruta: '/admin/planes', icono: 'assignment', label: 'Planes' },
    { ruta: '/admin/matriculas', icono: 'how_to_reg', label: 'Matrículas' },
    { ruta: '/admin/pagos', icono: 'payments', label: 'Pagos' }
  ];
}
