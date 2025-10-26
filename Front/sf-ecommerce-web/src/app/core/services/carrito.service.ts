import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface ItemCarrito {
  idProducto: string;
  nombre: string;
  imagenBase64?: string;
  categoria?: string | null;
  cantidad: number;
  precioNeto: number;
  iva: number;
  esExento: boolean;
}

const KEY = 'sf-cart-v1';

@Injectable({ providedIn: 'root' })
export class CarritoService {
  private _items = new BehaviorSubject<ItemCarrito[]>(this.leer());
  items$ = this._items.asObservable();
  get items() { return this._items.value; }

  private guardar(list: ItemCarrito[]) { localStorage.setItem(KEY, JSON.stringify(list)); }
  private leer(): ItemCarrito[] {
    try { return JSON.parse(localStorage.getItem(KEY) || '[]'); } catch { return []; }
  }

  // 👇 ESTE es el método que estás llamando desde InicioComponent
  agregar(it: ItemCarrito) {
    const list = [...this.items];
    const i = list.findIndex(x => x.idProducto === it.idProducto);
    if (i >= 0) list[i] = { ...list[i], cantidad: list[i].cantidad + it.cantidad };
    else list.push(it);
    this._items.next(list); this.guardar(list);
  }

  setCantidad(id: string, cant: number) {
    const list = this.items.map(x => x.idProducto === id ? { ...x, cantidad: Math.max(1, cant) } : x);
    this._items.next(list); this.guardar(list);
  }

  quitar(id: string) {
    const list = this.items.filter(x => x.idProducto !== id);
    this._items.next(list); this.guardar(list);
  }

  limpiar() { this._items.next([]); this.guardar([]); }

  totalNeto() { return this.items.reduce((s, x) => s + x.precioNeto * x.cantidad, 0); }
  totalIva()  { return this.items.reduce((s, x) => s + x.iva * x.cantidad, 0); }
  total()     { return this.totalNeto() + this.totalIva(); }
}
