import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

export interface AuthTokens {
  accessToken: string;
  refreshToken?: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthTokenService {
  
  private accessToken: string | null = null;

  private readonly _isAuthenticated$ = new BehaviorSubject<boolean>(false);
  readonly isAuthenticated$: Observable<boolean> = this._isAuthenticated$.asObservable();

  setTokens(tokens: AuthTokens) {
    this.accessToken = tokens.accessToken;
    if (tokens.refreshToken) {
      sessionStorage.setItem('refresh_token', tokens.refreshToken);
    }
    this._isAuthenticated$.next(true);
  }

  getAccessToken(): string | null {
    return this.accessToken;
  }

  getRefreshToken(): string | null {
    return sessionStorage.getItem('refresh_token');
  }

  clear() {
    this.accessToken = null;
    sessionStorage.removeItem('refresh_token');
    this._isAuthenticated$.next(false);
  }
}
