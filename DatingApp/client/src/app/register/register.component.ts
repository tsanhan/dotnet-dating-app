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
    private fb: FormBuilder
    ) { }

  ngOnInit(): void {
    this.initializeForm();
  }
  initializeForm() {
    this.registerForm = this.fb.group({
      //1. the order is not important
      gender: ['male'], // a radio button, no need for validator
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],// we;ll need to do something about this but for now we'll keep it a a string
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(8),
       ]],
      confirmPassword:['', [
        Validators.required,
         this.matchValues('password')
      ]],
      //2. go to the html
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
