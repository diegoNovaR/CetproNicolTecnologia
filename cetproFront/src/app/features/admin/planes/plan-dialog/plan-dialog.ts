import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { Curso, PlanEstudio } from '../../../../core/models/academico.models';

export interface PlanDialogData {
  plan: PlanEstudio | null;
  cursos: Curso[];
  cursoIdPreseleccionado?: string;
}

@Component({
  selector: 'app-plan-dialog',
  standalone: true,
  imports: [ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule],
  templateUrl: './plan-dialog.html',
  styleUrl: './plan-dialog.css'
})
export class PlanDialog {
  private readonly fb = inject(FormBuilder);
  private readonly dialogRef = inject(MatDialogRef<PlanDialog>);
  readonly data = inject<PlanDialogData>(MAT_DIALOG_DATA);

  readonly esEdicion = !!this.data.plan;

  readonly form = this.fb.nonNullable.group({
    cursoId: [this.data.plan?.cursoId ?? this.data.cursoIdPreseleccionado ?? '', Validators.required],
    tipo: [this.data.plan?.tipo ?? 'carrera_completa', Validators.required],
    duracionMeses: [this.data.plan?.duracionMeses ?? 1, [Validators.required, Validators.min(1)]],
    precioMatricula: [this.data.plan?.precioMatricula ?? 0, [Validators.required, Validators.min(0)]],
    pensionMensual: [this.data.plan?.pensionMensual ?? 0, [Validators.required, Validators.min(0)]]
  });

  guardar(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const valor = this.form.getRawValue();
    if (this.esEdicion) {
      const { cursoId, ...actualizacion } = valor;
      this.dialogRef.close(actualizacion);
    } else {
      this.dialogRef.close(valor);
    }
  }

  cancelar(): void {
    this.dialogRef.close();
  }
}
