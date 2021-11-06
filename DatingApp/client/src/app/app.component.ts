import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './models/user';
import { AccountService } from './services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'The Dating App';
  users: any



  constructor(/*3. no need: private http: HttpClient,*/ private accountService: AccountService) {

  }

  ngOnInit(): void {
    //2. no need fot this
    // this.getUsers();
    this.setCurrentUser();
  }

  setCurrentUser() {
    const userFromLS: any = localStorage.getItem('user');
    const user: User = JSON.parse(userFromLS);
    this.accountService.setCurrentUser(user);
  }

  // 1. we dont need this method - moving to home component:
  //   getUsers() {
  //     this.http.get('https://localhost:5001/api/users')

  //       .subscribe(response => {
  //         this.users = response
  //       },
  //         error => { console.log(error); }, () => { }) // also all are optional
  //   }
}
