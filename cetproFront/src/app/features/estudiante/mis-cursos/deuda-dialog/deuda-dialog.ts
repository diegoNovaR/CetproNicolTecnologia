import { Component, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { DeudaEstudiante } from '../../../../core/models/matricula.models';

@Component({
  selector: 'app-deuda-dialog',
  standalone: true,
  imports: [MatDialogModule, MatButtonModule, MatTableModule],
  templateUrl: './deuda-dialog.html',
  styleUrl: './deuda-dialog.css'
})
export class DeudaDialog {
  readonly deuda = inject<DeudaEstudiante>(MAT_DIALOG_DATA);
  readonly columnas = ['periodo', 'monto'];
}
