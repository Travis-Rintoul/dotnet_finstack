import { Injectable } from '@angular/core';
import { Stash } from '../lib/stash';
import { JobDTO } from '../models/dtos/job.dto';
import { delay, Observable, Subject } from 'rxjs';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class JobsService {
  private readonly stash = new Stash<JobDTO>();

  readonly jobs$ = this.stash.value$;
  readonly saved$ = this.stash.saved$;
  readonly saving$ = this.stash.saving$;
  readonly loaded$ = this.stash.loaded$;
  readonly loading$ = this.stash.loading$;

  constructor(private apiService: ApiService) {}

  load(): void {
    this.stash.load(this.apiService.getJobs());
  }
}
