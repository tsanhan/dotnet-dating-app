import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';

const routes: Routes = [ // we'll use this array to provide routes for angular
  { // will be loaded on http://localhost:4200/ without any routing
    path:'',
    component: HomeComponent,
    pathMatch: 'full' // better read the inline doc on pathMatch (hover above it)
  }, {
    path:'members',
    component: MemberListComponent
  },
  {
    path:'members/:id', //:id is a parameter, for example 4 will look like: http://localhost:4200/members/4
    component: MemberDetailComponent
  },
  {
    path:'lists',
    component: ListsComponent
  },
  {
    path:'messages',
    component: MessagesComponent
  },
  {
    path:'**', //wildcard route (on error/non existing route)
    component: HomeComponent,
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
