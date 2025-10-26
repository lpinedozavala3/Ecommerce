import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from 'src/environments/environment';
import { CartSummary } from '../models/cart';
import { ItemCarrito } from './carrito.service';

interface CartRequestItem {
  productoId: string;
  cantidad: number;
}

interface CartSummaryRequest {
  items: CartRequestItem[];
}

@Injectable({ providedIn: 'root' })
export class CartApiService {
  private readonly baseUrl = environment.backend_server + '/carrito';

  constructor(private http: HttpClient) {}

  resumen(items: ItemCarrito[]): Observable<CartSummary> {
    const payload: CartSummaryRequest = {
      items: items.map(i => ({ productoId: i.idProducto, cantidad: i.cantidad }))
    };
    return this.http.post<CartSummary>(`${this.baseUrl}/resumen`, payload);
  }
}
