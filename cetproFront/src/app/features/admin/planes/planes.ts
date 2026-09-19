import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { PlanesService } from '../../../core/services/planes.service';
import { CursosService } from '../../../core/services/cursos.service';
import { Curso, PlanEstudio } from '../../../core/models/academico.models';
import { PlanDialog, PlanDialogData } from './plan-dialog/plan-dialog';

@Component({
  selector: 'app-planes',
  standalone: true,
  imports: [
    FormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './planes.html'
})
export class Planes implements OnInit {
  private readonly planesService = inject(PlanesService);
  private readonly cursosService = inject(CursosService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);

  readonly columnas = ['curso', 'tipo', 'duracion', 'precioMatricula', 'pensionMensual', 'activo', 'acciones'];
  readonly cargando = signal(true);
  readonly planes = signal<PlanEstudio[]>([]);
  readonly cursos = signal<Curso[]>([]);
  readonly cursoFiltro = signal<string>('');

  ngOnInit(): void {
    this.cursosService.getAll().subscribe((cursos) => this.cursos.set(cursos));
    this.cargar();
  }

  cargar(): void {
    this.cargando.set(true);
    this.planesService.getAll(this.cursoFiltro() || undefined).subscribe({
      next: (data) => {
        this.planes.set(data);
        this.cargando.set(false);
      },
      error: () => this.cargando.set(false)
    });
  }

  onFiltroChange(cursoId: string): void {
    this.cursoFiltro.set(cursoId);
    this.cargar();
  }

  nombreCurso(cursoId: string): string {
    return this.cursos().find((c) => c.id === cursoId)?.nombre ?? '—';
  }

  crear(): void {
    const ref = this.dialog.open<PlanDialog, PlanDialogData>(PlanDialog, {
      data: { plan: null, cursos: this.cursos(), cursoIdPreseleccionado: this.cursoFiltro() }
    });
    ref.afterClosed().subscribe((resultado) => {
      if (!resultado) return;
      this.planesService.create(resultado).subscribe({
        next: () => {
          this.snackBar.open('Plan de estudio creado', 'Cerrar', { duration: 2000 });
          this.cargar();
        },
        error: (err) => this.snackBar.open(err?.error?.message ?? 'Error al crear el plan', 'Cerrar', { duration: 3000 })
      });
    });
  }

  editar(plan: PlanEstudio): void {
    const ref = this.dialog.open<PlanDialog, PlanDialogData>(PlanDialog, {
      data: { plan, cursos: this.cursos() }
    });
    ref.afterClosed().subscribe((resultado) => {
      if (!resultado) return;
      this.planesService.update(plan.id, resultado).subscribe({
        next: () => {
          this.snackBar.open('Plan de estudio actualizado', 'Cerrar', { duration: 2000 });
          this.cargar();
        },
        error: (err) => this.snackBar.open(err?.error?.message ?? 'Error al actualizar el plan', 'Cerrar', { duration: 3000 })
      });
    });
  }

  eliminar(plan: PlanEstudio): void {
    if (!confirm('¿Eliminar este plan de estudio?')) return;
    this.planesService.delete(plan.id).subscribe({
      next: () => {
        this.snackBar.open('Plan de estudio eliminado', 'Cerrar', { duration: 2000 });
        this.cargar();
      },
      error: (err) => this.snackBar.open(err?.error?.message ?? 'Error al eliminar el plan', 'Cerrar', { duration: 3000 })
    });
  }
}
