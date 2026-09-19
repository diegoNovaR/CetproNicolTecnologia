import { DatePipe } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatChipsModule } from '@angular/material/chips';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatriculasService } from '../../../core/services/matriculas.service';
import { MatriculaResumen } from '../../../core/models/matricula.models';

@Component({
  selector: 'app-matriculas',
  standalone: true,
  imports: [DatePipe, MatTableModule, MatChipsModule, MatButtonModule, MatIconModule, MatProgressSpinnerModule],
  templateUrl: './matriculas.html'
})
export class Matriculas implements OnInit {
  private readonly matriculasService = inject(MatriculasService);

  readonly columnas = ['estudiante', 'curso', 'plan', 'estado', 'fechaSolicitud', 'fechaAprobacion'];
  readonly cargando = signal(true);
  readonly matriculas = signal<MatriculaResumen[]>([]);

  ngOnInit(): void {
    this.cargar();
  }

  cargar(): void {
    this.cargando.set(true);
    this.matriculasService.getAll().subscribe({
      next: (data) => {
        this.matriculas.set(data);
        this.cargando.set(false);
      },
      error: () => this.cargando.set(false)
    });
  }

  colorEstado(estado: string): 'primary' | 'accent' | 'warn' {
    if (estado === 'activa') return 'primary';
    if (estado === 'pendiente') return 'accent';
    return 'warn';
  }
}
