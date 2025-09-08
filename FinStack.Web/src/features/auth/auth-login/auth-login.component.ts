import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CardComponent } from '../../../shared/ui/card/card.component';
import { CardHeaderComponent } from '../../../shared/ui/card/card-header/card-header.component';
import { CardFooterComponent } from '../../../shared/ui/card/card-footer/card-footer.component';

@Component({
  selector: 'app-auth-login',
  imports: [RouterModule, CardComponent, CardHeaderComponent, CardFooterComponent],
  templateUrl: './auth-login.component.html',
  styleUrl: './auth-login.component.scss'
})
export class AuthLoginComponent {

}
