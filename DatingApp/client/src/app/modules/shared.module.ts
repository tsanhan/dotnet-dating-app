import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import {TabsModule } from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { NgxSpinnerModule } from 'ngx-spinner';
import { FileUploadModule } from 'ng2-file-upload';
import { BsDatepickerModule } from "ngx-bootstrap/datepicker";
import { PaginationModule } from "ngx-bootstrap/pagination";
import { ButtonsModule } from "ngx-bootstrap/buttons";
import { FormsModule } from '@angular/forms';
import { TimeagoModule } from 'ngx-timeago';
import { MemberCardComponent } from '../members/member-card/member-card.component';
import { RouterModule } from '@angular/router';
@NgModule({
  declarations: [
    MemberCardComponent// 1. add this
  ],
  imports: [
    CommonModule,
    RouterModule,// 2. add this because we use routerLink in member-card.component.html
    BsDropdownModule.forRoot(),
    ToastrModule.forRoot({
      positionClass:'toast-bottom-right'
    }),
    TabsModule.forRoot(),
    NgxGalleryModule,
    NgxSpinnerModule,
    FileUploadModule,
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot(),
    ButtonsModule.forRoot(),
    TimeagoModule.forRoot()
  ],
  exports: [
    MemberCardComponent,//3. add this so this will be available in other modules
    BsDropdownModule,
    BsDatepickerModule,
    ToastrModule,
    TabsModule,
    NgxGalleryModule,
    NgxSpinnerModule,
    FileUploadModule,
    FormsModule,
    PaginationModule ,
    ButtonsModule,
    TimeagoModule
    //3. go back to member-detail.component.html

  ]
})
export class SharedModule { }
