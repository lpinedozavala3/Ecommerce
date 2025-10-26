import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', redirectTo: 'inicio', pathMatch: 'full' },  // 👈 redirección inicial
  { path: 'inicio', loadChildren: () => import('./features/inicio/inicio.module').then(m => m.InicioModule) },
  { path: 'catalogo', loadChildren: () => import('./features/catalogo/catalogo.module').then(m => m.CatalogoModule) },
  { path: 'carrito', loadChildren: () => import('./features/carrito/carrito.module').then(m => m.CarritoModule) },
  { path: '**', redirectTo: 'inicio' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { initialNavigation: 'enabled' })],
  exports: [RouterModule]
})
export class AppRoutingModule {}
