import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreateCurso, Curso, UpdateCurso } from '../models/academico.models';

@Injectable({ providedIn: 'root' })
export class CursosService {
  private readonly baseUrl = `${environment.apiUrl}/cursos`;

  constructor(private http: HttpClient) {}

  getAll(areaId?: string): Observable<Curso[]> {
    let params = new HttpParams();
    if (areaId) params = params.set('areaId', areaId);
    return this.http.get<Curso[]>(this.baseUrl, { params });
  }

  getById(id: string): Observable<Curso> {
    return this.http.get<Curso>(`${this.baseUrl}/${id}`);
  }

  create(dto: CreateCurso): Observable<Curso> {
    return this.http.post<Curso>(this.baseUrl, dto);
  }

  update(id: string, dto: UpdateCurso): Observable<Curso> {
    return this.http.put<Curso>(`${this.baseUrl}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
