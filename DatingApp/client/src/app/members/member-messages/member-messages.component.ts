import { MembersService } from 'src/app/services/members.service';
import { Component, Input, OnInit } from '@angular/core';
import { Message } from 'src/app/models/message';
import { MessageService } from 'src/app/services/message.service';
import { NgForm } from '@angular/forms';

@Component({
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
    //1. we'll need to handle this method differently now that it returns a promise we have a hub connection.
    // .subscribe((message) => {
    //   this.messages.unshift(message as Message);
    //   form.reset();
    // });
    .then(() => {
      form.reset();
    });
    //2. back to README.md
  }
}
