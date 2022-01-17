import { AccountService } from './../../services/account.service';
import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { Member } from 'src/app/models/member';
import { Message } from 'src/app/models/message';
import { MembersService } from 'src/app/services/members.service';
import { MessageService } from 'src/app/services/message.service';
import { PresenceService } from 'src/app/services/presence.service';
import { User } from 'src/app/models/user';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit ,OnDestroy {
  @ViewChild('memberTabs', {static:true}) memberTabs: TabsetComponent;

  messages:Message[] = [];
  member: Member;
  galleryOptions: NgxGalleryOptions[]
  galleryImages: NgxGalleryImage[]

  activeTab: TabDirective;
  user: User;

  constructor(
    private route: ActivatedRoute,
    private messageService: MessageService,
    public presence: PresenceService,
    //1. we'll need the account service to get the user,
    //* this is needed to create the hub connection in the message service
    private accountService: AccountService

    ) {
      //2. populate local user property
      this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user)
    }

  ngOnInit(): void {

    this.route.data.subscribe(data => {
      this.member = data['member'];
    });

    this.route.queryParams.subscribe(params => {
      params.tab ? this.selectTab(params.tab) : this.selectTab(0);
    });

    this.galleryOptions = [{
      width: '500px',
      height: '500px',
      imagePercent: 100,
      thumbnailsColumns: 4,
      imageAnimation: NgxGalleryAnimation.Slide,
      preview: false,
    }];

    this.galleryImages = this.getImages();

  }

  getImages():NgxGalleryImage[] {
    const imageUrls = [];
    for (const photo of this.member.photos) {
      imageUrls.push({
        small: photo?.url,
        medium: photo?.url,
        big: photo?.url,
      });
    }
    return imageUrls;
  }



  selectTab(tabId: number) {
    this.memberTabs.tabs[tabId].active = true;
  }


  onTabActivated(data: TabDirective){
    this.activeTab = data;
    if(this.activeTab.heading === 'Messages' && this.messages.length === 0) {
      //3. right now, on tab selection we just load the messages  (api call)
      // * we'll change this because we'll get those messages from SignalR hub
      // this.loadMessages();
      //4. get the messages from SignalR
      this.messageService.createHubConnection(this.user, this.member.username)
    //5. else I want the user to disconnect from the messages hub on tab switch
    } else {
      this.messageService.stopHubConnection();
      //6. but wait, there is another reason to stop the hub connection:
      //* if the user is exiting the member  details
      //* in other words, when this component is no longer show on the screen
      //* when this happens, component is destroyed, and there is a hook for that,
      //* it's called OnDestroy and we can execute code when it happen
      //* go and add OnDestroy to the interfaces this component 'implements'
    }
  }
  //7. implement ngOnDestroy
  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
    //8. now we could have an issue here, we could try to stop the connection after we already did that.
    // * if we changed the tab AND navigated away from this component
    // * we'll fix this in the stopHubConnection method is self.
    // * go to message.service.ts

    //9. now that we don't populate the message array, we don't need to pass them down to the member messages component
    // go to the html
  }


  loadMessages() {
    this.messageService.getMessageThread(this.member.username).subscribe(messages => {
      this.messages = messages;
    })
  }
}
