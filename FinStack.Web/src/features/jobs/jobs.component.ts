import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { JobsService } from '../../core/services/jobs.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-jobs',
  imports: [CommonModule],
  templateUrl: './jobs.component.html',
  styleUrl: './jobs.component.scss',
  standalone: true
})
export class JobsComponent {

  loading$: Observable<boolean>;

  constructor(private jobService: JobsService) {
    this.loading$ = jobService.loading$;
  }

  test() {
    this.jobService.load();
  }
}
