import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Group } from '../models/group';
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

    this.hubConnection.on('NewMessage', (message: Message) => {
      const currentMessages = this.messageThreadSource$.getValue();
      this.messageThreadSource$.next([...currentMessages, message]);
    });

    //1. ok so when a user get a new message ('NewMessage') it's marked as read, cool!
    // * we get the message thread when we join  a group ('ReceiveMessageThread'), also cool...
    // * but what we want to make sure of is that:
    // * we, as [user2] will ne notified when [user1] entered the group.
    // * then we'll know all out messages was read by [user1]
    // * we will know [user1] entered the group by listening to the 'UpdatedGroup' method with data about the group.
    // * lets implement the 'UpdatedGroup' listener:
    this.hubConnection.on('UpdatedGroup', (group: Group) => {
      //2. if the connections in the group doesn't have my friend in them, then he is not in the group yet (we didn't read my messages yet)
      if (group.connections.some(x => x.username === otherUsername)) {
        //3. if he has entered the group I take the message thread I have
        this.messageThread$.pipe(take(1)).subscribe((messages: Message[]) => {
          //4. and update the dateRead to now, the date time he must have read them!
          messages.forEach(message => {
            if (!message.dateRead)
              message.dateRead = new Date(Date.now());
          });
          //5. and after the dateRead property in the message thread, we can up date the state with the new data.
          this.messageThreadSource$.next([...messages]);
        });
      }
      //6. back to README.md
    });


  }

  stopHubConnection() {
    if (this.hubConnection) {
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

  async sendMessage(username: string, content: string) {
    const createMessage = { recipientUsername: username, content };
    return this.hubConnection.invoke('SendMessage', createMessage)
      .catch(error => console.log(error));

  }

  deleteMessage(id: number) {
    return this.http.delete(this.baseUrl + 'messages/' + id);
  }


}
