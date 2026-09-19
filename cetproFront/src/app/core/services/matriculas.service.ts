import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  DeudaEstudiante,
  Matricula,
  MatricularRequest,
  MatriculaResumen
} from '../models/matricula.models';

@Injectable({ providedIn: 'root' })
export class MatriculasService {
  private readonly baseUrl = `${environment.apiUrl}/matriculas`;

  constructor(private http: HttpClient) {}

  solicitar(request: MatricularRequest): Observable<Matricula> {
    return this.http.post<Matricula>(this.baseUrl, request);
  }

  getAll(usuarioId?: string): Observable<MatriculaResumen[]> {
    let params = new HttpParams();
    if (usuarioId) params = params.set('usuarioId', usuarioId);
    return this.http.get<MatriculaResumen[]>(this.baseUrl, { params });
  }

  calcularDeuda(matriculaId: string): Observable<DeudaEstudiante> {
    return this.http.get<DeudaEstudiante>(`${this.baseUrl}/${matriculaId}/deuda`);
  }
}
