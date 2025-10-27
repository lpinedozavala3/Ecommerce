import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from 'src/environments/environment';
import { AuthResponse, LoginPayload, RegistroPayload } from '../models/auth';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly baseUrl = environment.backend_server + '/auth';

  constructor(private http: HttpClient) {}

  login(payload: LoginPayload): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseUrl}/login`, payload);
  }

  registro(payload: RegistroPayload): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseUrl}/register`, payload);
  }
}
