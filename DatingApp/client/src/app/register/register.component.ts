import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
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
  //1. add this
  registerForm: FormGroup;

  constructor(
    private accountService: AccountService,
    private toastr: ToastrService ) { }

  ngOnInit(): void {
    // 2. add this
    this.initializeForm();
  }
  // 2. add this methos
  initializeForm() {
    // form group contains form controls
    this.registerForm = new FormGroup({
      username: new FormControl(),
      password: new FormControl(),
      confirmPassword: new FormControl(),
    });
  }
  register() {
    //3. comment this for now
    // this.accountService.register(this.model).subscribe((re) => {
    //   console.log(re);
    //   this.cancel();
    // }, error => {
    //   console.log(error);
    //   this.toastr.error(error.error);
    // });

    //4. and log the form values
    console.log(this.registerForm.value);
    //5. go to the html

  }

  cancel() {
    this.cancelRegister.emit(false);
  }



}
