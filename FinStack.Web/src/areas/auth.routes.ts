import { Routes } from '@angular/router';
import { AuthShellComponent } from '../core/layout/auth-shell/auth-shell.component';
import { AuthLoginComponent } from '../features/auth/auth-login/auth-login.component';
import { AuthRegisterComponent } from '../features/auth/auth-register/auth-register.component';

export const AUTH_ROUTES: Routes = [
    {
        path: 'auth',
        component: AuthShellComponent,
        children: [
            {
                path: '',
                redirectTo: 'login',
                pathMatch: 'full'
            },
            {
                path: 'login',
                component: AuthLoginComponent
            },
            {
                path: 'register',
                component: AuthRegisterComponent
            }
        ],
    },
];
