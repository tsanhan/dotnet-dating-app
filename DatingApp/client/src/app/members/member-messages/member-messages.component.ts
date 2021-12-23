import { Component, Input, OnInit } from '@angular/core';
import { Message } from 'src/app/models/message';
import { MessageService } from 'src/app/services/message.service';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
  //1. add fields
  @Input() username:string;
  messages:Message[];

  //2. inject what we need
  constructor(private messageService: MessageService) { }

  ngOnInit(): void {
    //3. load messages
    this.loadMessages();
  }

  //4. implement load messages method
  loadMessages() {
    this.messageService.getMessageThread(this.username).subscribe(messages => {
      this.messages = messages;
    })
  }
  //5. go to the html

}
