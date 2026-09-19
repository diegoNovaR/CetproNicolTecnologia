export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegistrarRequest {
  nombre: string;
  apellido: string;
  email: string;
  telefono?: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  usuarioId: string;
  nombre: string;
  apellido: string;
  email: string;
  rol: string;
}

export interface UsuarioActual {
  usuarioId: string;
  nombre: string;
  apellido: string;
  email: string;
  rol: string;
}
