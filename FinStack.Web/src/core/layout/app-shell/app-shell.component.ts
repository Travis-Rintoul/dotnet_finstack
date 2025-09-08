import { Component } from '@angular/core';
import { NavComponent } from '../nav/nav.component';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-app-shell',
  imports: [NavComponent, SidebarComponent, RouterOutlet],
  templateUrl: './app-shell.component.html',
  styleUrl: './app-shell.component.scss'
})
export class AppShellComponent {

}
