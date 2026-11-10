import { Component, OnInit, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../../core/services/auth.service';
import { MatriculasService } from '../../../core/services/matriculas.service';
import { ContenidoService } from '../../../core/services/contenido.service';
import { NotificacionesService } from '../../../core/services/notificaciones.service';
import {
  DeudaEstudiante,
  MatriculaResumen,
  PagoAprobadoEvento,
  VerificarAcceso
} from '../../../core/models/matricula.models';
import { DeudaDialog } from './deuda-dialog/deuda-dialog';

@Component({
  selector: 'app-mis-cursos',
  standalone: true,
  imports: [MatCardModule, MatButtonModule, MatChipsModule, MatIconModule, MatProgressSpinnerModule],
  templateUrl: './mis-cursos.html',
  styleUrl: './mis-cursos.css'
})
export class MisCursos implements OnInit {
  private readonly authService = inject(AuthService);
  private readonly matriculasService = inject(MatriculasService);
  private readonly contenidoService = inject(ContenidoService);
  private readonly notificaciones = inject(NotificacionesService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);

  readonly cargando = signal(true);
  readonly matriculas = signal<MatriculaResumen[]>([]);
  readonly verificandoId = signal<string | null>(null);
  readonly resultadosAcceso = signal<Record<string, VerificarAcceso>>({});
  readonly deudas = signal<Record<string, DeudaEstudiante>>({});

  constructor() {
    this.notificaciones.pagoAprobado$
      .pipe(takeUntilDestroyed())
      .subscribe((evento) => this.alAprobarsePago(evento));
  }

  ngOnInit(): void {
    const usuarioId = this.authService.usuarioActual()?.usuarioId;
    if (!usuarioId) return;

    this.matriculasService.getAll(usuarioId).subscribe({
      next: (data) => {
        this.matriculas.set(data);
        this.cargando.set(false);
        data.forEach((m) => this.refrescarDeuda(m.id));
      },
      error: () => this.cargando.set(false)
    });
  }

  private alAprobarsePago(evento: PagoAprobadoEvento): void {
    const { pago, estadoMatricula } = evento;

    this.snackBar.open(`Tu pago de ${pago.periodo} fue aprobado`, 'Cerrar', { duration: 6000 });

    this.matriculas.update((lista) =>
      lista.map((m) => (m.id === pago.matriculaId ? { ...m, estado: estadoMatricula } : m))
    );

    // El estado de acceso pudo cambiar con este pago: se descarta el resultado viejo.
    const { [pago.matriculaId]: _descartado, ...restoAccesos } = this.resultadosAcceso();
    this.resultadosAcceso.set(restoAccesos);

    this.refrescarDeuda(pago.matriculaId);
  }

  private refrescarDeuda(matriculaId: string): void {
    this.matriculasService.calcularDeuda(matriculaId).subscribe({
      next: (deuda) => this.deudas.set({ ...this.deudas(), [matriculaId]: deuda }),
      error: () => undefined
    });
  }

  verDeuda(matricula: MatriculaResumen): void {
    this.matriculasService.calcularDeuda(matricula.id).subscribe({
      next: (deuda) => this.dialog.open(DeudaDialog, { data: deuda }),
      error: (err) => this.snackBar.open(err?.error?.message ?? 'No se pudo calcular la deuda', 'Cerrar', { duration: 3000 })
    });
  }

  verificarAcceso(matricula: MatriculaResumen): void {
    const usuarioId = this.authService.usuarioActual()?.usuarioId;
    if (!usuarioId) return;

    this.verificandoId.set(matricula.id);

    this.contenidoService.verificarAcceso(usuarioId, matricula.cursoId).subscribe({
      next: (resultado) => {
        this.verificandoId.set(null);
        this.resultadosAcceso.set({ ...this.resultadosAcceso(), [matricula.id]: resultado });
      },
      error: (err) => {
        this.verificandoId.set(null);
        this.snackBar.open(err?.error?.message ?? 'No se pudo verificar el acceso', 'Cerrar', { duration: 3000 });
      }
    });
  }

  accesoDe(matriculaId: string): VerificarAcceso | undefined {
    return this.resultadosAcceso()[matriculaId];
  }

  deudaDe(matriculaId: string): DeudaEstudiante | undefined {
    return this.deudas()[matriculaId];
  }
}
