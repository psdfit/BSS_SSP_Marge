import { HttpInterceptor, HttpRequest, HttpHandler, HttpUserEvent, HttpEvent, HttpResponse, HttpErrorResponse, HttpHeaders } from "@angular/common/http";
import { Observable, BehaviorSubject } from "rxjs";

import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { environment } from "../../environments/environment";
import { AuthService } from "./auth-service.service";
import { tap, finalize } from "rxjs/operators";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  lcount: number = 0;
  constructor(private router: Router,private auth:AuthService) {  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (req.headers.get('No-Auth') == "True") {
    }
    else if (localStorage.getItem("IsAuthorized") == null) {
      this.router.navigateByUrl('/login');
    }
        this.lcount++;
        if (this.lcount > 0) {
           this.auth.setLoading(true);
        }          
        return next.handle(req).pipe(
          tap(event => {
            if (event instanceof HttpResponse) {
              if (this.lcount == 0) {           
                this.auth.setLoading(false);
              }
             
            }
          }, error => {            
              console.error('ERROR', error)
              if (error.status == 401) {
                this.router.navigateByUrl('/login');
              }
          }), finalize(() => {
            this.lcount--;
            if (this.lcount == 0) {
              this.auth.setLoading(false);
            }
          }));
       
    }
}
