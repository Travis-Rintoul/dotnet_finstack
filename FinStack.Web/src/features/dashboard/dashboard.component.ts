import { Component } from '@angular/core';
import { CardComponent } from '../../shared/ui/card/card.component';
import { DashboardUserDetailsComponent } from './dashboard-user-details/dashboard-user-details.component';
import { DashboardBuyAssetComponent } from './dashboard-buy-asset/dashboard-buy-asset.component';
import { CardHeaderComponent } from '../../shared/ui/card/card-header/card-header.component';
import { DashboardHoldingsComponent } from './dashboard-holdings/dashboard-holdings.component';

@Component({
  selector: 'app-dashboard',
  imports: [
    CardComponent,
    CardHeaderComponent,
    DashboardUserDetailsComponent,
    DashboardBuyAssetComponent,
    DashboardHoldingsComponent
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
  standalone: true
})
export class DashboardComponent {
  margin_bottom = 'mb-3'
}
