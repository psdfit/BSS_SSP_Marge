import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AppConfigService {
  appConfig;
  constructor(private http: HttpClient) { }

  loadAppConfig() {
    return this.http.get('/assets/appConfig.json')
      .toPromise()
      .then(data => {
        this.appConfig = data;
      })
  }
  //Getter
  getAppConfig() {
    return this.appConfig
  }
}
