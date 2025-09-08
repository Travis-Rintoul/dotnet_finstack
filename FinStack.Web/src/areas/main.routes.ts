import { Routes } from '@angular/router';
import { AppShellComponent } from '../core/layout/app-shell/app-shell.component';
import { DASHBOARD_ROUTES } from '../features/dashboard/dashboard.routes';
import { PortfolioComponent } from '../features/portfolio/portfolio.component';
import { InvestComponent } from '../features/invest/invest.component';
import { InsightsComponent } from '../features/insights/insights.component';
import { SettingsComponent } from '../features/settings/settings.component';
import { DashboardComponent } from '../features/dashboard/dashboard.component';
import { TransactionsComponent } from '../features/transactions/transactions.component';
import { BudgetsComponent } from '../features/budgets/budgets.component';
import { ReportsComponent } from '../features/reports/reports.component';
import { JobsComponent } from '../features/jobs/jobs.component';
import { UsersComponent } from 'features/users/users.component';

export const MAIN_ROUTES: Routes = [
    {
        path: '',
        component: AppShellComponent,
        children: [
            { 
                path: '', 
                component: DashboardComponent
            },
            { 
                path: 'transactions', 
                component: TransactionsComponent
            },
            { 
                path: 'budgets', 
                component: BudgetsComponent
            },
            {
                path: 'portfolio',
                component: PortfolioComponent,
            },
            {
                path: 'invest',
                component: InvestComponent,
            },
            {
                path: 'reports',
                component: ReportsComponent,
            },
            {
                path: 'insights',
                component: InsightsComponent,
            },
            {
                path: 'settings',
                component: SettingsComponent,
            },
            {
                path: 'jobs',
                component: JobsComponent
            },
            {
                path: 'users',
                component: UsersComponent
            }
        ],
    },
];
