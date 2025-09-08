import { EnvironmentInjector, Injectable } from '@angular/core';
import { delay, Observable } from 'rxjs';
import { JobDTO } from '../models/dtos/job.dto';
import { HttpClient } from '@angular/common/http';
import { UserDTO } from 'core/models/dtos/user.dto';
import { asResult, Result } from 'core/lib/result';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  readonly API_BASE_URL = ' http://localhost:5175/api';

  constructor(private http: HttpClient, env: EnvironmentInjector) {
  }

  getJobs(): Observable<Result<JobDTO[], Error>> {
    return this.http.get<JobDTO[]>(`${this.API_BASE_URL}/v1/jobs`).pipe(asResult());
  }

  getUsers(): Observable<Result<UserDTO[], Error>> {
    return this.http.get<UserDTO[]>(`${this.API_BASE_URL}/v1/users`).pipe(
      delay(5000),
      asResult()
    );
  }
}
