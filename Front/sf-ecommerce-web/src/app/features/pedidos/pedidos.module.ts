import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDialogModule } from '@angular/material/dialog';
import { SharedModule } from 'src/app/shared/shared.module';

import { PedidosRoutingModule } from './pedidos-routing.module';
import { PedidosComponent } from './pedidos.component';
import { ExitoComponent } from './pages/exito/exito.component';


@NgModule({
  declarations: [
    PedidosComponent,
    ExitoComponent
  ],
  imports: [
    CommonModule,
    PedidosRoutingModule,
    ReactiveFormsModule,
    MatTabsModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatDialogModule,
    SharedModule
  ]
})
export class PedidosModule { }
