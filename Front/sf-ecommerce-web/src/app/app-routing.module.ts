import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreGuard } from './core/guards/store.guard';

const routes: Routes = [
  { path: 'pagina-no-encontrada', loadChildren: () => import('./features/not-found/not-found.module').then(m => m.NotFoundModule) },
  { path: '', pathMatch: 'full', redirectTo: 'pagina-no-encontrada' },
  {
    path: ':store',
    canActivate: [StoreGuard],
    canActivateChild: [StoreGuard],
    children: [
      { path: '', redirectTo: 'inicio', pathMatch: 'full' },
      { path: 'inicio', loadChildren: () => import('./features/inicio/inicio.module').then(m => m.InicioModule) },
      { path: 'catalogo', loadChildren: () => import('./features/catalogo/catalogo.module').then(m => m.CatalogoModule) },
      { path: 'carrito', loadChildren: () => import('./features/carrito/carrito.module').then(m => m.CarritoModule) },
      { path: 'producto', loadChildren: () => import('./features/producto/producto.module').then(m => m.ProductoModule) },
      { path: 'checkout', loadChildren: () => import('./features/checkout/checkout.module').then(m => m.CheckoutModule) },
      { path: 'pedidos', loadChildren: () => import('./features/pedidos/pedidos.module').then(m => m.PedidosModule) },
      { path: 'novedades', loadChildren: () => import('./features/novedades/novedades.module').then(m => m.NovedadesModule) },
      { path: 'contacto', loadChildren: () => import('./features/contacto/contacto.module').then(m => m.ContactoModule) },
      { path: 'auth', loadChildren: () => import('./features/auth/auth.module').then(m => m.AuthModule) },
      { path: 'login', redirectTo: 'auth/login', pathMatch: 'full' },
      { path: 'registro', redirectTo: 'auth/registro', pathMatch: 'full' }
    ]
  },
  { path: '**', redirectTo: 'pagina-no-encontrada' }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      initialNavigation: 'enabledNonBlocking'
    })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {}
