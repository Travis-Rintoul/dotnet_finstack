import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss',
  standalone: true
})
export class SidebarComponent {
  public menuItems = [
    { title: 'Dashboard', route: '/' },
    { title: 'Transactions', route: '/transactions' },
    { title: 'Budgets', route: '/budgets' },
    { title: 'Portfolio', route: '/portfolio' },
    { title: 'Invest', route: '/invest' },
    { title: 'Reports', route: '/reports' },
    { title: 'AI Insights', route: '/insights' },
    { title: 'Data Feeds', route: '/feeds' },
    { title: 'Jobs', route: '/jobs' },
    { title: 'Users', route: '/users' },

    { title: 'Settings', route: '/settings' },
  ];
}
