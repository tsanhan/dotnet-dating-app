import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { User } from '../models/user';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  currentUser$: Observable<User>;
  model: any = {};

  constructor(private accountService: AccountService,
    /*1. inject the router*/ private router :Router) {

    this.currentUser$ = this.accountService.currentUser$;
  }

  ngOnInit(): void {
  }

  login() {
    this.accountService.login(this.model)
      .subscribe(response => {
        //2. navigate
        this.router.navigateByUrl('/members');
        console.log(response);
      },
      error => {
        console.log(error);

      })
  }

  logout() {
    this.accountService.logout();
    //3. navigate
    this.router.navigateByUrl('/');
  }

}
