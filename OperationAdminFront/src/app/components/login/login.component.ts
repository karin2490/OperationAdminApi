import { Component, OnInit } from '@angular/core';
import { FormGroup,FormControl,Validators, Form } from '@angular/forms';
import { LoginModel } from 'src/app/models/loginModel';
import {LoginService} from '../../services/login.service';
import {Router} from '@angular/router'

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm= new FormGroup({
    Email:new FormControl('',Validators.required),
    Pin:new FormControl('',Validators.required)
  })

  constructor( private loginService:LoginService, private router:Router) { }

  ngOnInit(): void {
  }

  onLogin(form:LoginModel){
    console.log(form);

    this.loginService.Login(form).subscribe((response:any)=>{
      console.log(response);
      let dataResponse=response.data;
    });
  }

}
