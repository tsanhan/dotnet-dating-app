import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';
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

    //1. we'll add another listener connection to handle 'NewMessage' method
    this.hubConnection.on('NewMessage', (message: Message) => {
      const currentMessages = this.messageThreadSource$.getValue();
      this.messageThreadSource$.next([...currentMessages, message]);
    });



  }

  stopHubConnection() {
    if(this.hubConnection) {
      this.hubConnection.stop().catch(error => console.log(error));
    }
  }

  getMessages(pageNumber: number, pageSize: number, container: string) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('container', container);

    return getPaginatedResult<Message[]>(this.baseUrl + 'messages', params, this.http);
  }

  getMessageThread(username: string) {
    return this.http.get<Message[]>(`${this.baseUrl}messages/thread/${username}`);
  }

  //3. this can be an async function (we return a promise)
  // * but more importantly, we want to know when the message is sent, so we could reset the message form
  async sendMessage(username: string, content: string) {
    const createMessage = { recipientUsername: username, content };
    //2. about the sending message, I see we create an api call, we'll our hub instead:
    //  * the way sending messages work in the hub is by calling the 'invoke' method, with the method name in the hub (in the BE,'SendMessage' in our case) and the parameters
    return this.hubConnection.invoke('SendMessage', createMessage)
              //now this does not return an observable like API call, this returns a promise, and we don't have access to our interceptor to handle the response
              .catch(error => console.log(error));

    // return this.http.post(this.baseUrl + 'messages', createMessage);
  }
  //4. go to member-messages.component.ts to use this method

  deleteMessage(id: number) {
    return this.http.delete(this.baseUrl + 'messages/' + id);
  }


}
