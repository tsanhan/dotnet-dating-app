import { ConfirmService } from './../services/confirm.service';
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
  container: string = 'Unread';
  pageNumber: number = 1;
  pageSize: number = 5;

  loading = false;

  constructor(
    private messageService: MessageService,
    private confirmService: ConfirmService //1. inject the service
  ) { }

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages() {
    this.loading = true;
    this.messageService.getMessages(this.pageNumber, this.pageSize, this.container).subscribe(response => {
      this.messages = response.result;
      this.pagination = response.pagination;
      this.loading = false;
    });

  }

  pageChanged(event: any): void {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.loadMessages();
    }
  }

  deleteMessage(id: number) {
    //2. use the confirm service to show the modal, to insure the user is sure they want to delete the message
    this.confirmService.confirm('Confirm Delete Message', 'This cannot be undone!').subscribe(result => {
      if (result) {
        this.messageService.deleteMessage(id).subscribe(() => {
          this.messages.splice(this.messages.findIndex(m => m.id === id), 1);
        });
      }
    })
    //3. back to README.md

  }





}
