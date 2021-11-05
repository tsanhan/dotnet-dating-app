import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../models/user';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  //2. remove the loggedIn property from the nav component
  // loggedIn: boolean = false;

  //3. use currentUser$ to get the current user
  currentUser$: Observable<User>;


  model: any = {};
  constructor(private accountService: AccountService) {
    this.currentUser$ = this.accountService.currentUser$;
  }

  ngOnInit(): void {
    //7. we dont need this anymore
    // this.getCurrentUser();

    //8. go to nav.component.html and switch the ngIf to the currentUser$
    //9. two things cna be changed here:
    // 1 we can use this.accountService.currentUser$ in the template, no need for this.currentUser$
    // 2 to do 1 we need the accountService in the constructor to be public

    //10. we can remove the logout() link in the nav.component.html
  }

  login() {
    this.accountService.login(this.model)
      .subscribe(response => {
        console.log(response);
        // 4. no need of this anymore
        // this.loggedIn = true;
      },
      error => {
        console.log(error);

      })
  }

  logout() {
    this.accountService.logout();
    // 5. no need of this anymore
    // this.loggedIn = false;
  }

  //6. we dont need this anymore
  // getCurrentUser() {
  //   //1. we should not use subscribe here:
  //   // this current user is not an http request, this never completes: potential for memory leak (explain why - what async pipe do?)
  //   // lets use the async pipe
  //   this.accountService.currentUser$.subscribe(user => {
  //     this.loggedIn = !!user;
  //   },
  //   error => {
  //     console.log(error);

  //   })
  // }

}
