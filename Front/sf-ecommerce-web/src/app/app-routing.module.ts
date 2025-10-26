import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', redirectTo: 'inicio', pathMatch: 'full' },  // 👈 redirección inicial
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
  { path: 'registro', redirectTo: 'auth/registro', pathMatch: 'full' },
  { path: '**', redirectTo: 'inicio' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { initialNavigation: 'enabled' })],
  exports: [RouterModule]
})
export class AppRoutingModule {}
