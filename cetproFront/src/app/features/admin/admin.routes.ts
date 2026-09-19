import { Routes } from '@angular/router';
import { AdminLayout } from './layout/admin-layout';

export const ADMIN_ROUTES: Routes = [
  {
    path: '',
    component: AdminLayout,
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', loadComponent: () => import('./dashboard/dashboard').then((m) => m.AdminDashboard) },
      { path: 'areas', loadComponent: () => import('./areas/areas').then((m) => m.Areas) },
      { path: 'cursos', loadComponent: () => import('./cursos/cursos').then((m) => m.Cursos) },
      { path: 'planes', loadComponent: () => import('./planes/planes').then((m) => m.Planes) },
      { path: 'matriculas', loadComponent: () => import('./matriculas/matriculas').then((m) => m.Matriculas) },
      { path: 'pagos', loadComponent: () => import('./pagos/pagos').then((m) => m.Pagos) }
    ]
  }
];
