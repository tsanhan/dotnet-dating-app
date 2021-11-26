import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Member } from 'src/app/models/member';
import { MembersService } from 'src/app/services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  //1. first we'll say our members will be an observable
  members$: Observable<Member[]>

  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    //2. populate the members Observable
    this.members$ = this.memberService.getMembers();
  }

  //3. we don't need the leadMembers method anymore
  // leadMembers(){
  //   this.memberService.getMembers()
  //   .subscribe(members => {
  //     this.members = members;
  //   })
  // }

  //4. so how will get the data from the observable?
  // * we'll use the async pipe,
  // * async pipe subscribes and unsubscribes when the component is destroyed
  // * go to the html and add the async pipe
}
