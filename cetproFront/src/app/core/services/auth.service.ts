import { HttpClient } from '@angular/common/http';
import { Injectable, computed, signal } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthResponse, LoginRequest, RegistrarRequest, UsuarioActual } from '../models/auth.models';
import { getRoleFromToken, isTokenExpired } from './jwt.util';

const TOKEN_KEY = 'cetpro_token';
const USER_KEY = 'cetpro_usuario';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly baseUrl = `${environment.apiUrl}/auth`;

  private readonly usuarioActualSignal = signal<UsuarioActual | null>(this.leerUsuarioGuardado());

  readonly usuarioActual = this.usuarioActualSignal.asReadonly();
  readonly estaAutenticado = computed(() => !!this.usuarioActualSignal());

  constructor(private http: HttpClient) {}

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseUrl}/login`, request).pipe(
      tap((response) => this.guardarSesion(response))
    );
  }

  registrar(request: RegistrarRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseUrl}/registrar`, request).pipe(
      tap((response) => this.guardarSesion(response))
    );
  }

  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
    this.usuarioActualSignal.set(null);
  }

  getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  /** Rol tomado del JWT decodificado (no del cuerpo de la respuesta), como fuente de verdad para el guard. */
  getRolDesdeToken(): string | null {
    const token = this.getToken();
    return token ? getRoleFromToken(token) : null;
  }

  tieneSesionValida(): boolean {
    const token = this.getToken();
    return !!token && !isTokenExpired(token);
  }

  private guardarSesion(response: AuthResponse): void {
    localStorage.setItem(TOKEN_KEY, response.token);

    const usuario: UsuarioActual = {
      usuarioId: response.usuarioId,
      nombre: response.nombre,
      apellido: response.apellido,
      email: response.email,
      rol: getRoleFromToken(response.token) ?? response.rol
    };

    localStorage.setItem(USER_KEY, JSON.stringify(usuario));
    this.usuarioActualSignal.set(usuario);
  }

  private leerUsuarioGuardado(): UsuarioActual | null {
    const token = localStorage.getItem(TOKEN_KEY);
    const raw = localStorage.getItem(USER_KEY);
    if (!token || !raw || isTokenExpired(token)) return null;

    try {
      return JSON.parse(raw) as UsuarioActual;
    } catch {
      return null;
    }
  }
}
