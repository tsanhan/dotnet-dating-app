import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
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
    this.initializeForm();
  }
  initializeForm() {
    this.registerForm = new FormGroup({
      //1. add initial value and validation
      username: new FormControl(/*initial value */ "Hello", /*validations*/ Validators.required),
      //2. add multiple validations
      password: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
      //3. we woth this to be required AND the same as the password... we need a custom validator, we'll put required for now
      confirmPassword: new FormControl('', Validators.required),
      //4. for now lets see in the browser what impact it made on the form (the json is being updated as we type)
      //5. back to readme.md
    });
  }
  register() {
    // this.accountService.register(this.model).subscribe((re) => {
    //   console.log(re);
    //   this.cancel();
    // }, error => {
    //   console.log(error);
    //   this.toastr.error(error.error);
    // });

    console.log(this.registerForm.value);

  }

  cancel() {
    this.cancelRegister.emit(false);
  }



}
