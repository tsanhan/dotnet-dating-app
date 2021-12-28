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
  messages: Message[] = [];
  pagination: Pagination;
  //1. start here to change to Unread
  container: string = 'Unread';
  //2. go to the html, to fix the way I use ngSwitch
  pageNumber: number = 1;
  pageSize: number = 5;

  //3. add the loading variable
  loading= false;

  constructor(private messageService: MessageService) { }

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages() {
    //4. we change the loading status to true
    this.loading = true;
    this.messageService.getMessages(this.pageNumber, this.pageSize, this.container).subscribe(response => {
      this.messages = response.result;
      this.pagination = response.pagination;
      //5. we change the loading status back
      this.loading = false;
      //6. go to the html to use this variable, point 4.
    });

  }

  pageChanged(event: any): void {
    if(this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.loadMessages();
    }
  }



}
