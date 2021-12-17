import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/models/member';
import { MembersService } from 'src/app/services/members.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css'],

})
export class MemberCardComponent implements OnInit {
  @Input() member!: Member;
  constructor(
    //1. we inject things we'll need
    private memberService: MembersService,
    private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  //2. implement add like
  addLike(member: Member) {
    this.memberService.addLike(member.username).subscribe(
      // we don't get anything from the server on success
      () => {
        this.toastr.success(`You have liked: ${member.knownAs}`);
      });
      // no need handle error, our interceptor will handle it
  }
  //3. go to the html

}
