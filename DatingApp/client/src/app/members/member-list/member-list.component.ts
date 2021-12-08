import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/models/member';
import { Pagination } from 'src/app/models/pagination';
import { User } from 'src/app/models/user';
import { UserParams } from 'src/app/models/userParams';
import { AccountService } from 'src/app/services/account.service';
import { MembersService } from 'src/app/services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  members: Member[];
  pagination: Pagination;
  //1. instead using these props we'll use UserParams
  // pageNumber: number = 1;
  // pageSize: number = 5;
  userParams: UserParams;
  user: User;

  constructor(private memberService: MembersService, /*2. need to get the user to populate userParams */private accountService: AccountService) {
    // 3. populate local user and userParams properties
    accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user;
      this.userParams = new UserParams(user);
    });
  }

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    // 4. use th local variables
    this.memberService.getMembers(this.userParams).subscribe(
      response => {
        this.members = response.result;
        this.pagination = response.pagination;
      }
    );
  }

  //5. update the userParams
  pageChanged(event: any){
    this.userParams.pageNumber = event.page;
    this.loadMembers();

  }
//6. back to readme.md



}
