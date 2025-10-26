// core/services/catalogo.service.ts
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Page } from '../models/Page';
import { PagedResponse } from '../models/Paged';
import { ProductoFilter } from '../models/Filters/ProductoFilter';
import { Categoria } from '../models/Categoria.';
import { Producto, ProductoDetalle } from '../models/producto';

@Injectable({ providedIn: 'root' })
export class CatalogoService {
  private REST_API_SERVER = environment.backend_server + '/catalogo';

  constructor(private http: HttpClient) {}

  // core/services/catalogo.service.ts
getDataByPage(filter: ProductoFilter, page: Page): Observable<PagedResponse<Producto>> {
  let params = new HttpParams()
    .set('pageNumber', String(page.pageNumber + 1)) // 1-based en API
    .set('pageSize',   String(page.pageSize));

  if (filter?.searchText?.trim()) params = params.set('searchText', filter.searchText.trim());
  if (filter?.categoriaId)        params = params.set('categoriaId', filter.categoriaId);
  return this.http.get<PagedResponse<Producto>>(
    `${this.REST_API_SERVER}/productos/list`,
    { params }
  );
}


  obtenerCategorias(): Observable<Categoria[]> {
    return this.http.get<Categoria[]>(`${this.REST_API_SERVER}/categorias`);
  }

  obtenerDetalle(productoId: string): Observable<ProductoDetalle> {
    return this.http.get<ProductoDetalle>(`${this.REST_API_SERVER}/productos/${productoId}`);
  }

  // (Opcional) compatibilidad si aún lo usas en otra vista
  obtenerProductos(searchText?: string, categoriaId?: string): Observable<Producto[]> {
    let params = new HttpParams();
    if (searchText)  params = params.set('searchText', searchText);
    if (categoriaId) params = params.set('categoriaId', categoriaId);
    return this.http.get<Producto[]>(`${this.REST_API_SERVER}/productos`, { params });
  }
}
