import { Component, OnInit } from '@angular/core';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/models/member';
import { User } from 'src/app/models/user';
import { AccountService } from 'src/app/services/account.service';
import { MembersService } from 'src/app/services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  // 1. declare the member and the current user
  member: Member;
  user:User;

  constructor(
    //2. to populate the member and the current user I need the services
    private accountService: AccountService,
    private memberService: MembersService,

    ) {
      //3. at first I need the user data
      this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user); // you can say it's synchronous
    }

  ngOnInit(): void {
    //5. I need to get the member data
    this.loadMember();
    //6. go to the template to show the member username
  }

  //4. I need to get the member data based on the username
  loadMember() {
    this.memberService.getMember(this.user.username).subscribe(member => this.member = member);
  }

}
