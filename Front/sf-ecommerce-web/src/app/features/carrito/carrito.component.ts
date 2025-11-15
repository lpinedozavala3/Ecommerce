import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';

import { CarritoService, ItemCarrito } from 'src/app/core/services/carrito.service';
import { CartApiService } from 'src/app/core/services/cart-api.service';
import { CartSummary } from 'src/app/core/models/cart';
import { StoreContextService } from 'src/app/core/services/store-context.service';

@Component({
  selector: 'app-carrito',
  templateUrl: './carrito.component.html',
  styleUrls: ['./carrito.component.scss']
})
export class CarritoComponent implements OnInit, OnDestroy {
  items: ItemCarrito[] = [];
  resumen?: CartSummary;
  mensajes: string[] = [];
  cargando = false;

  private sub?: Subscription;

  constructor(
    private carrito: CarritoService,
    private cartApi: CartApiService,
    private snack: MatSnackBar,
    public store: StoreContextService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.sub = this.carrito.items$.subscribe(items => {
      this.items = items;
      this.obtenerResumen();
    });
  }

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
  }

  actualizarCantidad(item: ItemCarrito, cantidad: number): void {
    const value = Math.max(1, Math.round(Number(cantidad) || 1));
    this.carrito.setCantidad(item.idProducto, value);
  }

  quitar(item: ItemCarrito): void {
    this.carrito.quitar(item.idProducto);
    this.snack.open('Producto eliminado del carrito', undefined, { duration: 2000 });
  }

  limpiar(): void {
    this.carrito.limpiar();
    this.snack.open('Carrito vacío', undefined, { duration: 2000 });
  }

  irAlCheckout(): void {
    this.router.navigate(this.store.storeLink('checkout'));
  }

  private obtenerResumen(): void {
    if (!this.items.length) {
      this.resumen = undefined;
      this.mensajes = [];
      return;
    }

    this.cargando = true;
    this.cartApi.resumen(this.items).subscribe({
      next: resumen => {
        this.resumen = resumen;
        this.mensajes = resumen.mensajes || [];
        resumen.items.forEach(det => {
          const item = this.items.find(i => i.idProducto === det.productoId);
          if (!item) return;
          if (det.stockDisponible <= 0) {
            this.carrito.quitar(item.idProducto);
          } else if (det.stockDisponible < item.cantidad) {
            this.carrito.setCantidad(item.idProducto, det.stockDisponible);
          }
        });
        this.cargando = false;
      },
      error: err => {
        this.cargando = false;
        this.resumen = undefined;
        const message = err?.error?.message || 'No pudimos calcular el resumen del carrito.';
        this.snack.open(message, 'Cerrar', { duration: 3000 });
      }
    });
  }

  trackById(_index: number, item: ItemCarrito): string {
    return item.idProducto;
  }
}
