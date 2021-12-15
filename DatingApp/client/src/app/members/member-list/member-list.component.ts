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
  userParams: UserParams;
  user: User;

  genderList = [{ value: 'male', display: 'Males' }, { value: 'female', display: 'Females' }]


  constructor(private memberService: MembersService,/*2. no need for the accountService*//* private accountService: AccountService */ ) {
    //1. so this is where contracting the userParams object.
    // we need to do it in the members.service.ts.
    // go to members.service.ts
    //3. use the member service to populate the userParams object

    // accountService.currentUser$.pipe(take(1)).subscribe(user => {
    //   this.user = user;
    //   this.userParams = new UserParams(user);
    // });
    this.userParams = this.memberService.UserParams;

  }

  ngOnInit(): void {
    this.loadMembers();
  }





  loadMembers() {
    //4. before we actually go get the members, we'll set the user params (this method is called many times)
    this.memberService.UserParams = this.userParams;

    this.memberService.getMembers(this.userParams).subscribe(
      response => {
        this.members = response.result;
        this.pagination = response.pagination;
      }
    );
  }
  resetFilters() {
    //5. to reset the filters we'll add a new resetUserParams() method to the members.service.ts
    //6. go to members.service.ts
    // this.userParams = new UserParams(this.user);
    //7. reset the userParams object
    this.userParams = this.memberService.resetUserParams();
    this.loadMembers();
  }

  pageChanged(event: any) {
    this.userParams.pageNumber = event.page;
    //8. update the userParams in the service
    this.memberService.UserParams = this.userParams;
    //9. test in the browser, change filters, go and come back the members page to see the filters stay
    //10. back to readme.md
    this.loadMembers();

  }



}
