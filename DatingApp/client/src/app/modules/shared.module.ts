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
import { ModalModule } from 'ngx-bootstrap/modal';

@NgModule({
  declarations: [
    MemberCardComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    ModalModule.forRoot(),//1. add this
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
    ModalModule,//2. add this
    MemberCardComponent,
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
    //3. go back to README.md
  ]
})
export class SharedModule { }
