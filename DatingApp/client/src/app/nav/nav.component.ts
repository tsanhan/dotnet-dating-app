import { Component, OnInit } from '@angular/core';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  //3. store a property if logged in
  loggedIn: boolean = false;
  model: any = {};
  //1. inject account service
  constructor(private accountService: AccountService) { }

  ngOnInit(): void {

  }

  login() {
    //2. login
    this.accountService.login(this.model) // returning observable (it's lazy)
      .subscribe(response => {
        console.log(response);
        this.loggedIn = true;
      },
      error => {
        console.log(error);

      })

  }

}
