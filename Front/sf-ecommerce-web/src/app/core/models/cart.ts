export interface CartItemSummary {
  productoId: string;
  nombre: string;
  imagenBase64?: string;
  precio: number;
  cantidadSolicitada: number;
  stockDisponible: number;
  exento: boolean;
  subtotal: number;
  iva: number;
}

export interface CartSummary {
  items: CartItemSummary[];
  subtotal: number;
  impuestos: number;
  envio: number;
  total: number;
  mensajes: string[];
}
