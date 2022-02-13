import { MembersService } from 'src/app/services/members.service';
import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { Message } from 'src/app/models/message';
import { MessageService } from 'src/app/services/message.service';
import { NgForm } from '@angular/forms';

@Component({
  //1. change the change detection strategy.
  changeDetection: ChangeDetectionStrategy.OnPush,
  //2. this is to avoid the change detection to run every time the component is rendered on the screen (which is a lot of times) and to run only when the component itself is changed (which is not the case)
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {

  @Input() username:string;
  @Input() messages:Message[];
  messageContent:string;

  constructor(
    public messageService: MessageService) { }

  ngOnInit(): void {

  }
  sendMessage(form:NgForm) {
    this.messageService.sendMessage(this.username, this.messageContent)

    .then(() => {
      form.reset();
    });

  }
}
