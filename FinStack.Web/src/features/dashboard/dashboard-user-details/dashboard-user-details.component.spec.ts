import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardUserDetailsComponent } from './dashboard-user-details.component';

describe('DashboardUserDetailsComponent', () => {
  let component: DashboardUserDetailsComponent;
  let fixture: ComponentFixture<DashboardUserDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardUserDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardUserDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
