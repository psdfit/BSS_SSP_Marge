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
  constructor(private router: Router, private auth: AuthService) {}

  // intercept(
  //   req: HttpRequest<any>,
  //   next: HttpHandler
  // ): Observable<HttpEvent<any>> {
  //   req= req.clone({
  //     setHeaders: {
  //       "Content-Security-Policy":
  //       "default-src 'self'; script-src 'self'; object-src 'none';",
  //       "X-Frame-Options": "DENY",
  //       "X-Content-Type-Options": "nosniff",
  //     },
  //   });

  //   // return next.handle(secureReq);


  //   if (req.headers.get("No-Auth") == "True") {
  //   } else if (localStorage.getItem("IsAuthorized") == null) {
  //     this.router.navigateByUrl("/login");
  //   }
  //   this.lcount++;
  //   if (this.lcount > 0) {
  //     this.auth.setLoading(true);
  //   }
  //   return next.handle(req).pipe(
  //     tap(
  //       (event) => {
  //         if (event instanceof HttpResponse) {
  //           if (this.lcount == 0) {
  //             this.auth.setLoading(false);
  //           }
  //         }
  //       },
  //       (error) => {
  //         console.error("ERROR", error);
  //         if (error.status == 401) {
  //           this.router.navigateByUrl("/login");
  //         }
  //       }
  //     ),
  //     finalize(() => {
  //       this.lcount--;
  //       if (this.lcount == 0) {
  //         this.auth.setLoading(false);
  //       }
  //     })
  //   );
  // }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // debugger;
    // Clone the request to set security headers
    req = req.clone({
      setHeaders: {
        // Enable Content-Security-Policy allowing self and localhost
        // 'Content-Security-Policy': "default-src",
    
        // Deny embedding in frames or iframes
        // 'X-Frame-Options': 'DENY',
    
        // Prevent content sniffing (important for security)
        // 'X-Content-Type-Options': 'nosniff',
    
        // // Force HTTPS for the next 1 year and include subdomains
        // 'Strict-Transport-Security': 'max-age=31536000; includeSubDomains; preload',
    
        // // No caching for this request (useful for sensitive data)
        // 'Cache-Control': 'no-store, no-cache, must-revalidate',
    
        // Disallow referrer data from being sent
        // 'Referrer-Policy': 'no-referrer',
    
        // // Disable geolocation, microphone, and camera access
        // 'Permissions-Policy': "geolocation=(), microphone=(), camera=()",
    
        // // Block cross-site scripting attacks
        // 'X-XSS-Protection': '1;',
    
        // Disallow cross-domain Flash or other plugin usage
        // 'X-Permitted-Cross-Domain-Policies': 'none'
      },
    });
    

    // Check if the request has the No-Auth header
    if (req.headers.get('No-Auth') === 'True') {
      // No authentication required, continue handling the request
    } else if (localStorage.getItem('IsAuthorized') == null) {
      // If user is not authorized, redirect to the login page
      this.router.navigateByUrl('/login');
    }

    // Increment loading count to indicate loading state
    this.lcount++;
    if (this.lcount > 0) {
      this.auth.setLoading(true);
    }

    return next.handle(req).pipe(
      tap(
        (event) => {
          if (event instanceof HttpResponse) {
            // If response is successful, reset loading state
            if (this.lcount === 0) {
              this.auth.setLoading(false);
            }
          }
        },
        (error) => {
          // Handle errors, especially 401 (Unauthorized)
          console.error('ERROR', error);
          if (error.status === 401) {
            this.router.navigateByUrl('/login');
          }
        }
      ),
      finalize(() => {
        // Finalize the loading state after the response
        this.lcount--;
        if (this.lcount === 0) {
          this.auth.setLoading(false);
        }
      })
    );
  }
}


