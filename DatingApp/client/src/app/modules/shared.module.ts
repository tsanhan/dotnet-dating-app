import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import {TabsModule } from 'ngx-bootstrap/tabs';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BsDropdownModule.forRoot(),
    ToastrModule.forRoot({
      positionClass:'toast-bottom-right'
    }),
    TabsModule.forRoot()//1. import that
  ],
  exports: [
    BsDropdownModule,
    ToastrModule,
    TabsModule//2. export that
  ]
})
export class SharedModule { }
