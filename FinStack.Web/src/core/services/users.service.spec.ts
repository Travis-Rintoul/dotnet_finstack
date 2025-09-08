import { TestBed } from '@angular/core/testing';
import { UsersService } from './users.service';
import { ApiService } from './api.service';
import { of } from 'rxjs';

describe('UsersService (mocked ApiService)', () => {
  let service: UsersService;

  const apiMock: Partial<ApiService> = {
    getUsers: jasmine.createSpy('getUsers').and.returnValue(of([])),
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        UsersService,
        { provide: ApiService, useValue: apiMock },
      ],
    });
    service = TestBed.inject(UsersService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
