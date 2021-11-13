import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-test-errors',
  templateUrl: './test-errors.component.html',
  styleUrls: ['./test-errors.component.css']
})
export class TestErrorsComponent implements OnInit {
  baseURL = "https://localhost:5001/api/";
  //1. add property to the component
  validationErrors: string[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
  }

  //3. implement error methods
  get404Error() {
    this.http.get(this.baseURL + 'buggy/not-found').subscribe(response => {
      console.log(response);
    }, error => {
      console.log(error);
    });
  }

  get400Error() {
    this.http.get(this.baseURL + 'buggy/bad-request').subscribe(response => {
      console.log(response);
    }, error => {
      console.log(error);
    });
  }

  get500Error() {
    this.http.get(this.baseURL + 'buggy/server-error').subscribe(response => {
      console.log(response);
    }, error => {
      console.log(error);
    });
  }


  get401Error() {
    this.http.get(this.baseURL + 'buggy/auth').subscribe(response => {
      console.log(response);
    }, error => {
      console.log(error);
    });
  }

  get400ValidationError() {
    this.http.post(this.baseURL + 'account/register', {}).subscribe(response => {
      console.log(response);
    }, error => {
      console.log(error);
      //2. we know we get here
      this.validationErrors = error;
      //3. go to the template and display the errors
    });
  }

}
