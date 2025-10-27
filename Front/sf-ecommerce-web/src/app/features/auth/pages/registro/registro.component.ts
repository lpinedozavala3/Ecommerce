import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

import { AuthService } from 'src/app/core/services/auth.service';
import { AuthStateService } from 'src/app/core/services/auth-state.service';

@Component({
  selector: 'app-registro',
  templateUrl: './registro.component.html',
  styleUrls: ['./registro.component.scss']
})
export class RegistroComponent {
  loading = false;

  formulario = this.fb.group({
    nombre: ['', [Validators.required, Validators.maxLength(120)]],
    apellido: ['', [Validators.required, Validators.maxLength(120)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
    telefono: ['']
  });

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private authState: AuthStateService,
    private snack: MatSnackBar,
    private router: Router
  ) {}

  submit(): void {
    if (this.formulario.invalid || this.loading) {
      this.formulario.markAllAsTouched();
      return;
    }

    this.loading = true;
    const payload = {
      nombre: this.nombre?.value ?? '',
      apellido: this.apellido?.value ?? '',
      email: this.email?.value ?? '',
      password: this.password?.value ?? '',
      telefono: this.formulario.value.telefono || undefined
    };

    this.auth.registro(payload).subscribe({
      next: resp => {
        this.loading = false;
        this.authState.iniciarSesion(resp, true);
        this.snack.open('¡Registro exitoso! Ya puedes comprar.', undefined, { duration: 2500 });
        this.router.navigate(['/inicio']);
      },
      error: err => {
        this.loading = false;
        const message = err?.error?.message || 'No pudimos crear tu cuenta. Intenta nuevamente.';
        this.snack.open(message, 'Cerrar', { duration: 3500 });
      }
    });
  }

  get nombre() { return this.formulario.get('nombre'); }
  get apellido() { return this.formulario.get('apellido'); }
  get email() { return this.formulario.get('email'); }
  get password() { return this.formulario.get('password'); }
}
