import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthRegisterComponent } from './auth-register.component';
import { provideRouter } from '@angular/router';

describe('AuthRegisterComponent', () => {
  let component: AuthRegisterComponent;
  let fixture: ComponentFixture<AuthRegisterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AuthRegisterComponent],
      providers: [provideRouter([])]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AuthRegisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
