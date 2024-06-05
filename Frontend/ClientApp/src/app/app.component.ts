import { Component } from '@angular/core';
import { AuthService } from './security/auth-service.service';
import { Observable } from 'rxjs';
import { delay } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { BnNgIdleService } from 'bn-ng-idle';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = environment.PrjTitle;
  processing: boolean;
  Loading: Observable<boolean>;
  auth: any;
  constructor(auth: AuthService, private bnIdle: BnNgIdleService) {
    this.Loading = auth.isLoading;
    this.Loading.pipe(delay(0)).subscribe((loading: boolean) => {
      this.processing = loading;
    });
    this.auth = auth;
  }

  // initiate it in your component OnInit
  ngOnInit(): void {
    this.bnIdle.startWatching(3600).subscribe((isTimedOut: boolean) => {
      if (isTimedOut) {
        console.log('session expired');
        this.auth.logout();
      }
    });
  }

  ngOnDestroy(): void {

  }


   
}


