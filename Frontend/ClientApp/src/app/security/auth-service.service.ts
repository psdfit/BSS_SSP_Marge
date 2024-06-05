import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Router } from '@angular/router';
import { UserRightsModel, UsersModel } from '../master-data/users/users.component';
import { HttpParams } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private loggedIn = new BehaviorSubject<boolean>(false); // {1}
  private Loading = new BehaviorSubject<boolean>(false);
  get isLoggedIn() {
    return this.loggedIn.asObservable(); // {2}
  }
  get isLoading() {
    return this.Loading.asObservable(); // {2}
  }
  redirectUrl: string = "";
  constructor(private route: Router) {
    if (sessionStorage.getItem(environment.AuthToken) != null) {
      this.loggedIn.next(true);
    }
    else if (localStorage.getItem('RememberMe') != null) {
      this.setCredntial(JSON.parse(localStorage.getItem('RememberMe')), true);
    }
  }
  setLoading(v: boolean) {
    this.Loading.next(v); // {2}
  }
  setCredntial(cred: any, rememberme: boolean) {
    let u: UsersModel = JSON.parse(atob(cred[0]));
    let UserImg = u.UserImage;
    u.UserImage = "";
    sessionStorage.setItem(environment.AuthToken, btoa(JSON.stringify(u)));
    sessionStorage.setItem(environment.RightsToken, cred[1]);
    sessionStorage.setItem("UserOrgs", cred[2]);
    sessionStorage.setItem("UserImage", UserImg);
    localStorage.setItem("IsAuthorized", "1");
    sessionStorage.setItem("SessionID", u.SessionID);
    sessionStorage.setItem("UserID", u.UserID.toString());
    if (rememberme) {
      localStorage.setItem('RememberMe', JSON.stringify(cred));
    }
    this.loggedIn.next(true);
  }
  logout() {
    sessionStorage.removeItem(environment.AuthToken);
    sessionStorage.removeItem(environment.RightsToken);
    sessionStorage.removeItem("UserImage");
    localStorage.removeItem('RememberMe');
    this.loggedIn.next(false);
    this.route.navigate(["/login"]);
  }
}
