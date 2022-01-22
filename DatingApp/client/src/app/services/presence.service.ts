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
      this.toastr.info(`${username} has connected`);
    });

    this.hubConnection.on('UserIsOffline', (username) => {
      this.toastr.warning(`${username} has disconnected`);
    });

    this.hubConnection.on('GetOnlineUsers', (usernames: string[]) => {
      this.onlineUsersSource$.next(usernames);
    });

    //1. add a new hub connection event
    this.hubConnection.on('NewMessageReceived', ({username, knownAs}) => {
      this.toastr.info(`${knownAs} send you a new message!`)
      // one cool thing we get with this toastr is the fact we can handle events related to the toastr using observables.
      //  * here is want to navigate (to the chat) the user if he tap on the massage
      .onTap
      .pipe(take(1))
      .subscribe(() => {
        // we want to navigate the user to the chat page of the user that send the message
        this.router.navigateByUrl(`/members/${username}?tab=3`);
      } );
      // back to README.md

    });

  }

  stopHubConnection(){
    this.hubConnection
    .stop()
    .catch(error => console.log(error));
  }
}
