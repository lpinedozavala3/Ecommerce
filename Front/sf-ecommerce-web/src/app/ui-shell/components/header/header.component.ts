import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ActivatedRouteSnapshot, NavigationEnd, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';

import { CarritoService } from 'src/app/core/services/carrito.service';
import { AuthStateService } from 'src/app/core/services/auth-state.service';
import { UsuarioSesion } from 'src/app/core/models/auth';
import { CatalogoService } from 'src/app/core/services/catalogo.service';
import { Categoria } from 'src/app/core/models/Categoria.';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {
  itemsEnCarrito = 0;
  usuario: UsuarioSesion | null = null;
  categorias: Categoria[] = [];
  searchControl = new FormControl('', { nonNullable: true });
  selectedCategoriaId: string | null = null;

  private subs: Subscription[] = [];

  constructor(
    private carrito: CarritoService,
    private authState: AuthStateService,
    private catalogo: CatalogoService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.subs.push(
      this.carrito.items$.subscribe(items => (this.itemsEnCarrito = items.reduce((sum, item) => sum + item.cantidad, 0)))
    );

    this.subs.push(
      this.authState.usuario$.subscribe(usuario => (this.usuario = usuario))
    );

    this.subs.push(
      this.catalogo.obtenerCategorias().subscribe(categorias => (this.categorias = categorias ?? []))
    );

    this.syncFromRoute();

    this.subs.push(
      this.router.events.pipe(filter(event => event instanceof NavigationEnd)).subscribe(() => this.syncFromRoute())
    );
  }

  ngOnDestroy(): void {
    this.subs.forEach(s => s.unsubscribe());
  }

  cerrarSesion(): void {
    this.authState.cerrarSesion();
  }

  submitSearch(): void {
    const term = this.searchControl.value.trim();
    this.router.navigate(['/inicio'], {
      queryParams: { q: term || null, page: 1 },
      queryParamsHandling: 'merge'
    });
  }

  clearSearch(event: MouseEvent): void {
    event.preventDefault();
    this.searchControl.setValue('');
    this.router.navigate(['/inicio'], {
      queryParams: { q: null, page: 1 },
      queryParamsHandling: 'merge'
    });
  }

  navegarCategoria(cat: Categoria): void {
    this.router.navigate(['/inicio'], {
      queryParams: { categoria: cat.idCategoria, page: 1 },
      queryParamsHandling: 'merge'
    });
  }

  irAlCarrito(): void {
    this.router.navigate(['/carrito']);
  }

  private syncFromRoute(): void {
    const root = this.router.routerState.snapshot.root;
    const search = this.lookupQueryParam(root, 'q') ?? '';
    const categoria = this.lookupQueryParam(root, 'categoria');

    if (this.searchControl.value !== search) {
      this.searchControl.setValue(search, { emitEvent: false });
    }

    this.selectedCategoriaId = categoria ?? null;
  }

  private lookupQueryParam(route: ActivatedRouteSnapshot | null, key: string): string | null {
    if (!route) {
      return null;
    }

    if (route.queryParamMap.has(key)) {
      return route.queryParamMap.get(key);
    }

    for (const child of route.children) {
      const value = this.lookupQueryParam(child, key);
      if (value !== null) {
        return value;
      }
    }

    return null;
  }
}
