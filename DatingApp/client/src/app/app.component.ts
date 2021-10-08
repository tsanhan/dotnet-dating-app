import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'The Dating App';
  users: any // 3. ts gives up type safety unless we use 'any';

  //1. we use DI using the constructor
  constructor(private http: HttpClient/*can read from inline doc */) {

  }

  // 2. a life cycle event:
  // after the ctor is done, angular invoke the oninit LS Hook 
  // can read from OnInit inline doc
  ngOnInit(): void /*3. return type from method (in this case - nothing) */ {
    this.getUsers();
  }

  getUsers() {
    //4. using http 
    //on 'get' inline doc, show in parameters that '?' is optional, what the method returns
    // it return Observable (not available in JS it's related to rxjs)
    // they are streams of data, we wont go too deep here with Observables, 
    this.http.get('https://localhost:5001/api/users')
      // they are lazy, they do nothing unless somebody subscribe
      // all are optional
      // way 1:
      // .subscribe({
      //   next: response => {}, // what to do with returned data 
      //   error: error => {}, // what to do with error
      //   complete: () => {} // what to do when complete
      // })
      // way 2:
      .subscribe(response => { this.users = response }, error => { console.log(error); }, () => { }) // also all are optional
  }
}
