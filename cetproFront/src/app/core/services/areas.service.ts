import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Area, CreateArea, UpdateArea } from '../models/academico.models';

@Injectable({ providedIn: 'root' })
export class AreasService {
  private readonly baseUrl = `${environment.apiUrl}/areas`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Area[]> {
    return this.http.get<Area[]>(this.baseUrl);
  }

  getById(id: string): Observable<Area> {
    return this.http.get<Area>(`${this.baseUrl}/${id}`);
  }

  create(dto: CreateArea): Observable<Area> {
    return this.http.post<Area>(this.baseUrl, dto);
  }

  update(id: string, dto: UpdateArea): Observable<Area> {
    return this.http.put<Area>(`${this.baseUrl}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
