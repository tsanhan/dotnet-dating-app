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
  //2. add a property to hold the message content
  messageContent:string;

  //1. inject the message service
  constructor(private messageService: MessageService) { }

  ngOnInit(): void {

  }
  //3. create a method to send a message
  sendMessage(form:NgForm) {
    this.messageService.sendMessage(this.username, this.messageContent)
    // we know we get the message back from the API, we'll add it to the array;
    .subscribe((message) => {
      // put the message in the beginning of the array (descending order)
      this.messages.unshift(message as Message);
      // after the data is pushed to the array, we'll reset the form
      form.reset();
    });
  }
  //4. do to the html
}
