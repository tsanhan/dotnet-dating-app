import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Member } from 'src/app/models/member';
import { Pagination } from 'src/app/models/pagination';
import { MembersService } from 'src/app/services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  //1. first return the members back to static array
  members: Member[];
  //2. add pagination information
  pagination: Pagination;
  pageNumber: number = 1;
  pageSize: number = 5;

  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    //4. call the load members method
    this.loadMembers();
    //5. go to the html
  }

  //3. add a load members method
  loadMembers() {
    this.memberService.getMembers(this.pageNumber, this.pageSize).subscribe(
      response => {
        this.members = response.result;
        this.pagination = response.pagination;
      }
    );
  }


}
