import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  myAppUrl='https://localhost:44347/';
  myApiUrl='api/v1/UsersController'
  constructor(private http: HttpClient) { }

    GetUsers(){
      return this.http.get(this.myAppUrl+this.myApiUrl+'/get-all');
    }



  
}
