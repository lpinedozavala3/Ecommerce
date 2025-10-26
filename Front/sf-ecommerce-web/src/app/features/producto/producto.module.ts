import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { ProductoRoutingModule } from './producto-routing.module';
import { ProductoDetalleComponent } from './pages/detalle/producto-detalle.component';

@NgModule({
  declarations: [ProductoDetalleComponent],
  imports: [
    CommonModule,
    ProductoRoutingModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule
  ]
})
export class ProductoModule {}
