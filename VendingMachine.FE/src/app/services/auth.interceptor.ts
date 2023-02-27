import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

    intercept(req: HttpRequest<any>,
              next: HttpHandler): Observable<HttpEvent<any>> {

        const idToken = localStorage.getItem("id_token");

        if (idToken) {

            req = req.clone({
                setHeaders: {
                  Authorization: `Bearer ${idToken}`
                }
              });
            return next.handle(req);
        }
        else {
            return next.handle(req);
        }
    }
}