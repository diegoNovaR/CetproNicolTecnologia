import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Pais } from '../models/paises.models';

// Regiones fijas: la API no expone un endpoint que las liste.
// `valor` es el segmento que espera GET /region/{valor}.
export const REGIONES_PAISES = [
  { valor: 'americas', clave: 'PAISES.REGIONS.AMERICAS' },
  { valor: 'asia', clave: 'PAISES.REGIONS.ASIA' },
  { valor: 'europe', clave: 'PAISES.REGIONS.EUROPE' },
  { valor: 'africa', clave: 'PAISES.REGIONS.AFRICA' },
  { valor: 'oceania', clave: 'PAISES.REGIONS.OCEANIA' },
  { valor: 'antarctic', clave: 'PAISES.REGIONS.ANTARCTIC' }
] as const;

@Injectable({ providedIn: 'root' })
export class PaisesService {
  private readonly baseUrl = environment.countriesApiUrl;

  constructor(private http: HttpClient) {}

  getPorRegion(region: string): Observable<Pais[]> {
    return this.http
      .get<Pais[]>(`${this.baseUrl}/region/${region}`)
      .pipe(map((paises) => [...paises].sort((a, b) => a.name.localeCompare(b.name))));
  }
}
