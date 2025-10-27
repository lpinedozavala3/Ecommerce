import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

import { AuthResponse, UsuarioSesion } from '../models/auth';

const STORAGE_KEY = 'sf-auth-v1';

function getStorage(): Storage | null {
  if (typeof window === 'undefined') {
    return null;
  }
  try {
    return window.localStorage;
  } catch {
    return null;
  }
}

@Injectable({ providedIn: 'root' })
export class AuthStateService {
  private readonly _usuario$ = new BehaviorSubject<UsuarioSesion | null>(this.leer());
  readonly usuario$ = this._usuario$.asObservable();

  get usuario(): UsuarioSesion | null {
    return this._usuario$.value;
  }

  iniciarSesion(payload: AuthResponse, persistir: boolean): void {
    const data: UsuarioSesion = {
      id: payload.clienteId,
      email: payload.email,
      nombreCompleto: `${payload.nombre} ${payload.apellido}`.trim(),
      token: payload.token
    };

    this._usuario$.next(data);
    const storage = getStorage();
    if (persistir && storage) {
      storage.setItem(STORAGE_KEY, JSON.stringify(data));
    } else if (storage) {
      storage.removeItem(STORAGE_KEY);
    }
  }

  cerrarSesion(): void {
    this._usuario$.next(null);
    const storage = getStorage();
    storage?.removeItem(STORAGE_KEY);
  }

  private leer(): UsuarioSesion | null {
    try {
      const storage = getStorage();
      if (!storage) return null;
      const raw = storage.getItem(STORAGE_KEY);
      return raw ? JSON.parse(raw) : null;
    } catch {
      return null;
    }
  }
}
