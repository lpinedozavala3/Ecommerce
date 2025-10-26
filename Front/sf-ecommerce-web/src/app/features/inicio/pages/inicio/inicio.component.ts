// features/inicio/inicio.component.ts
import { Component, ChangeDetectorRef, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CatalogoService } from 'src/app/core/services/catalogo.service';
import { CarritoService } from 'src/app/core/services/carrito.service';
import { Producto } from 'src/app/core/models/producto';
import { Subject } from 'rxjs';
import { Categoria } from 'src/app/core/models/producto';
import { Page } from 'src/app/core/models/Page';
import { ProductoFilter } from 'src/app/core/models/Filters/ProductoFilter';
import { PagedResponse } from 'src/app/core/models/Paged';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-inicio',
  templateUrl: './inicio.component.html',
  styleUrls: ['./inicio.component.scss']
})
export class InicioComponent implements OnInit {

  // Filtro claro
  searchText = '';
  selectedCategoriaId: string | null = null;

  // Datos UI
  categorias: Categoria[] = [];           // cargadas desde API
  productos: Producto[] = [];
  loading = false;

  // Paginación (mismo patrón)
  page = new Page();

  private search$ = new Subject<void>();

  constructor(
    private catalogo: CatalogoService,
    private carrito: CarritoService,
    private ref: ChangeDetectorRef,
    private router: Router
  ) { this.starterPage(); }

  ngOnInit(): void {
    this.catalogo.obtenerCategorias().subscribe(cats => {
      // Insertamos “Todos” (id null) como opción UI:
      this.categorias = [{ idCategoria: '', nombreCategoria: 'Todos', slugCategoria: 'todos' }, ...(cats || [])];
    });

    this.search$.pipe(debounceTime(300)).subscribe(() => this.setPage({ offset: 0 }));
    this.setPage({ offset: 0 });
  }

  starterPage() {
    this.page.pageNumber = 0;
    this.page.pageSize = 12;
  }

  setPage(pageInfo: { offset: number }) {
    this.loading = true;
    this.page.pageNumber = pageInfo.offset;

    const filter: ProductoFilter = {
      searchText: this.searchText?.trim() || undefined,
      categoriaId: this.selectedCategoriaId || undefined
    };

    this.catalogo.getDataByPage(filter, this.page).subscribe({
      next: (res: PagedResponse<Producto>) => {
        this.productos = res.data;
        console.log(this.productos);
        this.setPagefromResponse(this.page, res);
        this.loading = false;
        this.ref.detectChanges();
      },
      error: _ => { this.loading = false; this.ref.detectChanges(); }
    });
  }

  setPagefromResponse(current: Page, resp: PagedResponse<Producto>) {
    current.pageSize = resp.pageSize;
    current.pageNumber = Math.max((resp.pageNumber ?? 1) - 1, 0);
    current.totalPages = resp.totalPages;
    current.totalElements = resp.totalRecords;
  }

  onEnterSearch() { this.search$.next(); }
  clearSearch()    { this.searchText = ''; this.search$.next(); }

  selectCategoria(cat: Categoria) {
    this.selectedCategoriaId = cat.idCategoria || null; // vacío => “Todos”
    this.setPage({ offset: 0 });
  }

  onPageChange(e: { pageIndex: number; pageSize: number }) {
    this.page.pageSize = e.pageSize;
    this.setPage({ offset: e.pageIndex });
  }

  // Obtiene el nombre de la primera categoría del producto (si existe)
  getPrimaryCategoria(p: Producto): string | undefined {
    return (p.categorias && p.categorias.length) ? p.categorias[0].nombreCategoria : undefined;
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
