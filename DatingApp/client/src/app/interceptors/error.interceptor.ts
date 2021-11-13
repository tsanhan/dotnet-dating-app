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

//0. it's acts like a service, it implements HttpInterceptor
@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(
    /*4. lets inject some things we need to handle our errors */
    private router: Router, // 5. sometime we want to redirect the user to an error page
    private toastr: ToastrService, //6. sometime we want to display a toaster notification
  ) { }

  /**
   * @param request 1. we can either intercept the `request` that gos out
   * @param next 2. or the response that comes back in the `next` after handleing it
   * @returns
   */
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request)//7. this is the default behavior of the interceptor
      .pipe(
        catchError(err => { //8. err will be the response that comes back from the server (if there is an error)
          switch (err.status) {
            case 400: //10. 2 types of 400: 1. bad request 2. validation error
              if (err.error.errors) {// 11. if there is a validation error
                //12. model state error - is how these errors known in ASP.NET = {modelName: {fieldName: 'field error message'}}
                const modelStateErrors = [];
                //13. we flattening the array of errors (class task: make this a online)
                for (const key in err.error.errors) {
                  if (err.error.errors[key]) {
                    modelStateErrors.push(err.error.errors[key]);
                  }
                }
                // 14. we throw the array of errors so the component can handle it visually (maybe show a list of them)
                throw modelStateErrors.flat(); // why 'flat' not exist? (https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/flat)
                // 14.1: solution: https://stackoverflow.com/questions/53556409/typescript-flatmap-flat-flatten-doesnt-exist-on-type-any
                // 14.2: go to tsconfig.json
              } else {
                //15. if there is no validation error
                // for some reason statusText is OK by default can read about it here:https://github.com/angular/angular/issues/23334
                this.toastr.error(err.statusText === "OK" ? "Bad Request" : err.statusText, err.status);

              }
              break;

            //16. if there is a 401 error
            case 401:
              // for some reason statusText is OK by default can read about it here:https://github.com/angular/angular/issues/23334
              this.toastr.error(err.statusText === "OK" ? "Unauthorised" : err.statusText, err.status);
              break;

            //17. if there is a 401 error
            case 404:
              this.router.navigateByUrl('/not-found');
              break;

            //18. if there is a 500 error,
            // we want to to still navigate to an error page but also get some details
            // we can add some state to the router (we'll see how we use it later)
            case 500:
              const navigationExtras: NavigationExtras = { state: { error: err.error } };
              this.router.navigateByUrl('/server-error',navigationExtras);
              break;

            //19. if there is some odd error
            default:
              this.toastr.error("Something unexpected went wrong")
              console.log(err);
              break;
            // 20. we need to provide interceptors in the app.module.ts go to app.module.ts
          }
          throw throwError(err)//9. if we wont to handle the error, we can throw it;
        })
      );
  }
}
