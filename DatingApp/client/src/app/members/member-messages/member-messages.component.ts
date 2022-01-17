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
    //1. we'll subscribe to the messages directly from the template so we'll make this injection public
    public messageService: MessageService) { }
    //2. go to the html

  ngOnInit(): void {

  }
  sendMessage(form:NgForm) {
    this.messageService.sendMessage(this.username, this.messageContent)
    .subscribe((message) => {
      this.messages.unshift(message as Message);
      form.reset();
    });
  }
}
