import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';

import { CatalogoService } from 'src/app/core/services/catalogo.service';
import { CarritoService } from 'src/app/core/services/carrito.service';
import { ProductoDetalle } from 'src/app/core/models/producto';
import { StoreContextService } from 'src/app/core/services/store-context.service';

@Component({
  selector: 'app-producto-detalle',
  templateUrl: './producto-detalle.component.html',
  styleUrls: ['./producto-detalle.component.scss']
})
export class ProductoDetalleComponent implements OnInit, OnDestroy {
  producto?: ProductoDetalle;
  loading = true;
  private sub?: Subscription;

  constructor(
    private route: ActivatedRoute,
    private catalogo: CatalogoService,
    private carrito: CarritoService,
    private snack: MatSnackBar,
    public store: StoreContextService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.sub = this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (!id) {
        this.loading = false;
        return;
      }

      this.loading = true;
      this.catalogo.obtenerDetalle(id).subscribe({
        next: prod => {
          this.producto = prod;
          this.loading = false;
        },
        error: () => {
          this.producto = undefined;
          this.loading = false;
        }
      });
    });
  }

  getCategorias(): string {
    if (!this.producto?.categorias?.length) {
      return '';
    }
    return this.producto.categorias.map(c => c.nombreCategoria).join(' / ');
  }

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
  }

  agregarAlCarrito(): void {
    if (!this.producto) return;

    if (this.producto.stock <= 0) {
      this.snack.open('Sin stock disponible', 'Cerrar', { duration: 2500 });
      return;
    }

    this.carrito.agregar({
      idProducto: this.producto.productoId,
      nombre: this.producto.nombrePublico || 'Producto',
      imagenBase64: this.producto.imagenBase64,
      categoria: this.producto.categorias?.[0]?.nombreCategoria,
      cantidad: 1,
      precioNeto: this.producto.precio,
      iva: Math.round(this.producto.precio * 0.19),
      esExento: !!this.producto.exento
    });

    this.snack.open('Producto agregado al carrito', 'Ver carrito', {
      duration: 2500,
      horizontalPosition: 'right'
    }).onAction().subscribe(() => this.router.navigate(this.store.storeLink('carrito')));
  }
}
