import { HttpClient } from '@angular/common/http';

import { Injectable } from '@angular/core';

import { LoginModel } from '../models/loginModel';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  myAppUrl='https://localhost:44348/';
  myApiUrl='api/v1/Auth/';
  

  constructor(private http: HttpClient) { }

  Login(form:LoginModel){
    return this.http.post(this.myAppUrl+this.myApiUrl+'Login',form)
  }
}
