import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ReniecResponse } from '../models/reniec.models';

@Injectable({ providedIn: 'root' })
export class ReniecService {
  constructor(private http: HttpClient) {}

  buscarPorDni(dni: string): Observable<ReniecResponse> {
    return this.http.get<ReniecResponse>(`https://api.apis.net.pe/v2/reniec/dni?numero=${dni}`, {
      headers: { Authorization: `Bearer ${environment.reniecToken}` }
    });
  }
}
