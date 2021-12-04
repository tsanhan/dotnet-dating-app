import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validator, ValidatorFn, Validators } from '@angular/forms';
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
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.initializeForm();
  }
  initializeForm() {
    this.registerForm = new FormGroup({
      username: new FormControl("Hello", Validators.required),
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(8),
       ]),
      confirmPassword: new FormControl('', [
        Validators.required,
         /*2. use the validator*/this.matchValues('password')
      ]),
      //3. testing the functionality in the browser
      // * this looks like it's working but it's not:
      // * AFTER getting a valid state, try to change the password field
      // * why do you think it's not working?
      // * answer: because the validator is running only on confirmPassword change
      // * how would you fix this? (remember we can listen to control changes and we can check validity)
      // * we can just add the same validator to the password field (is it a good idea?)
      //   * try it' tell me what happens
      //   * the problem here is that on the first right match the other field is still not valid
      // * we can listen to changes in the password field and update the validity of the confirmPassword field
    });
    //4.
    this.registerForm.get('password')?.valueChanges.subscribe(() => {
      this.registerForm.get('confirmPassword')?.updateValueAndValidity();
    });
  }

  //1. add a validator (a function returning a ValidatorFn)
  matchValues(matchTo: string): ValidatorFn {
    // all our consols are derived from AbstractControl (can go up the parent chain to see)
    return (control: AbstractControl): { [key: string]: any } | null => {

      // we checking this ⬇️ value against the value of the matchTo control
      return control?.value === (control?.parent as FormGroup)?.controls[matchTo].value ? null : { isMatching: true };
      // if the control is not valid, return {isMatching: true} (the validator error)
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
