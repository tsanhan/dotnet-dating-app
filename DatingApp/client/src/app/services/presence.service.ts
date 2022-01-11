import { ToastrService } from 'ngx-toastr';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
  //1. store the hubUrl
  hubUrl = environment.hubUrl;

  //2. store the hub connection object we het from SignalR
  private hubConnection: HubConnection;

  constructor(
    private toastr: ToastrService //3. inject toastr
  ) { }

  //4. a method to create the hub connection
  // * when a user connect to our app AND they authenticated,
  // * we'll automatically create a hub connection that will connect them to our presence hub
  createHubConnection(user: User) { // we passing the user because we'll need to sent the jwt when we make the connection as well
    // * we cannot use the interceptor, it's good for http, and this is not http anymore, it's websocket
    this.hubConnection = new HubConnectionBuilder().withUrl(
      `${this.hubUrl}presence`,
      {
        // this method is to get and add the access token to every message
        accessTokenFactory: () => user.token
      })
      // we also want to reconnect automatically when the connection is lost
      .withAutomaticReconnect()
      .build();

    //5. so far we created the hub connection, lets start the hub connection
    this.hubConnection
    .start()
    .catch(error => console.log(error));

    //6. now that we started the connection lets listen to event cumming from the hub.
    // * remember we send methods and the username? lets react on them.
    this.hubConnection.on('UserIsOnline', (username) => {
      //7. when the event is triggered, we want to show a toastr message
      // * we wont keep it here, in real life this will be vey enoing
      this.toastr.info(`${username} has connected`);
    }

    );
    //8. also react to UserIsOffline method
    this.hubConnection.on('UserIsOffline', (username) => {
      this.toastr.warning(`${username} has disconnected`);
    });

    //9. so.. conclude so far:
    // 1. we send the message ans we see in the PresenceHub.cs the message is sent
    //     * this happens when a user connects to all the other users already connected
    // 2. in the client we listen to the a specific method
    //     * as we ware already connected before (as different user) and react to the newly connected user (not us)
  }
  //10. we need a way to stop the hub connection
  stopHubConnection(){
    this.hubConnection
    .stop()
    .catch(error => console.log(error));
  }

  //11. now when do we want to create/stop the hub connection?
  // * create the hub connection:
  //     * the app first starts IF the user is logged in
  //     * when the user logs in or register
  // * stop the hub connection:
  //     * if the user is logged out

  //12. go to app.component.ts

}
