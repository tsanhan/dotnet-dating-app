import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validator, ValidatorFn, Validators } from '@angular/forms';
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
    private toastr: ToastrService,
    private fb: FormBuilder//1. inject the service
    ) { }

  ngOnInit(): void {
    this.initializeForm();
  }
  initializeForm() {
    //2. change this to use the form builder
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(8),
       ]],
      confirmPassword:['', [
        Validators.required,
         this.matchValues('password')
      ]],

    });
    this.registerForm.get('password')?.valueChanges.subscribe(() => {
      this.registerForm.get('confirmPassword')?.updateValueAndValidity();
    });
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {

      return control?.value === (control?.parent as FormGroup)?.controls[matchTo].value ? null : { isMatching: true };
    }

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
