import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { Area } from '../../../../core/models/academico.models';

export interface AreaDialogData {
  area: Area | null;
}

@Component({
  selector: 'app-area-dialog',
  standalone: true,
  imports: [ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  templateUrl: './area-dialog.html',
  styleUrl: './area-dialog.css'
})
export class AreaDialog {
  private readonly fb = inject(FormBuilder);
  private readonly dialogRef = inject(MatDialogRef<AreaDialog>);
  readonly data = inject<AreaDialogData>(MAT_DIALOG_DATA);

  readonly esEdicion = !!this.data.area;

  readonly form = this.fb.nonNullable.group({
    nombre: [this.data.area?.nombre ?? '', Validators.required],
    descripcion: [this.data.area?.descripcion ?? ''],
    imagenUrl: [this.data.area?.imagenUrl ?? '']
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
