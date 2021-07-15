import { Component, OnInit } from '@angular/core';
import { UsersService } from 'src/app/services/users.service';

declare var $: any;

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {

  constructor(
    public userService:UsersService
    ) { }


    listaUsuarios=[];

  ngOnInit() {}

  ObtieneUsuarios(){
    this.userService.GetUsers().subscribe((response:any)=>{
      this.listaUsuarios=response.result;
    })
  }

}
