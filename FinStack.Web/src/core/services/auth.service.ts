import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthTokens, AuthTokenService } from './auth-token.service';
import { map, Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly baseUrl = 'http://localhost:5175';

  constructor(private http: HttpClient, private tokenService: AuthTokenService) { }

  login(email: string, password: string): Observable<void> {
    return this.http.post<AuthTokens>(`${this.baseUrl}/auth/login`, { email, password }).pipe(
      tap(tokens => { 
        this.tokenService.setTokens(tokens);
        console.log(tokens);
      }),
      map(() => void 0)
    );
  }

  refresh(): Observable<AuthTokens> {
    const refreshToken = this.tokenService.getRefreshToken();
    return this.http.post<AuthTokens>(`${this.baseUrl}/auth/refresh`, { refreshToken }).pipe(
      tap(tokens => this.tokenService.setTokens(tokens))
    );
  }

  logout(): void {
    this.tokenService.clear();
  }
}
