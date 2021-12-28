import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Message } from '../models/message';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl;

  constructor(
    private http: HttpClient
  ) { }

  getMessages(pageNumber:number, pageSize:number, container:string) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('container', container);

    return getPaginatedResult<Message[]>(this.baseUrl + 'messages', params, this.http);
  }

  getMessageThread(username: string){
    return this.http.get<Message[]>(`${this.baseUrl}messages/thread/${username}`);
  }

  sendMessage(username:string, content:string) {
    const createMessage = {recipientUsername: username, content};
    return this.http.post(this.baseUrl + 'messages', createMessage);
  }

  //1. ad the functionality to delete a message.
  deleteMessage(id:number) {
    return this.http.delete(this.baseUrl + 'messages/' + id);
  }
  //2. go to messages.component.ts to use this method


}
