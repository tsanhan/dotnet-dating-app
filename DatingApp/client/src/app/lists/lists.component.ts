import { Component, OnInit } from '@angular/core';
import { Member } from '../models/member';
import { Pagination } from '../models/pagination';
import { MembersService } from '../services/members.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  members: Partial<Member>[] = []
  predicate = 'liked';
  //1. add pageNumber and pageSize for pagination
  pageNumber = 1;
  pageSize = 5;
  //3. add pagination
  pagination: Pagination;

  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    this.loadLikes();
  }


  loadLikes() {
    //2. pass pageNumber and pageSize to getLikes
    this.memberService.getLikes(this.predicate, this.pageNumber, this.pageSize).subscribe(members => {
      //4. the members are in result
      this.members = members.result;
      //5. the pagination is in pagination
      this.pagination = members.pagination;

    });
  }

  //6. to use pagination we need our pageChanged event
  pageChanged(event: any): void {
    this.pageNumber = event.page;
    this.loadLikes();
  }
  //7. go to the html to add the pagination ui

}
