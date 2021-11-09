import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';

const routes: Routes = [
  {
    path:'',
    component: HomeComponent,
    pathMatch: 'full'
  }, {
    path:'members',
    component: MemberListComponent,
    canActivate: [AuthGuard] // protect this route
  },
  {
    path:'members/:id',
    component: MemberDetailComponent,
    canActivate: [AuthGuard] // protect this route
  },
  {
    path:'lists',
    component: ListsComponent,
    canActivate: [AuthGuard] // protect this route
  },
  {
    path:'messages',
    component: MessagesComponent,
    canActivate: [AuthGuard] // protect this route
  },
  {
    path:'**',
    component: HomeComponent,
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
