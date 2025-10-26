import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';

import { CarritoService } from 'src/app/core/services/carrito.service';
import { AuthStateService } from 'src/app/core/services/auth-state.service';
import { UsuarioSesion } from 'src/app/core/models/auth';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {
  itemsEnCarrito = 0;
  usuario: UsuarioSesion | null = null;

  private subs: Subscription[] = [];

  constructor(
    private carrito: CarritoService,
    private authState: AuthStateService
  ) {}

  ngOnInit(): void {
    this.subs.push(
      this.carrito.items$.subscribe(items => (this.itemsEnCarrito = items.reduce((sum, item) => sum + item.cantidad, 0)))
    );

    this.subs.push(
      this.authState.usuario$.subscribe(usuario => (this.usuario = usuario))
    );
  }

  ngOnDestroy(): void {
    this.subs.forEach(s => s.unsubscribe());
  }

  cerrarSesion(): void {
    this.authState.cerrarSesion();
  }
}
