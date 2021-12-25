import { Component, Input, OnInit } from '@angular/core';
import { Message } from 'src/app/models/message';
import { MessageService } from 'src/app/services/message.service';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
  //1. make the messages an Input property:
  // messages:Message[];
  @Input() username:string;
  @Input() messages:Message[];

  constructor() { }

  ngOnInit(): void {
    // 2. no need to load the messaged on component init
    // this.loadMessages();

  }

  //3. cut this:
  // loadMessages() {
  //   this.messageService.getMessageThread(this.username).subscribe(messages => {
  //     this.messages = messages;
  //   })
  // }
  //4. go to member-detail.component.ts, point 6.

}
