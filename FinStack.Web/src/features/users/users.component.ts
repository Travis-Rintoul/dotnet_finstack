import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { UserDTO } from 'core/models/dtos/user.dto';
import { UsersService } from 'core/services/users.service';
import { Observable } from 'rxjs';
import { CardHeaderComponent } from 'shared/ui/card/card-header/card-header.component';
import { CardComponent } from 'shared/ui/card/card.component';
import { LoadingComponent } from 'shared/ui/loading/loading.component';

@Component({
  selector: 'app-users',
  imports: [CommonModule, CardComponent, CardHeaderComponent, LoadingComponent],
  templateUrl: './users.component.html',
  styleUrl: './users.component.scss',
  standalone: true
})
export class UsersComponent {

  users$: Observable<UserDTO[]>;
  loading$: Observable<boolean>;

  constructor(private userService: UsersService) {
    this.loading$ = userService.loading$;
    this.users$ = userService.users$;
  }

  test() {
    this.userService.load();
  }
}
