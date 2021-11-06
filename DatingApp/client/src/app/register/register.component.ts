import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  //1. cleanup the code, we don't need to get the users from the server.
  //cleanup here, in the html, in home.component.html and home.component.ts:
  @Output() cancelRegister = new EventEmitter<boolean>();

  model: any = {};

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }

  register() {
    //2. register the user
    this.accountService.register(this.model).subscribe((re) => {
      console.log(re);
      this.cancel();// this is temporary, we will use the router to navigate to another page
    }, error => {
      console.log(error);
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
  }



}
