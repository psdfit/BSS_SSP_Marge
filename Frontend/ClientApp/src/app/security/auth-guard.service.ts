import { Injectable } from '@angular/core';
import {
  CanActivate, Router,
  ActivatedRouteSnapshot,
  RouterStateSnapshot, CanActivateChild, CanLoad, Route
} from '@angular/router';
import { AuthService } from './auth-service.service';
import { environment } from '../../environments/environment';
import { CommonSrvService } from '../common-srv.service';
//import { Router } from '@angular/router';
@Injectable()
export class AuthGuardService implements CanActivate, CanActivateChild, CanLoad {
  constructor(private authService: AuthService, private router: Router, private commonService: CommonSrvService) { }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    let url: string = state.url;
    ///authorization
    //if (!this.isAuthorized(url)) {
    //  this.router.navigate(['/error/accessdenied']);
    //}
    ///authorization
    if (!this.isAuthorized(url)) {
      this.router.navigate(['/error/accessdenied']);
    }
    console.log('Url-1:' + url);
    return this.checkLogin(url);
  }
  canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    return this.canActivate(route, state);
  }
  canLoad(route: Route): boolean {
    let url: string = route.path;
    console.log('Url:' + url);
    //if (this.checkLogin(url)) {
    //  return true;
    //}
    return true;
  }
  checkLogin(url: string): boolean {
    if (sessionStorage.getItem(environment.AuthToken) != null && sessionStorage.getItem(environment.AuthToken).length > 0) { return true; }
    // Store the attempted URL for redirecting
    this.authService.redirectUrl = url;
    // Navigate to the login page with extras
    this.router.navigate(['/login']);
    return false;
  }
  isAuthorized(path: string) {
    if (path == '/home') {
      return true;
    }
    let splittedPath = path.split("/");
    let rights = JSON.parse(sessionStorage.getItem(environment.RightsToken)).filter(x => x.CanView == 1)
    return rights.find(x => x.modpath == splittedPath[1] && x.Path == splittedPath[2].split("?")[0]) != undefined;
  }
}
