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

  constructor(private http: HttpClient, /*1. injecting the service*/private accountService: AccountService) {

  }

  ngOnInit(): void {
    this.getUsers();
    this.setCurrentUser(); // 3. invoking init state and go to nav.component.ts
  }

  //2. setting the current user
  setCurrentUser() {
    const userFromLS:any = localStorage.getItem('user');
    const user: User = JSON.parse(userFromLS);
    this.accountService.setCurrentUser(user);
  }

  getUsers() {
    this.http.get('https://localhost:5001/api/users')

      .subscribe(response => {
        this.users = response
      },
        error => { console.log(error); }, () => { }) // also all are optional
  }
}
