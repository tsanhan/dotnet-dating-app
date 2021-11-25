import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
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
  //4. after point 8. in the template:
  // creating a reference to the form in the template
  @ViewChild('editForm') editForm: NgForm;

  member: Member;
  user:User;

  constructor(
    private accountService: AccountService,
    private memberService: MembersService,
    private toastr: ToastrService//2. add the toastr service
    ) {
      this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
    }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    this.memberService.getMember(this.user.username).subscribe(member => this.member = member);
  }

  //1. update the member
  updateMember() {
    // place holder for update member
    console.log(this.member);

    //3. use the service and go back to the template
    this.toastr.success('Profile updated successfully');

    //5. reset the form with the member value:
    this.editForm.reset(this.member);
    //6. test in the browser, after submitting, the alert disappears and the button is disabled
    //7. the reset functionality in the NgForm
    //   * the functionality is using the 'name' attribute in the template to bind to the form controls
    //   * the ngModel is to bind to the member property
    //   * we can the difference when if we change the 'name' (say from lookingFor to lookingfor)
    //   * after the submit (editForm.reset run) we see nothing in lookingFor
    //   * this is because NgForm could not find a form control for 'lookingfor'
    //   * go to the template and change the 'name' attribute to test this
  }

}
