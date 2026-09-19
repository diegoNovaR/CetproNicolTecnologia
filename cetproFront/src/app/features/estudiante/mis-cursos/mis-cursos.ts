import { Component, OnInit, inject, signal } from '@angular/core';
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
import { MatriculaResumen, VerificarAcceso } from '../../../core/models/matricula.models';
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
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);

  readonly cargando = signal(true);
  readonly matriculas = signal<MatriculaResumen[]>([]);
  readonly verificandoId = signal<string | null>(null);
  readonly resultadosAcceso = signal<Record<string, VerificarAcceso>>({});

  ngOnInit(): void {
    const usuarioId = this.authService.usuarioActual()?.usuarioId;
    if (!usuarioId) return;

    this.matriculasService.getAll(usuarioId).subscribe({
      next: (data) => {
        this.matriculas.set(data);
        this.cargando.set(false);
      },
      error: () => this.cargando.set(false)
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
}
