import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardBuyAssetComponent } from './dashboard-buy-asset.component';

describe('DashboardBuyAssetComponent', () => {
  let component: DashboardBuyAssetComponent;
  let fixture: ComponentFixture<DashboardBuyAssetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardBuyAssetComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardBuyAssetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
