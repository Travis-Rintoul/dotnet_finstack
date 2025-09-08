import { TestBed } from '@angular/core/testing';
import { JobsService } from './jobs.service';
import { ApiService } from './api.service';
import { of } from 'rxjs';

describe('JobsService (with mocked ApiService)', () => {
  let service: JobsService;

  const apiMock = {
    getJobs: jasmine.createSpy('getJobs').and.returnValue(of([])),
  } as Partial<ApiService>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        JobsService,
        { provide: ApiService, useValue: apiMock },
      ],
    });
    service = TestBed.inject(JobsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
