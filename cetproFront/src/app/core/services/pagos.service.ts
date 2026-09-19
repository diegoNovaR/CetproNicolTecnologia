import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Pago, PagoResumen } from '../models/matricula.models';

@Injectable({ providedIn: 'root' })
export class PagosService {
  private readonly baseUrl = `${environment.apiUrl}/pagos`;

  constructor(private http: HttpClient) {}

  getAll(estado?: 'pendiente' | 'aprobado'): Observable<PagoResumen[]> {
    let params = new HttpParams();
    if (estado) params = params.set('estado', estado);
    return this.http.get<PagoResumen[]>(this.baseUrl, { params });
  }

  aprobar(pagoId: string): Observable<Pago> {
    return this.http.put<Pago>(`${this.baseUrl}/${pagoId}/aprobar`, {});
  }
}
