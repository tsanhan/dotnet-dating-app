import { ToastrService } from 'ngx-toastr';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { User } from '../models/user';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
  hubUrl = environment.hubUrl;
  //1. store the online users in a behavior subject
  private onlineUsersSource$ = new BehaviorSubject<string[]>([]);
  //2. we'll have another observable to get the online users
  onlineUsers$ = this.onlineUsersSource$.asObservable();


  private hubConnection: HubConnection;
  constructor(
    private toastr: ToastrService
  ) { }

  createHubConnection(user: User) {
    this.hubConnection = new HubConnectionBuilder().withUrl(
      `${this.hubUrl}presence`,
      {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
    .start()
    .catch(error => console.log(error));

    this.hubConnection.on('UserIsOnline', (username) => {
      this.toastr.info(`${username} has connected`);
    });

    this.hubConnection.on('UserIsOffline', (username) => {
      this.toastr.warning(`${username} has disconnected`);
    });

    //3. we'll create another listening event for getting the online users
    this.hubConnection.on('GetOnlineUsers', (usernames: string[]) => {
      this.onlineUsersSource$.next(usernames);
    });
    //4. next we need to add the indication if the user is online in the member card and member detail components
    //* go to member-card.component.ts
  }

  stopHubConnection(){
    this.hubConnection
    .stop()
    .catch(error => console.log(error));
  }
}
