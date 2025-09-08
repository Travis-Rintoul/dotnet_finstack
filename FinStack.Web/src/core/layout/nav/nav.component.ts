import { Component } from '@angular/core';
import { AuthService } from 'core/services/auth.service';

@Component({
  selector: 'app-nav',
  imports: [],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.scss'
})
export class NavComponent {

  constructor(private authService: AuthService) {}

  login() {
    this.authService.login('test@gmail.com', 'Abc123!').subscribe(x => {
      console.log(x);
    });
  }
}
