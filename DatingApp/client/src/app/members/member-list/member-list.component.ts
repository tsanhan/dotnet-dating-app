import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/models/member';
import { MembersService } from 'src/app/services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  // 1. store members
  members: Member[] = [];

  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    //3. call member
    this.leadMembers();
  }

  //2. implement a method to fetch data
  leadMembers(){
    this.memberService.getMembers()
    .subscribe(members => {
      this.members = members; // no need to worry about errors 😉
    })
  }

  //4. test the component using the browser (login and enter localhost:4200/members)

  // 5. fix errors from last commit in MembersService if not working [can give as task to class] (1. Authentication => Authorization, 2.  as any)?.token, 3. users'` => users`)

}
