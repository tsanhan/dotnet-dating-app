import { Component, OnInit } from '@angular/core';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  loggedIn: boolean = false;
  model: any = {};
  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
    this.getCurrentUser(); //3. use at on init, and go back to README.md
  }

  login() {
    this.accountService.login(this.model)
      .subscribe(response => {
        console.log(response);
        this.loggedIn = true;
      },
      error => {
        console.log(error);

      })
  }

  logout() {
    this.accountService.logout(); //1. use service to logout
    this.loggedIn = false;
  }

  // 2. get the current user
  getCurrentUser() {
    this.accountService.currentUser$.subscribe(user => {
      this.loggedIn = !!user;
    },
    error => {
      console.log(error);

    })
  }

}
