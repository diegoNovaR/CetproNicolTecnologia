import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { Area, Curso } from '../../../../core/models/academico.models';

export interface CursoDialogData {
  curso: Curso | null;
  areas: Area[];
  areaIdPreseleccionada?: string;
}

@Component({
  selector: 'app-curso-dialog',
  standalone: true,
  imports: [ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule],
  templateUrl: './curso-dialog.html',
  styleUrl: './curso-dialog.css'
})
export class CursoDialog {
  private readonly fb = inject(FormBuilder);
  private readonly dialogRef = inject(MatDialogRef<CursoDialog>);
  readonly data = inject<CursoDialogData>(MAT_DIALOG_DATA);

  readonly esEdicion = !!this.data.curso;

  readonly form = this.fb.nonNullable.group({
    areaId: [this.data.curso?.areaId ?? this.data.areaIdPreseleccionada ?? '', Validators.required],
    nombre: [this.data.curso?.nombre ?? '', Validators.required],
    descripcion: [this.data.curso?.descripcion ?? ''],
    imagenUrl: [this.data.curso?.imagenUrl ?? '']
  });

  guardar(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.dialogRef.close(this.form.getRawValue());
  }

  cancelar(): void {
    this.dialogRef.close();
  }
}
