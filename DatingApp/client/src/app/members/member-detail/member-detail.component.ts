import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
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
  @ViewChild('memberTabs'/*5. add static */, {static:true}) memberTabs: TabsetComponent;

  messages:Message[] = [];
  member: Member;
  galleryOptions: NgxGalleryOptions[]
  galleryImages: NgxGalleryImage[]

  activeTab: TabDirective;

  constructor(private memberService: MembersService, private route: ActivatedRoute, private messageService: MessageService ) { }
  ngOnInit(): void {
    this.loadMember();

    //3. get the query params:
    this.route.queryParams.subscribe(params => {
      params.tab ? this.selectTab(params.tab) : this.selectTab(0);
    });
    //4. test to see if works, oops, not working! why? (try to debug)
    // * answer: all our template content is conditional
    // * we'll start with a partial solution: add a static property to the ViewChild, do that.
    // 6.
    // * 'static' means that we can access the property before it might change doe to change detection.
    // * change detection is a under the hood mechanism angular uses to check if anything changed
    //   * like clicking, an async operation, and even component creation, like this one.
    // * why this is part of the solution? because we change memberTabs synchronously, and we need access to it.
    // * how is it synchronous?
    //   * well, ngOninit is running after the component is constructed.
    //   * after ngOninit is done a(!) change detection cycle is starting
    //   * static:false will be resolve the ViewChild after the change detection cycle
    //   * static:true will be resolve the ViewChild before the change detection cycle
    //   * and we need access to the ViewChild before the change detection cycle!
    //   * why? because all our operations in ngOninit are synchronous.
    //   * the ViewChild resolves too late if we don't make it static.
    // 7. if we have time we'll talk about change detection/ and the component life cycle in more detail.
    // 8. after chekiang if it works we still get the error, this is because all our template content is still conditional
    //   * so.. we just remove the condition right? go and do that, go to the html, point 3


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

  //1. create a method to activate a tab
  selectTab(tabId: number) {
    this.memberTabs.tabs[tabId].active = true;
  }
  //2. go to the html to activate this method


  onTabActivated(data: TabDirective){
    this.activeTab = data;
    if(this.activeTab.heading === 'Messages' && this.messages.length === 0) {
      this.loadMessages();
    }
  }


  loadMessages() {
    this.messageService.getMessageThread(this.member.username).subscribe(messages => {
      this.messages = messages;
    })
  }
}
