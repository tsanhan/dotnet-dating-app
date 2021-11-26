import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
  //1. we can have multiple request in progress
  busyRequestCount = 0;

  constructor(private spinnerService: NgxSpinnerService) { }

  //1. create a method to show spinner
  busy() {
    this.busyRequestCount++;
    this.spinnerService.show(
      undefined, // no need fo a name
      {// there are about 50 spinner types
        type: 'line-scale-party',
        bdColor: 'rgba(255,255,255,0)',// background color, we don't want to dim our background
        color: '#333333'
      });

  }

  //2. create a method to hide spinner
  idle() {
    this.busyRequestCount--;
    if (this.busyRequestCount <= 0) { // just in case
      this.busyRequestCount = 0;
      this.spinnerService.hide();
    }
  }

  //3. so how do we engage the spinner service?
  // * we can use interceptors to govern requests go in and out
  //4. create an interceptor named 'loading', go to loading.interceptor.ts
}
