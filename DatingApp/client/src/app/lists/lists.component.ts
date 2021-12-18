import { Component, OnInit } from '@angular/core';
import { Member } from '../models/member';
import { MembersService } from '../services/members.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  //1. sore the members in the array, but these wont be our full member objects
  members: Partial<Member>[] = [] // every property is partial
  predicate = 'liked';

  //2. inject the members service
  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    this.loadLikes();
  }

  //3. load the members from the service
  loadLikes() {
    this.memberService.getLikes(this.predicate).subscribe(members => {
      this.members = members;
      //4. to fix the error we need to specify that getLikes method returns an observable on array of members
      // go to members.service.ts
      //5. go to the html
    });
  }

}
