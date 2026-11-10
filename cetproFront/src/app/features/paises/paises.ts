import { Location } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { MatToolbarModule } from '@angular/material/toolbar';
import { TranslatePipe } from '@ngx-translate/core';
import { Subject, catchError, of, switchMap, tap } from 'rxjs';
import { Pais } from '../../core/models/paises.models';
import { PaisesService, REGIONES_PAISES } from '../../core/services/paises.service';

@Component({
  selector: 'app-paises',
  standalone: true,
  imports: [
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatFormFieldModule,
    MatSelectModule,
    MatTableModule,
    MatProgressSpinnerModule,
    TranslatePipe
  ],
  templateUrl: './paises.html',
  styleUrl: './paises.css'
})
export class Paises {
  private readonly paisesService = inject(PaisesService);
  private readonly location = inject(Location);
  private readonly regionElegida = new Subject<string>();

  readonly regiones = REGIONES_PAISES;
  readonly columnas = ['idioma', 'nombreNativo', 'codigo'];

  readonly regionSeleccionada = signal<string | null>(null);
  readonly codigoPais = signal<string | null>(null);
  readonly paises = signal<Pais[]>([]);
  readonly cargando = signal(false);
  readonly error = signal(false);

  readonly paisSeleccionado = computed(
    () => this.paises().find((p) => p.alpha3Code === this.codigoPais()) ?? null
  );

  constructor() {
    // switchMap descarta la petición anterior si se cambia de región antes de que responda.
    this.regionElegida
      .pipe(
        tap(() => {
          this.cargando.set(true);
          this.error.set(false);
          this.paises.set([]);
          this.codigoPais.set(null);
        }),
        switchMap((region) =>
          this.paisesService.getPorRegion(region).pipe(
            catchError(() => {
              this.error.set(true);
              return of<Pais[]>([]);
            })
          )
        ),
        takeUntilDestroyed()
      )
      .subscribe((paises) => {
        this.paises.set(paises);
        this.cargando.set(false);
      });
  }

  alCambiarRegion(region: string): void {
    this.regionSeleccionada.set(region);
    this.regionElegida.next(region);
  }

  alCambiarPais(codigo: string): void {
    this.codigoPais.set(codigo);
  }

  volver(): void {
    this.location.back();
  }
}
