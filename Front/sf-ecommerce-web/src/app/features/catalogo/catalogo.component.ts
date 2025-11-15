import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';

import { CatalogoService } from 'src/app/core/services/catalogo.service';
import { CarritoService } from 'src/app/core/services/carrito.service';
import { Producto } from 'src/app/core/models/producto';
import { Page } from 'src/app/core/models/Page';
import { ProductoFilter } from 'src/app/core/models/Filters/ProductoFilter';
import { PagedResponse } from 'src/app/core/models/Paged';
import { Categoria } from 'src/app/core/models/Categoria.';
import { StoreContextService } from 'src/app/core/services/store-context.service';

@Component({
  selector: 'app-catalogo',
  templateUrl: './catalogo.component.html',
  styleUrls: ['./catalogo.component.scss']
})
export class CatalogoComponent implements OnInit, OnDestroy {
  productos: Producto[] = [];
  categorias: Categoria[] = [];

  searchControl = new FormControl('');
  categoriaControl = new FormControl(null);

  loadingProductos = false;
  page = new Page();
  selectedCategoriaId: string | null = null;

  private subs = new Subscription();

  constructor(
    private catalogo: CatalogoService,
    private carrito: CarritoService,
    private router: Router,
    private route: ActivatedRoute,
    private ref: ChangeDetectorRef,
    public store: StoreContextService
  ) {
    this.page.pageSize = 12;
  }

  ngOnInit(): void {
    this.subs.add(
      this.catalogo.obtenerCategorias().subscribe(categorias => {
        this.categorias = categorias ?? [];
        this.ref.markForCheck();
      })
    );

    this.subs.add(
      this.searchControl.valueChanges
        .pipe(
          debounceTime(300),
          map(value => (value || '').trim()),
          distinctUntilChanged()
        )
        .subscribe(term => {
          this.router.navigate([], {
            relativeTo: this.route,
            queryParams: { q: term || null, page: 1 },
            queryParamsHandling: 'merge'
          });
        })
    );

    this.subs.add(
      this.categoriaControl.valueChanges.pipe(distinctUntilChanged()).subscribe(categoriaId => {
        this.router.navigate([], {
          relativeTo: this.route,
          queryParams: { categoria: categoriaId || null, page: 1 },
          queryParamsHandling: 'merge'
        });
      })
    );

    this.subs.add(
      this.route.queryParamMap.subscribe(params => {
        const search = params.get('q') ?? '';
        const categoria = params.get('categoria');
        const pageParam = Number(params.get('page') ?? '1');
        const pageIndex = Number.isFinite(pageParam) ? Math.max(pageParam - 1, 0) : 0;

        if (this.searchControl.value !== search) {
          this.searchControl.setValue(search, { emitEvent: false });
        }

        if (this.categoriaControl.value !== (categoria ?? null)) {
          this.categoriaControl.setValue(categoria ?? null, { emitEvent: false });
        }

        this.selectedCategoriaId = categoria ?? null;
        this.setPage({ offset: pageIndex });
      })
    );
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  setPage(pageInfo: { offset: number }): void {
    this.loadingProductos = true;
    this.page.pageNumber = pageInfo.offset;

    const filter: ProductoFilter = {
      searchText: this.searchTerm || undefined,
      categoriaId: this.selectedCategoriaId || undefined
    };

    this.catalogo.getDataByPage(filter, this.page).subscribe({
      next: (res: PagedResponse<Producto>) => {
        this.productos = res.data;
        this.setPagefromResponse(this.page, res);
        this.loadingProductos = false;
        this.ref.detectChanges();
      },
      error: _ => {
        this.loadingProductos = false;
        this.ref.detectChanges();
      }
    });
  }

  private setPagefromResponse(current: Page, resp: PagedResponse<Producto>): void {
    current.pageSize = resp.pageSize;
    current.pageNumber = Math.max((resp.pageNumber ?? 1) - 1, 0);
    current.totalPages = resp.totalPages;
    current.totalElements = resp.totalRecords;
  }

  onPageChange(event: { pageIndex: number; pageSize: number }): void {
    this.page.pageSize = event.pageSize;
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { page: event.pageIndex + 1 },
      queryParamsHandling: 'merge'
    });
  }

  getPrimaryCategoria(producto: Producto): string | undefined {
    return producto.categorias && producto.categorias.length
      ? producto.categorias[0].nombreCategoria
      : undefined;
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

  clearFilters(): void {
    this.searchControl.setValue('', { emitEvent: false });
    this.categoriaControl.setValue(null, { emitEvent: false });
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { q: null, categoria: null, page: 1 }
    });
  }

  get hasActiveFilters(): boolean {
    return !!this.searchTerm || !!this.selectedCategoriaId;
  }

  get searchTerm(): string {
    return (this.searchControl.value || '').trim();
  }

  get showingRangeStart(): number {
    if (!this.page.totalElements) {
      return 0;
    }

    return this.page.pageNumber * this.page.pageSize + 1;
  }

  get showingRangeEnd(): number {
    if (!this.page.totalElements) {
      return 0;
    }

    return Math.min(this.showingRangeStart + this.productos.length - 1, this.page.totalElements);
  }

  get categoriaSeleccionadaNombre(): string | null {
    if (!this.selectedCategoriaId) {
      return null;
    }

    const categoria = this.categorias.find(cat => cat.idCategoria === this.selectedCategoriaId);
    return categoria?.nombreCategoria ?? null;
  }
}
