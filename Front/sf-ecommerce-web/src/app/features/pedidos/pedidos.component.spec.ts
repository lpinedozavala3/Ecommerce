import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { of } from 'rxjs';

import { AuthStateService } from 'src/app/core/services/auth-state.service';
import { PedidosService } from 'src/app/core/services/pedidos.service';
import { PedidosComponent } from './pedidos.component';

describe('PedidosComponent', () => {
  let component: PedidosComponent;
  let fixture: ComponentFixture<PedidosComponent>;

  beforeEach(async () => {
    const pedidosServiceMock = {
      obtenerPedidos: jasmine.createSpy('obtenerPedidos').and.returnValue(of([])),
      obtenerDireccion: jasmine.createSpy('obtenerDireccion').and.returnValue(of(null)),
      guardarDireccion: jasmine.createSpy('guardarDireccion').and.returnValue(of(null as any))
    };

    const authStateMock = {
      usuario$: of(null),
      usuario: null
    } as Partial<AuthStateService>;

    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule],
      declarations: [PedidosComponent],
      providers: [
        { provide: PedidosService, useValue: pedidosServiceMock },
        { provide: AuthStateService, useValue: authStateMock }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PedidosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
