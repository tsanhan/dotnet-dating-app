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
  //1. we'ss start with a hub connection for our messages
  hubUrl = environment.hubUrl;
  private hubConnection: HubConnection;

  //8. subject and observable fot the messages
  private messageThreadSource$ = new BehaviorSubject<Message[]>([]);
  messageThread$ = this.messageThreadSource$.asObservable();

  constructor(
    private http: HttpClient
  ) { }

  //2. create a method to connect to the hub
  connectHubConnection(user: User, otherUsername: string) {
    //3. create a connection using a builder
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${this.hubUrl}message?user=${otherUsername}`, { // passing the other user's username as a query string
        accessTokenFactory: () => user.token // authenticate using the user's token
      })
      .withAutomaticReconnect() // to try and reconnect if the connection is lost
      .build();

    //6. start the connection
    this.hubConnection.start().catch(err => console.error(err));

    //7. now what will we do when we receive a message after connecting?
    // * we'll store the thread of the messages in an observable.
    // so create a source behavior subject and an observable to get the thread

    //9. create a method to handle the incoming messages (the methodName need to be exactly the same as the one in the hub)
    this.hubConnection.on('ReceiveMessageThread', (messages: Message[]) => {
      this.messageThreadSource$.next(messages);
    });

    //10. we don't deal with 'NewMessage' method yet.
    // * lets see see what we need to do in the message component...
    // * because there we'll need to:
    //    * create this connection,
    //    * receive the messages from the messageThread$ observable
    //    * deal with stopping the connection then the user (we'll create the method for it in a sec)
  }

  //11. add a method to stop the hub connection
  stopHubConnection() {
    this.hubConnection.stop().catch(error => console.log(error));
  }
  //12. back to README.md

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
