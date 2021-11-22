import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { MemberListComponent } from '../members/member-list/member-list.component';
import { MemberDetailComponent } from '../members/member-detail/member-detail.component';
import { MemberCardComponent } from '../members/member-card/member-card.component';

const routes: Routes = [
  {path:'',component: MemberListComponent, pathMatch:'full'},
  {path:':username',component: MemberDetailComponent}, //1. change this
];

@NgModule({
  declarations: [
    MemberListComponent,
    MemberDetailComponent,
    MemberCardComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ],
  exports:[
    RouterModule,
    MemberListComponent,
    MemberDetailComponent]
})
export class MembersModule { }
