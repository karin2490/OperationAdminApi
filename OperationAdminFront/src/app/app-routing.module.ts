import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {LoginComponent} from './components/login/login.component';
import {UsersComponent} from './components/users/users.component';
import {AccountComponent} from './components/account/account.component';
import {TeamsComponent} from './components/teams/teams.component';
import {TeamsLogComponent} from './components/teams-log/teams-log.component';

const routes: Routes = [
  {path:'',redirectTo:'login',pathMatch:'full'},
  {path:'login',component:LoginComponent},
  {path:'users',component:UsersComponent},
  {path:'account',component:AccountComponent},
  {path:'teams',component:TeamsComponent},
  {path:'teams-log',component:TeamsLogComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
export const routingComponents=[LoginComponent,UsersComponent,AccountComponent,TeamsComponent,TeamsLogComponent]
