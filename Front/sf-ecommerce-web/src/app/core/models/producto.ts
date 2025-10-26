// Nota: la API ahora devuelve `categorias` como un array de objetos Categoria
// Temporal: definimos aquí la interfaz `Categoria` localmente porque el fichero
// original en el repo tiene un nombre con puntos (`Categoria..ts`) que rompe
// la resolución automática del import en algunos entornos. Reemplazar por
// "import { Categoria } from './Categoria..'" cuando se normalice el nombre.
export interface Categoria {
  idCategoria: string;
  nombreCategoria: string;
  slugCategoria: string;
}

export interface Producto {
  productoId: string;
  nombrePublico: string | null;
  precio: number;
  stock: number;
  imagenBase64: string;
  visibleEnTienda: boolean;
  categorias?: Categoria[];
  destacado?: boolean;
  exento?: boolean;
  novedad?: boolean;
}

export interface ProductoDetalle extends Producto {
  descripcionCorta?: string | null;
  descripcionLarga?: string | null;
  codigoBarra?: string | null;
  unidadMedida?: string | null;
  activo?: boolean;
}
