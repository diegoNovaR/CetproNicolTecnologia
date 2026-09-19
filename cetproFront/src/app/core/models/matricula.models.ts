export type EstadoMatricula = 'pendiente' | 'activa' | 'completada' | 'cancelada';
export type EstadoPago = 'pendiente' | 'aprobado';

export interface Pago {
  id: string;
  matriculaId: string;
  numeroCuota: number;
  periodo: string;
  monto: number;
  estado: EstadoPago;
  fechaPago?: string | null;
}

export interface Matricula {
  id: string;
  usuarioId: string;
  planEstudioId: string;
  estado: EstadoMatricula;
  fechaSolicitud: string;
  fechaAprobacion?: string | null;
  pagos: Pago[];
}

export interface MatricularRequest {
  usuarioId: string;
  planEstudioId: string;
}

export interface MatriculaResumen {
  id: string;
  usuarioId: string;
  usuarioNombreCompleto: string;
  cursoId: string;
  cursoNombre: string;
  planEstudioId: string;
  planTipo: string;
  estado: EstadoMatricula;
  fechaSolicitud: string;
  fechaAprobacion?: string | null;
}

export interface PagoResumen {
  id: string;
  matriculaId: string;
  usuarioNombreCompleto: string;
  cursoNombre: string;
  numeroCuota: number;
  periodo: string;
  monto: number;
  estado: EstadoPago;
  fechaPago?: string | null;
}

export interface CuotaPendiente {
  periodo: string;
  monto: number;
}

export interface DeudaEstudiante {
  matriculaId: string;
  totalDeuda: number;
  cuotasPendientes: number;
  detalleCuotas: CuotaPendiente[];
}

export interface VerificarAcceso {
  tieneAcceso: boolean;
  motivoRechazo?: string | null;
}
