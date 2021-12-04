import { Component, Input, OnInit, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.css']
})
export class TextInputComponent implements ControlValueAccessor { // 1. we implement the ControlValueAccessor interface (read the inline doc)
  @Input() label: string;
  @Input() type = 'text';

  // 3.
  //  * inject using @Self(): DI will not go up the dependency hierarchy to check there NgControl is provided,
  //  we make sure this class is self contained and not dependent on any other class
  //  * we use the NgControl to get the form control, read NgControl's inline doc
  constructor(@Self() public ngControl: NgControl) {
    // 4. we bind the valueAccessor (the way to access the form control) to 'this' class
    this.ngControl.valueAccessor = this;
    // 5. go to the html
  }
  // 2. we must implement the methods but it does not matter if they are empty
  writeValue(obj: any): void {
  }
  registerOnChange(fn: any): void {
  }
  registerOnTouched(fn: any): void {
  }





}
