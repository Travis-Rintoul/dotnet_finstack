import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CardComponent } from '../../../shared/ui/card/card.component';

@Component({
  selector: 'app-auth-register',
  imports: [RouterModule, CardComponent],
  templateUrl: './auth-register.component.html',
  styleUrl: './auth-register.component.scss'
})
export class AuthRegisterComponent {

}
