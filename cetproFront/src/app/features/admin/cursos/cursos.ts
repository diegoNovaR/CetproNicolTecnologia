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
import { CursosService } from '../../../core/services/cursos.service';
import { AreasService } from '../../../core/services/areas.service';
import { Area, Curso } from '../../../core/models/academico.models';
import { CursoDialog, CursoDialogData } from './curso-dialog/curso-dialog';

@Component({
  selector: 'app-cursos',
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
  templateUrl: './cursos.html'
})
export class Cursos implements OnInit {
  private readonly cursosService = inject(CursosService);
  private readonly areasService = inject(AreasService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);

  readonly columnas = ['nombre', 'area', 'activo', 'acciones'];
  readonly cargando = signal(true);
  readonly cursos = signal<Curso[]>([]);
  readonly areas = signal<Area[]>([]);
  readonly areaFiltro = signal<string>('');

  ngOnInit(): void {
    this.areasService.getAll().subscribe((areas) => this.areas.set(areas));
    this.cargar();
  }

  cargar(): void {
    this.cargando.set(true);
    this.cursosService.getAll(this.areaFiltro() || undefined).subscribe({
      next: (data) => {
        this.cursos.set(data);
        this.cargando.set(false);
      },
      error: () => this.cargando.set(false)
    });
  }

  onFiltroChange(areaId: string): void {
    this.areaFiltro.set(areaId);
    this.cargar();
  }

  nombreArea(areaId: string): string {
    return this.areas().find((a) => a.id === areaId)?.nombre ?? '—';
  }

  crear(): void {
    const ref = this.dialog.open<CursoDialog, CursoDialogData>(CursoDialog, {
      data: { curso: null, areas: this.areas(), areaIdPreseleccionada: this.areaFiltro() }
    });
    ref.afterClosed().subscribe((resultado) => {
      if (!resultado) return;
      this.cursosService.create(resultado).subscribe({
        next: () => {
          this.snackBar.open('Curso creado', 'Cerrar', { duration: 2000 });
          this.cargar();
        },
        error: (err) => this.snackBar.open(err?.error?.message ?? 'Error al crear el curso', 'Cerrar', { duration: 3000 })
      });
    });
  }

  editar(curso: Curso): void {
    const ref = this.dialog.open<CursoDialog, CursoDialogData>(CursoDialog, {
      data: { curso, areas: this.areas() }
    });
    ref.afterClosed().subscribe((resultado) => {
      if (!resultado) return;
      this.cursosService.update(curso.id, resultado).subscribe({
        next: () => {
          this.snackBar.open('Curso actualizado', 'Cerrar', { duration: 2000 });
          this.cargar();
        },
        error: (err) => this.snackBar.open(err?.error?.message ?? 'Error al actualizar el curso', 'Cerrar', { duration: 3000 })
      });
    });
  }

  eliminar(curso: Curso): void {
    if (!confirm(`¿Eliminar el curso "${curso.nombre}"?`)) return;
    this.cursosService.delete(curso.id).subscribe({
      next: () => {
        this.snackBar.open('Curso eliminado', 'Cerrar', { duration: 2000 });
        this.cargar();
      },
      error: (err) => this.snackBar.open(err?.error?.message ?? 'Error al eliminar el curso', 'Cerrar', { duration: 3000 })
    });
  }
}
