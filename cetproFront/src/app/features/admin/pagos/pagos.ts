import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatChipsModule } from '@angular/material/chips';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { PagosService } from '../../../core/services/pagos.service';
import { PagoResumen } from '../../../core/models/matricula.models';

@Component({
  selector: 'app-pagos',
  standalone: true,
  imports: [
    FormsModule,
    MatTableModule,
    MatChipsModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatSelectModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './pagos.html'
})
export class Pagos implements OnInit {
  private readonly pagosService = inject(PagosService);
  private readonly snackBar = inject(MatSnackBar);

  readonly columnas = ['estudiante', 'curso', 'cuota', 'periodo', 'monto', 'estado', 'acciones'];
  readonly cargando = signal(true);
  readonly aprobandoId = signal<string | null>(null);
  readonly pagos = signal<PagoResumen[]>([]);
  readonly filtroEstado = signal<'' | 'pendiente' | 'aprobado'>('pendiente');

  ngOnInit(): void {
    this.cargar();
  }

  cargar(): void {
    this.cargando.set(true);
    this.pagosService.getAll(this.filtroEstado() || undefined).subscribe({
      next: (data) => {
        this.pagos.set(data);
        this.cargando.set(false);
      },
      error: () => this.cargando.set(false)
    });
  }

  onFiltroChange(estado: '' | 'pendiente' | 'aprobado'): void {
    this.filtroEstado.set(estado);
    this.cargar();
  }

  aprobar(pago: PagoResumen): void {
    this.aprobandoId.set(pago.id);

    this.pagosService.aprobar(pago.id).subscribe({
      next: () => {
        this.aprobandoId.set(null);
        this.snackBar.open(
          `Pago aprobado. El pago y la matrícula de ${pago.usuarioNombreCompleto} cambiaron de estado.`,
          'Cerrar',
          { duration: 4000 }
        );
        this.cargar();
      },
      error: (err) => {
        this.aprobandoId.set(null);
        this.snackBar.open(err?.error?.message ?? 'Error al aprobar el pago', 'Cerrar', { duration: 3000 });
      }
    });
  }
}
