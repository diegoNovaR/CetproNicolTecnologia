import { Component, OnInit, inject, signal } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AreasService } from '../../../core/services/areas.service';
import { Area } from '../../../core/models/academico.models';
import { AreaDialog, AreaDialogData } from './area-dialog/area-dialog';

@Component({
  selector: 'app-areas',
  standalone: true,
  imports: [MatTableModule, MatButtonModule, MatIconModule, MatChipsModule, MatProgressSpinnerModule],
  templateUrl: './areas.html',
  styleUrl: './areas.css'
})
export class Areas implements OnInit {
  private readonly areasService = inject(AreasService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);

  readonly columnas = ['nombre', 'descripcion', 'activo', 'acciones'];
  readonly cargando = signal(true);
  readonly areas = signal<Area[]>([]);

  ngOnInit(): void {
    this.cargar();
  }

  cargar(): void {
    this.cargando.set(true);
    this.areasService.getAll().subscribe({
      next: (data) => {
        this.areas.set(data);
        this.cargando.set(false);
      },
      error: () => this.cargando.set(false)
    });
  }

  crear(): void {
    const ref = this.dialog.open<AreaDialog, AreaDialogData>(AreaDialog, { data: { area: null } });
    ref.afterClosed().subscribe((resultado) => {
      if (!resultado) return;
      this.areasService.create(resultado).subscribe({
        next: () => {
          this.snackBar.open('Área creada', 'Cerrar', { duration: 2000 });
          this.cargar();
        },
        error: (err) => this.snackBar.open(err?.error?.message ?? 'Error al crear el área', 'Cerrar', { duration: 3000 })
      });
    });
  }

  editar(area: Area): void {
    const ref = this.dialog.open<AreaDialog, AreaDialogData>(AreaDialog, { data: { area } });
    ref.afterClosed().subscribe((resultado) => {
      if (!resultado) return;
      this.areasService.update(area.id, resultado).subscribe({
        next: () => {
          this.snackBar.open('Área actualizada', 'Cerrar', { duration: 2000 });
          this.cargar();
        },
        error: (err) => this.snackBar.open(err?.error?.message ?? 'Error al actualizar el área', 'Cerrar', { duration: 3000 })
      });
    });
  }

  eliminar(area: Area): void {
    if (!confirm(`¿Eliminar el área "${area.nombre}"?`)) return;
    this.areasService.delete(area.id).subscribe({
      next: () => {
        this.snackBar.open('Área eliminada', 'Cerrar', { duration: 2000 });
        this.cargar();
      },
      error: (err) => this.snackBar.open(err?.error?.message ?? 'Error al eliminar el área', 'Cerrar', { duration: 3000 })
    });
  }
}
