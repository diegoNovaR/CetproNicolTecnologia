import { Routes } from '@angular/router';
import { EstudianteLayout } from './layout/estudiante-layout';

export const ESTUDIANTE_ROUTES: Routes = [
  {
    path: '',
    component: EstudianteLayout,
    children: [
      { path: '', redirectTo: 'mis-cursos', pathMatch: 'full' },
      { path: 'mis-cursos', loadComponent: () => import('./mis-cursos/mis-cursos').then((m) => m.MisCursos) },
      { path: 'solicitar', loadComponent: () => import('./solicitar/solicitar').then((m) => m.Solicitar) }
    ]
  }
];
