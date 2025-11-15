import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InicioRoutingModule } from './inicio-routing.module';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatChipsModule } from '@angular/material/chips';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';
import { InicioComponent } from './pages/inicio/inicio.component';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTooltipModule } from '@angular/material/tooltip';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    InicioComponent
  ],
  imports: [
    CommonModule,
    InicioRoutingModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,        // ← necesario para <mat-icon>
    MatChipsModule,       // ← necesario para <mat-chip-list> y <mat-chip>
    MatCardModule,
    MatButtonModule,
    MatPaginatorModule,
    MatTooltipModule,
    SharedModule
  ]
})
export class InicioModule { }
