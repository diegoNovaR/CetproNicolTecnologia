import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreatePlanEstudio, PlanEstudio, UpdatePlanEstudio } from '../models/academico.models';

@Injectable({ providedIn: 'root' })
export class PlanesService {
  private readonly baseUrl = `${environment.apiUrl}/planes-estudio`;

  constructor(private http: HttpClient) {}

  getAll(cursoId?: string): Observable<PlanEstudio[]> {
    let params = new HttpParams();
    if (cursoId) params = params.set('cursoId', cursoId);
    return this.http.get<PlanEstudio[]>(this.baseUrl, { params });
  }

  getById(id: string): Observable<PlanEstudio> {
    return this.http.get<PlanEstudio>(`${this.baseUrl}/${id}`);
  }

  create(dto: CreatePlanEstudio): Observable<PlanEstudio> {
    return this.http.post<PlanEstudio>(this.baseUrl, dto);
  }

  update(id: string, dto: UpdatePlanEstudio): Observable<PlanEstudio> {
    return this.http.put<PlanEstudio>(`${this.baseUrl}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
