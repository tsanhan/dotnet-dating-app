import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter<boolean>();

  model: any = {};

  constructor(
    private accountService: AccountService,
    /*1. inject the router*/ private toastr: ToastrService ) { }

  ngOnInit(): void {
  }

  register() {
    this.accountService.register(this.model).subscribe((re) => {
      console.log(re);
      this.cancel();
    }, error => {
      console.log(error);
      // 2. use the toastr
      this.toastr.error(error.error);
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
  }



}
