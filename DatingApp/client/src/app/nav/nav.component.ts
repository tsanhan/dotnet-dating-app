import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
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

  constructor(
    private accountService: AccountService,
    private router :Router,
    private toastr: ToastrService) {

    this.currentUser$ = this.accountService.currentUser$;
  }

  ngOnInit(): void {
  }

  login() {
    this.accountService.login(this.model)
      .subscribe(response => {
        this.router.navigateByUrl('/members');
        console.log(response);
      },
      // 1. we dont need error handling here now
      )
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

}
