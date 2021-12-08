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
  members: Member[];
  pagination: Pagination;
  pageNumber: number = 1;
  pageSize: number = 5;

  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    this.memberService.getMembers(this.pageNumber, this.pageSize).subscribe(
      response => {
        this.members = response.result;
        this.pagination = response.pagination;
      }
    );
  }
  //1. add the method
  pageChanged(event: any){
    this.pageNumber = event.page;
    this.loadMembers();
    //2. test in browser, works cool
    //3. all is working fine:
    // * paging back and forth,
    // * to last page and to first page
    // * links are disabled when on last/first page (this is thanks to [boundaryLinks] attribute)

    // 4. back to readme.md
  }




}
