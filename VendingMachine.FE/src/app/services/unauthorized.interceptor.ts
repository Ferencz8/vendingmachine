import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpResponse, HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { catchError, Observable, of, tap, throwError } from "rxjs";

@Injectable()
export class UnauthorizedInterceptor implements HttpInterceptor {
    constructor(public router: Router) {}
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      
      return next.handle(req).pipe(
        catchError((error) => {
        if (error instanceof HttpErrorResponse) {
          if (error.status === 403) {
            this.router.navigate([`/login`]);
          }
        }
        return throwError(()=>error);
      }));
    }
}