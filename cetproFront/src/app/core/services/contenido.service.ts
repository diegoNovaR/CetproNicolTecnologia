import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { VerificarAcceso } from '../models/matricula.models';

@Injectable({ providedIn: 'root' })
export class ContenidoService {
  private readonly baseUrl = `${environment.apiUrl}/contenidos`;

  constructor(private http: HttpClient) {}

  verificarAcceso(usuarioId: string, cursoId: string): Observable<VerificarAcceso> {
    const params = new HttpParams().set('usuarioId', usuarioId).set('cursoId', cursoId);
    return this.http.get<VerificarAcceso>(`${this.baseUrl}/verificar-acceso`, { params });
  }
}
