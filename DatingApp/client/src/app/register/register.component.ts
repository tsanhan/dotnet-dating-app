import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validator, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter<boolean>();

  //model: any = {}; // 6. remove this
  registerForm: FormGroup;
  maxDate: Date;
  //6. creat the property
  validationErrors: string[] = [];

  constructor(
    private accountService: AccountService,
    private toastr: ToastrService,
    private fb: FormBuilder,
    private router: Router

  ) { }

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);

  }
  initializeForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(8),
      ]],
      confirmPassword: ['', [
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

  // 1. uncomment the register method, with some fixes:
  register() {
    //2. register with the form data
    this.accountService.register(this.registerForm.value).subscribe((re) => {
      // 3.after registration, we'll redirect them to the members page (inject the router)
      this.router.navigate(['/members']);
      this.cancel();
    }, error => {
      // 5. in case of a mismatch (should not happen but still) we want to show them to the user, we'll get the error with come from the interceptor
      //    * create a property for validation errors
      this.validationErrors= error;
      //4. this.toastr.error(error.error);// we dont need this, the error will come from the interceptor
    });
    //7. go to the html

  }

  cancel() {
    this.cancelRegister.emit(false);
  }



}
