export interface PedidoItem {
  ordenItemId: string;
  ordenId: string;
  productoId: string;
  nombre: string;
  cantidad: number;
  precioNeto: number;
  iva: number;
  esExento: boolean;
  subtotal: number;
}

export interface PedidoResumen {
  ordenId: string;
  codigo: string;
  creadoEn: string;
  estado: string;
  totalNeto: number;
  totalIva: number;
  total: number;
  items: PedidoItem[];
}

export interface DireccionCliente {
  idDireccion: string;
  clienteId: string;
  calle: string;
  numero?: string | null;
  depto?: string | null;
  comuna: string;
  ciudad: string;
  region: string;
  pais: string;
  codigoPostal?: string | null;
  referencias?: string | null;
  esPrincipal: boolean;
  creadoEn: string;
  actualizadoEn?: string | null;
}

export interface UpsertDireccionPayload {
  calle: string;
  numero?: string | null;
  depto?: string | null;
  comuna: string;
  ciudad: string;
  region: string;
  pais: string;
  codigoPostal?: string | null;
  referencias?: string | null;
}
