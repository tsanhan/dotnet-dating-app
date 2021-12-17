import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(
    private router: Router,
    private toastr: ToastrService,
  ) { }

  /**
   * @param request
   * @param next
   * @returns
   */
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request)
      .pipe(
        catchError(err => {
          switch (err.status) {
            case 400:
              if (err.error.errors) {
                const modelStateErrors = [];
                for (const key in err.error.errors) {
                  if (err.error.errors[key]) {
                    modelStateErrors.push(err.error.errors[key]);
                  }
                }
                throw modelStateErrors.flat();
              } else /*3. check if this is an object, so we'll print a generic response */ if(typeof err.error === 'object') {
                //2. we'll keep this only if the error object is an object
                this.toastr.error(err.statusText === "OK" ? "Bad Request" : err.statusText, err.status);
                throw err;//1. add this and back to README.md
              } else { //4. because in other case it's a string, we'll print it to toastr
                this.toastr.error(err.error, err.status);
                throw err;
                //5. back to README.md
              }
              break;


            case 401:

              this.toastr.error(err.statusText === "OK" ? "Unauthorised" : err.statusText, err.status);
              break;


            case 404:
              this.router.navigateByUrl('/not-found');
              break;


            case 500:
              const navigationExtras: NavigationExtras = { state: { error: err.error } };
              this.router.navigateByUrl('/server-error',navigationExtras);
              break;


            default:
              this.toastr.error("Something unexpected went wrong")
              console.log(err);
              break;

          }
          throw throwError(err)
        })
      );
  }
}
