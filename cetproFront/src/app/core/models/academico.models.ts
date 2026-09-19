export interface Area {
  id: string;
  nombre: string;
  descripcion?: string | null;
  imagenUrl?: string | null;
  activo: boolean;
  fechaCreacion: string;
}

export interface CreateArea {
  nombre: string;
  descripcion?: string | null;
  imagenUrl?: string | null;
}

export type UpdateArea = CreateArea;

export interface Curso {
  id: string;
  areaId: string;
  nombre: string;
  descripcion?: string | null;
  imagenUrl?: string | null;
  activo: boolean;
  fechaCreacion: string;
}

export interface CreateCurso {
  areaId: string;
  nombre: string;
  descripcion?: string | null;
  imagenUrl?: string | null;
}

export type UpdateCurso = CreateCurso;

export type TipoPlanEstudio = 'carrera_completa' | 'modulo';

export interface PlanEstudio {
  id: string;
  cursoId: string;
  tipo: TipoPlanEstudio;
  duracionMeses: number;
  precioMatricula: number;
  pensionMensual: number;
  activo: boolean;
}

export interface CreatePlanEstudio {
  cursoId: string;
  tipo: TipoPlanEstudio;
  duracionMeses: number;
  precioMatricula: number;
  pensionMensual: number;
}

export type UpdatePlanEstudio = Omit<CreatePlanEstudio, 'cursoId'>;
