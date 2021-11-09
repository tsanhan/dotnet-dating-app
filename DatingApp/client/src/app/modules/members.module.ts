import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { MemberListComponent } from '../members/member-list/member-list.component';
import { MemberDetailComponent } from '../members/member-detail/member-detail.component';

const routes: Routes = [
  {path:'',component: MemberListComponent, pathMatch:'full'},
  {path:':id',component: MemberDetailComponent},
];

@NgModule({
  declarations: [
    // taken from app.module.ts, remove it from there and remove imports
    MemberListComponent,
    MemberDetailComponent,
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes) // these are child routes (not starting from the root path)
  ],
  exports:[
    RouterModule,
    MemberListComponent,
    MemberDetailComponent]
})
export class MembersModule { }
