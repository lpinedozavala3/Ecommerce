import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, of, throwError } from 'rxjs';

import { environment } from 'src/environments/environment';
import { DireccionCliente, PedidoResumen, UpsertDireccionPayload } from '../models/pedidos';
import { ApiResponse } from '../models/api-response';

@Injectable({ providedIn: 'root' })
export class PedidosService {
  private readonly baseUrl = `${environment.backend_server}/pedidos`;

  constructor(private http: HttpClient) {}

  obtenerPedidos(clienteId: string): Observable<PedidoResumen[]> {
    const params = new HttpParams().set('clienteId', clienteId);
    return this.http
      .get<ApiResponse<PedidoResumen[]>>(this.baseUrl, { params })
      .pipe(map(resp => resp.data ?? []));
  }

  obtenerDireccion(clienteId: string): Observable<DireccionCliente | null> {
    const params = new HttpParams().set('clienteId', clienteId);
    return this.http
      .get<ApiResponse<DireccionCliente>>(`${this.baseUrl}/direccion`, { params })
      .pipe(
        map(resp => resp.data ?? null),
        catchError((error: HttpErrorResponse) => {
          if (error.status === 404) {
            return of(null);
          }
          return throwError(() => error);
        })
      );
  }

  guardarDireccion(clienteId: string, payload: UpsertDireccionPayload): Observable<DireccionCliente> {
    const params = new HttpParams().set('clienteId', clienteId);
    return this.http
      .put<ApiResponse<DireccionCliente>>(`${this.baseUrl}/direccion`, payload, { params })
      .pipe(map(resp => resp.data));
  }
}
