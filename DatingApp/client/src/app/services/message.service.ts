import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Message } from '../models/message';
import { User } from '../models/user';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl;
  hubUrl = environment.hubUrl;
  private hubConnection: HubConnection;

  private messageThreadSource$ = new BehaviorSubject<Message[]>([]);
  messageThread$ = this.messageThreadSource$.asObservable();

  constructor(
    private http: HttpClient
  ) { }

  //2. on the way, we'll fix 2 things here:
  // * 1. change the name of the method to createHubConnection (user rename symbol)
  // * 2. change the query param to username: '...message?user=$...' to  'message?username='
  createHubConnection(user: User, otherUsername: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${this.hubUrl}message?username=${otherUsername}`, {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(err => console.error(err));

    this.hubConnection.on('ReceiveMessageThread', (messages: Message[]) => {
      this.messageThreadSource$.next(messages);
    });

  }

  stopHubConnection() {
    //1. if the connection exist we'll stop it:
    if(this.hubConnection) {
      this.hubConnection.stop().catch(error => console.log(error));
    }
    //3. go back to member-detail.component.ts, point 9.
  }

  getMessages(pageNumber: number, pageSize: number, container: string) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('container', container);

    return getPaginatedResult<Message[]>(this.baseUrl + 'messages', params, this.http);
  }

  getMessageThread(username: string) {
    return this.http.get<Message[]>(`${this.baseUrl}messages/thread/${username}`);
  }

  sendMessage(username: string, content: string) {
    const createMessage = { recipientUsername: username, content };
    return this.http.post(this.baseUrl + 'messages', createMessage);
  }

  deleteMessage(id: number) {
    return this.http.delete(this.baseUrl + 'messages/' + id);
  }


}
