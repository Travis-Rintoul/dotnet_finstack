import { inject, Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthTokenService } from 'core/services/auth-token.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
constructor(private tokenSvc: AuthTokenService) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const token = this.tokenSvc.getAccessToken();
        if (!token || req.url.includes('/auth/')) {
            return next.handle(req);
        }

        const authReq = req.clone({
            setHeaders: { Authorization: `Bearer ${token}` }
        });

        return next.handle(authReq);
    }
}