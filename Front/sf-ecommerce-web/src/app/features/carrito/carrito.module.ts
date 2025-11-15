import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { SharedModule } from 'src/app/shared/shared.module';

import { CarritoRoutingModule } from './carrito-routing.module';
import { CarritoComponent } from './carrito.component';

@NgModule({
  declarations: [CarritoComponent],
  imports: [
    CommonModule,
    FormsModule,
    CarritoRoutingModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    MatInputModule,
    MatSnackBarModule,
    SharedModule,
    MatListModule,
    MatTooltipModule
  ]
})
export class CarritoModule {}
