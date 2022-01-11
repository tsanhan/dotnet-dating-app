import { PresenceService } from './services/presence.service';
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



  constructor(
    private accountService: AccountService,
    private presenceService: PresenceService//2. inject the presence service

    ) {
  }

  ngOnInit(): void {
    this.setCurrentUser();
  }

  setCurrentUser() {
    const userFromLS: any = localStorage.getItem('user');
    const user: User = JSON.parse(userFromLS);
    //1. we really should check if the user is null or not
    if(user) {
      this.accountService.setCurrentUser(user);
      //2. create the hub connection
      this.presenceService.createHubConnection(user);
      //3. next we'll go to the account.service.ts
    }
  }

}
