import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from 'src/environments/environment';
import { DireccionCliente, PedidoResumen, UpsertDireccionPayload } from '../models/pedidos';

@Injectable({ providedIn: 'root' })
export class PedidosService {
  private readonly baseUrl = `${environment.backend_server}/pedidos`;

  constructor(private http: HttpClient) {}

  obtenerPedidos(clienteId: string): Observable<PedidoResumen[]> {
    const params = new HttpParams().set('clienteId', clienteId);
    return this.http.get<PedidoResumen[]>(this.baseUrl, { params });
  }

  obtenerDireccion(clienteId: string): Observable<DireccionCliente | null> {
    const params = new HttpParams().set('clienteId', clienteId);
    return this.http
      .get<DireccionCliente>(`${this.baseUrl}/direccion`, { params, observe: 'response' })
      .pipe(map((resp: HttpResponse<DireccionCliente>) => resp.body ?? null));
  }

  guardarDireccion(clienteId: string, payload: UpsertDireccionPayload): Observable<DireccionCliente> {
    const params = new HttpParams().set('clienteId', clienteId);
    return this.http.put<DireccionCliente>(`${this.baseUrl}/direccion`, payload, { params });
  }
}
