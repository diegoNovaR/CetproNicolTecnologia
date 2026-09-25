import { Component, OnInit, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslatePipe, TranslateService } from '@ngx-translate/core';
import { catchError, debounceTime, distinctUntilChanged, filter, of } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';
import { AreasService } from '../../../core/services/areas.service';
import { CursosService } from '../../../core/services/cursos.service';
import { PlanesService } from '../../../core/services/planes.service';
import { MatriculasService } from '../../../core/services/matriculas.service';
import { ReniecService } from '../../../core/services/reniec.service';
import { Area, Curso, PlanEstudio } from '../../../core/models/academico.models';
import { Matricula } from '../../../core/models/matricula.models';
import { ReniecResponse } from '../../../core/models/reniec.models';

type EstadoVerificacionDni = 'idle' | 'buscando' | 'verificado' | 'no-encontrado';

@Component({
  selector: 'app-solicitar',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatCardModule,
    MatTableModule,
    MatChipsModule,
    MatIconModule,
    MatProgressSpinnerModule,
    TranslatePipe
  ],
  templateUrl: './solicitar.html',
  styleUrl: './solicitar.css'
})
export class Solicitar implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly areasService = inject(AreasService);
  private readonly cursosService = inject(CursosService);
  private readonly planesService = inject(PlanesService);
  private readonly matriculasService = inject(MatriculasService);
  private readonly reniecService = inject(ReniecService);
  private readonly snackBar = inject(MatSnackBar);
  private readonly translateService = inject(TranslateService);

  readonly columnasPagos = ['numeroCuota', 'periodo', 'monto', 'estado'];

  readonly areas = signal<Area[]>([]);
  readonly cursos = signal<Curso[]>([]);
  readonly planes = signal<PlanEstudio[]>([]);

  readonly enviando = signal(false);
  readonly matriculaCreada = signal<Matricula | null>(null);

  // Signals (no plain fields): esta app corre sin zone.js, así que un campo de
  // clase mutado a mano dentro de un subscribe no dispara detección de cambios.
  readonly verificacionEstado = signal<EstadoVerificacionDni>('idle');
  readonly datosReniec = signal<ReniecResponse | null>(null);

  readonly form = this.fb.nonNullable.group({
    dni: ['', [Validators.required, Validators.pattern(/^\d{8}$/)]],
    nombre: ['', Validators.required],
    apellido: ['', Validators.required],
    telefonoContacto: ['', [Validators.required, Validators.pattern(/^9\d{8}$/)]],
    areaId: ['', Validators.required],
    cursoId: [{ value: '', disabled: true }, Validators.required],
    planEstudioId: [{ value: '', disabled: true }, Validators.required]
  });

  ngOnInit(): void {
    this.areasService.getAll().subscribe((areas) => this.areas.set(areas.filter((a) => a.activo)));

    this.form.controls.dni.valueChanges.subscribe((dni) => {
      if (this.verificacionEstado() !== 'idle' && !/^\d{8}$/.test(dni)) {
        this.resetVerificacionDni();
      }
    });

    this.form.controls.dni.valueChanges
      .pipe(
        debounceTime(500),
        distinctUntilChanged(),
        filter((dni) => /^\d{8}$/.test(dni))
      )
      .subscribe((dni) => this.verificarDni(dni));

    this.form.controls.areaId.valueChanges.subscribe((areaId) => {
      this.cursos.set([]);
      this.planes.set([]);
      this.form.controls.cursoId.reset('');
      this.form.controls.planEstudioId.reset('');
      this.form.controls.planEstudioId.disable();

      if (!areaId) {
        this.form.controls.cursoId.disable();
        return;
      }

      this.form.controls.cursoId.enable();
      this.cursosService.getAll(areaId).subscribe((cursos) => this.cursos.set(cursos.filter((c) => c.activo)));
    });

    this.form.controls.cursoId.valueChanges.subscribe((cursoId) => {
      this.planes.set([]);
      this.form.controls.planEstudioId.reset('');

      if (!cursoId) {
        this.form.controls.planEstudioId.disable();
        return;
      }

      this.form.controls.planEstudioId.enable();
      this.planesService.getAll(cursoId).subscribe((planes) => this.planes.set(planes.filter((p) => p.activo)));
    });
  }

  private verificarDni(dni: string): void {
    this.verificacionEstado.set('buscando');

    this.reniecService
      .buscarPorDni(dni)
      .pipe(catchError(() => of(null)))
      .subscribe((resultado) => {
        if (resultado) {
          this.datosReniec.set(resultado);
          this.verificacionEstado.set('verificado');
          this.form.patchValue({
            nombre: resultado.nombres,
            apellido: `${resultado.apellidoPaterno} ${resultado.apellidoMaterno}`
          });
          this.form.controls.nombre.disable();
          this.form.controls.apellido.disable();
        } else {
          this.datosReniec.set(null);
          this.verificacionEstado.set('no-encontrado');
          this.form.controls.nombre.enable();
          this.form.controls.apellido.enable();
        }
      });
  }

  private resetVerificacionDni(): void {
    this.verificacionEstado.set('idle');
    this.datosReniec.set(null);
    this.form.controls.nombre.enable({ emitEvent: false });
    this.form.controls.apellido.enable({ emitEvent: false });
  }

  solicitar(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const usuarioId = this.authService.usuarioActual()?.usuarioId;
    if (!usuarioId) return;

    // Payload de demostración: DNI, teléfono y el flag nombreVerificado se
    // quedan solo en el frontend, el backend no los conoce todavía.
    const nombreVerificado = this.verificacionEstado() === 'verificado';
    const datosFormulario = this.form.getRawValue();
    console.log('[Demo] Datos capturados en el frontend (no enviados al backend):', {
      dni: datosFormulario.dni,
      nombre: datosFormulario.nombre,
      apellido: datosFormulario.apellido,
      telefonoContacto: datosFormulario.telefonoContacto,
      nombreVerificado
    });

    this.enviando.set(true);
    this.matriculaCreada.set(null);

    this.matriculasService
      .solicitar({ usuarioId, planEstudioId: datosFormulario.planEstudioId })
      .subscribe({
        next: (matricula) => {
          this.enviando.set(false);
          this.matriculaCreada.set(matricula);
          this.snackBar.open(
            this.translateService.instant('SOLICITAR.SUCCESS', { count: matricula.pagos.length }),
            this.translateService.instant('AUTH.CLOSE'),
            { duration: 4000 }
          );
          this.form.reset({ dni: '', nombre: '', apellido: '', telefonoContacto: '', areaId: '', cursoId: '', planEstudioId: '' });
          this.resetVerificacionDni();
        },
        error: (err) => {
          this.enviando.set(false);
          this.snackBar.open(
            err?.error?.message ?? this.translateService.instant('SOLICITAR.ERROR'),
            this.translateService.instant('AUTH.CLOSE'),
            { duration: 4000 }
          );
        }
      });
  }
}
