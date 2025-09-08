import { Injectable } from '@angular/core';
import { Stash } from 'core/lib/stash';
import { UserDTO } from 'core/models/dtos/user.dto';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
    private readonly stash = new Stash<UserDTO>();
  
    readonly users$ = this.stash.value$;
    readonly saved$ = this.stash.saved$;
    readonly saving$ = this.stash.saving$;
    readonly loaded$ = this.stash.loaded$;
    readonly loading$ = this.stash.loading$;
  
    constructor(private apiService: ApiService) {}
  
    load(): void {
      this.stash.load(this.apiService.getUsers());
    }
}
