export interface AuthResponse {
  clienteId: string;
  tiendaId: string;
  email: string;
  nombre: string;
  apellido: string;
  telefono?: string | null;
  token: string;
  emitidoEn: string;
}

export interface LoginPayload {
  email: string;
  password: string;
  recordar?: boolean;
}

export interface RegistroPayload {
  email: string;
  password: string;
  nombre: string;
  apellido: string;
  telefono?: string;
}

export interface UsuarioSesion {
  id: string;
  email: string;
  nombreCompleto: string;
  token: string;
}
