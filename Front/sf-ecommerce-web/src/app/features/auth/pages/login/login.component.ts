import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

import { AuthService } from 'src/app/core/services/auth.service';
import { AuthStateService } from 'src/app/core/services/auth-state.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loading = false;

  formulario = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
    recordar: [true]
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
    const { recordar, email, password } = this.formulario.value;
    const payload = { email: email ?? '', password: password ?? '' };

    this.auth.login(payload).subscribe({
      next: resp => {
        this.loading = false;
        this.authState.iniciarSesion(resp, !!recordar);
        this.snack.open('¡Bienvenido nuevamente!', undefined, { duration: 2000 });
        this.router.navigate(['/inicio']);
      },
      error: err => {
        this.loading = false;
        const message = err?.error?.message || 'No pudimos iniciar sesión. Verifica tus datos.';
        this.snack.open(message, 'Cerrar', { duration: 3000 });
      }
    });
  }

  get email() { return this.formulario.get('email'); }
  get password() { return this.formulario.get('password'); }
}
