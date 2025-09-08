import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardHoldingsComponent } from './dashboard-holdings.component';

describe('DashboardHoldingsComponent', () => {
  let component: DashboardHoldingsComponent;
  let fixture: ComponentFixture<DashboardHoldingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardHoldingsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardHoldingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
