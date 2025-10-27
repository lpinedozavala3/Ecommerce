import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { BehaviorSubject, of } from 'rxjs';
import { ActivatedRoute, convertToParamMap } from '@angular/router';

import { CatalogoComponent } from './catalogo.component';
import { CatalogoService } from 'src/app/core/services/catalogo.service';
import { CarritoService } from 'src/app/core/services/carrito.service';
import { PagedResponse } from 'src/app/core/models/Paged';
import { Producto } from 'src/app/core/models/producto';

class CatalogoServiceStub {
  obtenerCategorias() {
    return of([]);
  }

  getDataByPage() {
    const emptyResponse: PagedResponse<Producto> = {
      status: 200,
      message: 'ok',
      data: [],
      pageNumber: 1,
      pageSize: 12,
      firstPage: '',
      lastPage: '',
      nextPage: null,
      previousPage: null,
      totalPages: 0,
      totalRecords: 0
    };

    return of(emptyResponse);
  }
}

class CarritoServiceStub {
  agregar() {}
}

describe('CatalogoComponent', () => {
  let component: CatalogoComponent;
  let fixture: ComponentFixture<CatalogoComponent>;
  const queryParams$ = new BehaviorSubject(convertToParamMap({}));

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CatalogoComponent],
      imports: [ReactiveFormsModule, RouterTestingModule],
      providers: [
        { provide: CatalogoService, useClass: CatalogoServiceStub },
        { provide: CarritoService, useClass: CarritoServiceStub },
        { provide: ActivatedRoute, useValue: { queryParamMap: queryParams$.asObservable() } }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CatalogoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
