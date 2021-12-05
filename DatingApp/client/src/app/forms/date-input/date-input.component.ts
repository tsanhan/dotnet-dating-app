import { Component, Input, OnInit, Self } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'app-date-input',
  templateUrl: './date-input.component.html',
  styleUrls: ['./date-input.component.css']
})
export class DateInputComponent implements ControlValueAccessor {
  @Input() label: string;

  //1.
  // will show the earliest date the datepicker will show
  // so we could say "You have to be over 18 to use this website"
  @Input() maxDate: Date;

  //2. based on documentation, we need to configure the bsDatepicker directive (what is a directive? it's a class wrapping a DOM element (Component derives from Directive), we will learn about it later)
  bsConfig: Partial<BsDatepickerConfig>; // Partial means that every property is optional, without Partial we'll have to provide every property in BsDatepickerConfig


  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
    //3.
    this.bsConfig = {
      containerClass: 'theme-dark-blue',
      dateInputFormat: 'DD MMMM YYYY' //2 digit day, full month name, 4 digit year
    };
    //4. go to the html
  }

  writeValue(obj: any): void {
  }
  registerOnChange(fn: any): void {
  }
  registerOnTouched(fn: any): void {
  }



}
