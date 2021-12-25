import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { Member } from 'src/app/models/member';
import { Message } from 'src/app/models/message';
import { MembersService } from 'src/app/services/members.service';
import { MessageService } from 'src/app/services/message.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  //1. get that memberTabs
  @ViewChild('memberTabs') memberTabs: TabsetComponent;

  messages:Message[] = [];
  member: Member;
  galleryOptions: NgxGalleryOptions[]
  galleryImages: NgxGalleryImage[]

  //2. store the current activated tab
  activeTab: TabDirective;
  //3 what is a directive? [talk a bit structural(*)/attribute[] directives].
  //  * what is the component vs directive? actually, component extends directive, only a component has a template.

  constructor(private memberService: MembersService, private route: ActivatedRoute, private messageService: MessageService ) { }
  ngOnInit(): void {
    this.loadMember();
    this.galleryOptions = [{
      width: '500px',
      height: '500px',
      imagePercent: 100,
      thumbnailsColumns: 4,
      imageAnimation: NgxGalleryAnimation.Slide,
      preview: false,
    }];

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


  loadMember() {
    const username = this.route.snapshot.paramMap.get('username') as string;
    this.memberService.getMember(username).subscribe(member => {

      this.member = member;
      this.galleryImages = this.getImages();
    });
  }

  //4. now we need to follow the tab change event
  onTabActivated(data: TabDirective){
    this.activeTab = data;
    if(this.activeTab.heading === 'Messages' /*7. no need to reload, if we already have messages*/ && this.messages.length === 0) {
      // 5. this is where we load the messages...
      //  * but we load the messages in the member messages component right?
      //  * well, we'll need to move it to here
      //  * go to member-messages.component.ts

      // 8. load the messages
      this.loadMessages();
      // 9. pass the messages to the component, go to the html, point 3
    }
  }

  //6. paste here
  // * now we see we missing some things:
  //  1. we need the message service, so go and inject it
  //  2. we need the messages, so go and add it like in member-messages.component.ts
  //  3. we need the username, this is easy, we have it in the member object
  loadMessages() {
    this.messageService.getMessageThread(this.member.username).subscribe(messages => {
      this.messages = messages;
    })
  }
}
