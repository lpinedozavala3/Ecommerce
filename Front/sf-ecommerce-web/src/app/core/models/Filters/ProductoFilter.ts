export interface ProductoFilter {
  searchText?: string;            // en vez de "q"
  categoriaId?: string | null;
  esNovedad?: boolean;
}
