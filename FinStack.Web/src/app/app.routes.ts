import { Routes } from '@angular/router';
import { MAIN_ROUTES } from '../areas/main.routes';
import { AUTH_ROUTES } from '../areas/auth.routes';

export const routes: Routes = [
    ...MAIN_ROUTES,
    ...AUTH_ROUTES,
];
