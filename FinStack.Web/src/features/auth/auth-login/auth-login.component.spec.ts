import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AuthLoginComponent } from './auth-login.component';
import { provideRouter } from '@angular/router';
import { of } from 'rxjs';
import { AuthService } from 'core/services/auth.service';

describe('AuthLoginComponent (shallow)', () => {
  let fixture: ComponentFixture<AuthLoginComponent>;

  const authMock = {
    login: jasmine.createSpy('login').and.returnValue(of({ ok: true })),
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AuthLoginComponent],
      providers: [
        provideRouter([]),
        { provide: AuthService, useValue: authMock },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(AuthLoginComponent);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(fixture.componentInstance).toBeTruthy();
  });
});
