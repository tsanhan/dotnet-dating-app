import { ToastrService } from 'ngx-toastr';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { User } from '../models/user';
import { BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
  hubUrl = environment.hubUrl;
  private onlineUsersSource$ = new BehaviorSubject<string[]>([]);
  onlineUsers$ = this.onlineUsersSource$.asObservable();


  private hubConnection: HubConnection;
  constructor(
    private toastr: ToastrService,
    private router: Router
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
      //1. here we'll remove this annoying toastr.
      // * and replace it with the ability to track the users that are online right now, add the new user to the list.
      // this.toastr.info(`${username} has connected`);
      this.onlineUsers$.pipe(take(1)).subscribe(onlineUsers => this.onlineUsersSource$.next([...onlineUsers, username]));
    });

    this.hubConnection.on('UserIsOffline', (username) => {
      //2. do similar thing here
      // this.toastr.warning(`${username} has disconnected`);
      this.onlineUsers$.pipe(take(1)).subscribe(onlineUsers => this.onlineUsersSource$.next([...onlineUsers.filter(u => u !== username)]));
    });
    //3. back to README.md

    this.hubConnection.on('GetOnlineUsers', (usernames: string[]) => {
      this.onlineUsersSource$.next(usernames);
    });


    this.hubConnection.on('NewMessageReceived', ({username, knownAs}) => {
      this.toastr.info(`${knownAs} send you a new message!`)

      .onTap
      .pipe(take(1))
      .subscribe(() => {
        this.router.navigateByUrl(`/members/${username}?tab=3`);
      } );

    });

  }

  stopHubConnection(){
    this.hubConnection
    .stop()
    .catch(error => console.log(error));
  }
}
