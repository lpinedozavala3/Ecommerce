import { Component, ChangeDetectorRef, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Page } from 'src/app/core/models/Page';
import { Producto } from 'src/app/core/models/producto';
import { CatalogoService } from 'src/app/core/services/catalogo.service';
import { CarritoService } from 'src/app/core/services/carrito.service';
import { ProductoFilter } from 'src/app/core/models/Filters/ProductoFilter';
import { PagedResponse } from 'src/app/core/models/Paged';
import { StoreContextService } from 'src/app/core/services/store-context.service';

@Component({
  selector: 'app-novedades',
  templateUrl: './novedades.component.html',
  styleUrls: ['./novedades.component.scss']
})
export class NovedadesComponent implements OnInit {
  productos: Producto[] = [];
  loading = false;
  page = new Page();

  constructor(
    private catalogo: CatalogoService,
    private carrito: CarritoService,
    private router: Router,
    private ref: ChangeDetectorRef,
    public store: StoreContextService
  ) {
    this.page.pageSize = 12;
  }

  ngOnInit(): void {
    this.setPage({ offset: 0 });
  }

  setPage(pageInfo: { offset: number }): void {
    this.loading = true;
    this.page.pageNumber = pageInfo.offset;

    const filter: ProductoFilter = { esNovedad: true };

    this.catalogo.getDataByPage(filter, this.page).subscribe({
      next: (res: PagedResponse<Producto>) => {
        this.productos = res.data;
        this.page.totalElements = res.totalRecords;
        this.page.totalPages = res.totalPages;
        this.page.pageNumber = Math.max((res.pageNumber ?? 1) - 1, 0);
        this.page.pageSize = res.pageSize;
        this.loading = false;
        this.ref.detectChanges();
      },
      error: () => {
        this.loading = false;
        this.ref.detectChanges();
      }
    });
  }

  onPageChange(e: { pageIndex: number; pageSize: number }): void {
    this.page.pageSize = e.pageSize;
    this.setPage({ offset: e.pageIndex });
  }

  agregar(producto: Producto): void {
    this.carrito.agregar({
      idProducto: producto.productoId,
      nombre: producto.nombrePublico || 'Producto',
      imagenBase64: producto.imagenBase64,
      categoria: this.getPrimaryCategoria(producto),
      cantidad: 1,
      precioNeto: producto.precio,
      iva: Math.round(producto.precio * 0.19),
      esExento: !!producto.exento
    });
  }

  verDetalle(producto: Producto): void {
    this.router.navigate(this.store.storeLink('producto', producto.productoId));
  }

  getPrimaryCategoria(producto: Producto): string | undefined {
    return producto.categorias?.[0]?.nombreCategoria;
  }
}
