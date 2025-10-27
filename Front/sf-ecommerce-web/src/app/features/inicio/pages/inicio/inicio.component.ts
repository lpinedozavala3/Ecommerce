// features/inicio/inicio.component.ts
import { Component, ChangeDetectorRef, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CatalogoService } from 'src/app/core/services/catalogo.service';
import { CarritoService } from 'src/app/core/services/carrito.service';
import { Producto } from 'src/app/core/models/producto';
import { Page } from 'src/app/core/models/Page';
import { ProductoFilter } from 'src/app/core/models/Filters/ProductoFilter';
import { PagedResponse } from 'src/app/core/models/Paged';
import { Categoria } from 'src/app/core/models/Categoria.';
import { Subscription, timer } from 'rxjs';

@Component({
  selector: 'app-inicio',
  templateUrl: './inicio.component.html',
  styleUrls: ['./inicio.component.scss']
})
export class InicioComponent implements OnInit, OnDestroy {
  searchText = '';
  selectedCategoriaId: string | null = null;

  productos: Producto[] = [];
  novedades: Producto[] = [];
  categorias: Categoria[] = [];

  loadingProductos = false;
  loadingNovedades = false;

  page = new Page();

  slides = [
    {
      title: 'Tu tienda digital minimalista',
      caption: 'Experiencias de compra simples, sin ruido y con envíos rápidos en todo el país.',
      theme: 'sunrise',
      cta: 'Explorar productos'
    },
    {
      title: 'Colecciones seleccionadas',
      caption: 'Cada semana curamos nuevas colecciones inspiradas en las tendencias del momento.',
      theme: 'night',
      cta: 'Ver colecciones'
    },
    {
      title: 'Pagos seguros y flexibles',
      caption: 'Aceptamos tarjetas, transferencias y billeteras digitales con confirmación inmediata.',
      theme: 'ocean',
      cta: 'Conocer más'
    }
  ];
  slideIndex = 0;

  private subs = new Subscription();
  private autoSlideSub?: Subscription;

  constructor(
    private catalogo: CatalogoService,
    private carrito: CarritoService,
    private ref: ChangeDetectorRef,
    private router: Router,
    private route: ActivatedRoute
  ) { this.starterPage(); }

  ngOnInit(): void {
    this.subs.add(
      this.catalogo.obtenerCategorias().subscribe(cats => {
        this.categorias = cats ?? [];
        this.ref.markForCheck();
      })
    );

    this.subs.add(
      this.route.queryParamMap.subscribe(params => {
        const search = params.get('q') ?? '';
        const categoria = params.get('categoria');
        const pageParam = Number(params.get('page') ?? '1');
        const pageIndex = Number.isFinite(pageParam) ? Math.max(pageParam - 1, 0) : 0;

        this.searchText = search;
        this.selectedCategoriaId = categoria;
        this.setPage({ offset: pageIndex });
      })
    );

    this.loadNovedades();
    this.startAutoSlide();
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
    this.autoSlideSub?.unsubscribe();
  }

  starterPage() {
    this.page.pageNumber = 0;
    this.page.pageSize = 12;
  }

  setPage(pageInfo: { offset: number }) {
    this.loadingProductos = true;
    this.page.pageNumber = pageInfo.offset;

    const filter: ProductoFilter = {
      searchText: this.searchText?.trim() || undefined,
      categoriaId: this.selectedCategoriaId || undefined
    };

    this.catalogo.getDataByPage(filter, this.page).subscribe({
      next: (res: PagedResponse<Producto>) => {
        this.productos = res.data;
        this.setPagefromResponse(this.page, res);
        this.loadingProductos = false;
        this.ref.detectChanges();
      },
      error: _ => { this.loadingProductos = false; this.ref.detectChanges(); }
    });
  }

  setPagefromResponse(current: Page, resp: PagedResponse<Producto>) {
    current.pageSize = resp.pageSize;
    current.pageNumber = Math.max((resp.pageNumber ?? 1) - 1, 0);
    current.totalPages = resp.totalPages;
    current.totalElements = resp.totalRecords;
  }

  onPageChange(e: { pageIndex: number; pageSize: number }) {
    this.page.pageSize = e.pageSize;
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { page: e.pageIndex + 1 },
      queryParamsHandling: 'merge'
    });
  }

  // Obtiene el nombre de la primera categoría del producto (si existe)
  getPrimaryCategoria(p: Producto): string | undefined {
    return (p.categorias && p.categorias.length) ? p.categorias[0].nombreCategoria : undefined;
  }

  loadNovedades(): void {
    this.loadingNovedades = true;
    const novedadesPage = new Page();
    novedadesPage.pageSize = 4;

    this.catalogo.getDataByPage({ esNovedad: true }, novedadesPage).subscribe({
      next: (res: PagedResponse<Producto>) => {
        this.novedades = res.data;
        this.loadingNovedades = false;
        this.ref.detectChanges();
      },
      error: _ => {
        this.loadingNovedades = false;
        this.ref.detectChanges();
      }
    });
  }

  selectCategoria(cat: Categoria): void {
    this.router.navigate(['/inicio'], {
      queryParams: { categoria: cat.idCategoria, page: 1 },
      queryParamsHandling: 'merge'
    });
  }

  onHeroCta(): void {
    this.router.navigate(['/novedades']);
  }

  private startAutoSlide(): void {
    this.autoSlideSub = timer(6000, 6000).subscribe(() => this.nextSlide());
  }

  nextSlide(): void {
    this.slideIndex = (this.slideIndex + 1) % this.slides.length;
  }

  prevSlide(): void {
    this.slideIndex = (this.slideIndex - 1 + this.slides.length) % this.slides.length;
  }

  get categoriaSeleccionadaNombre(): string | null {
    if (!this.selectedCategoriaId) {
      return null;
    }

    const categoria = this.categorias.find(c => c.idCategoria === this.selectedCategoriaId);
    return categoria?.nombreCategoria ?? null;
  }

  agregar(p: Producto) {
    this.carrito.agregar({
      idProducto: p.productoId,
      nombre: p.nombrePublico || 'Producto',
      imagenBase64: p.imagenBase64,
      // la API ahora devuelve `categorias` como array; enviamos el nombre de la
      // primera categoría (si existe) para mantener el formato anterior (string)
      categoria: (p.categorias && p.categorias.length) ? p.categorias[0].nombreCategoria : undefined,
      cantidad: 1,
      precioNeto: p.precio,
      iva: Math.round(p.precio * 0.19),
      esExento: !!p.exento
    });
  }

  verDetalle(p: Producto) {
    this.router.navigate(['/producto', p.productoId]);
  }
}
