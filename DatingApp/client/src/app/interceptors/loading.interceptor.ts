import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { BusyService } from '../services/busy.service';
import { delay, finalize } from 'rxjs/operators';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
  //1. inject BusyService
  constructor(private busyService: BusyService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    //2. show spinner
    this.busyService.busy();
    //3. return the request with some delay and then hide the spinner
    return next.handle(request).pipe(
      delay(1000),// delay for 1 second
      finalize(() => this.busyService.idle()) // when the request is done, we can hide the spinner
    );
    //4. this is an interceptor, so we need to add the interceptor to the providers in app.module.ts, go there.
  }
}
