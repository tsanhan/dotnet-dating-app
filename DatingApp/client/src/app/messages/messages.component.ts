import { Component, OnInit } from '@angular/core';
import { Message } from '../models/message';
import { Pagination } from '../models/pagination';
import { MembersService } from '../services/members.service';
import { MessageService } from '../services/message.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  //1. add properties
  messages: Message[] = [];
  pagination: Pagination;
  container: string = 'Inbox';
  pageNumber: number = 1;
  pageSize: number = 5;
  //2. inject services
  constructor(private messageService: MessageService) { }

  ngOnInit(): void {
    this.loadMessages();
  }

  //3. implement loadMessages method
  loadMessages() {
    this.messageService.getMessages(this.pageNumber, this.pageSize, this.container).subscribe(response => {
      this.messages = response.result;
      this.pagination = response.pagination;
    });

  }

  //4. implement pageChanged method
  pageChanged(event: any): void {
    if(this.pageNumber !== event.page) { // what do you think will happen if we don't check this? (answer infinite loop)
      this.pageNumber = event.page;
      this.loadMessages();
    }
  }

  //5 go to the html file


}
