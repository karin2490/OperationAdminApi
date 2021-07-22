import { HttpClient } from '@angular/common/http';

import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  myAppUrl='https://localhost:44347/';
  myApiUrl='api/v1/AuthController'
  

  constructor(private http: HttpClient) { }

  // Login(){
  //   return this.http.post(this.myAppUrl+this.myApiUrl+'/Login',)
  // }
}
