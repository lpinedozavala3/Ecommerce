import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatSelectModule } from '@angular/material/select';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTooltipModule } from '@angular/material/tooltip';
import { SharedModule } from 'src/app/shared/shared.module';

import { CatalogoRoutingModule } from './catalogo-routing.module';
import { CatalogoComponent } from './catalogo.component';
import { ListadoComponent } from './pages/listado/listado.component';
import { DetalleComponent } from './pages/detalle/detalle.component';


@NgModule({
  declarations: [
    CatalogoComponent,
    ListadoComponent,
    DetalleComponent
  ],
  imports: [
    CommonModule,
    CatalogoRoutingModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatSelectModule,
    SharedModule,
    MatPaginatorModule,
    MatTooltipModule
  ]
})
export class CatalogoModule { }
