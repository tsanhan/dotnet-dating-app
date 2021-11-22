import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { Member } from 'src/app/models/member';
import { MembersService } from 'src/app/services/members.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  member: Member;
  //1. add this
  galleryOptions: NgxGalleryOptions[]
  galleryImages: NgxGalleryImage[]

  constructor(private memberService: MembersService, private route: ActivatedRoute ) { }

  ngOnInit(): void {
    this.loadMember();
    //2. add this
    this.galleryOptions = [{
      width: '500px',
      height: '500px',
      imagePercent: 100,
      thumbnailsColumns: 4,// number of images in a row under the main image
      imageAnimation: NgxGalleryAnimation.Slide,
      preview: false,
    }];

    //4. add this

    //6. after returning from the css, comment this: this.galleryImages = this.getImages();
    //5. to to the template
  }

  //3. add this
  getImages():NgxGalleryImage[] {
    const imageUrls = [];
    for (const photo of this.member.photos) {
      imageUrls.push({
        // optional chaining because users may not have photos
        small: photo?.url,
        medium: photo?.url,
        big: photo?.url,
      });
    }
    return imageUrls;
  }


  loadMember() {
    const username = this.route.snapshot.paramMap.get('username') as string;
    //7. change to this
    this.memberService.getMember(username).subscribe(member => {

      this.member = member;
      this.galleryImages = this.getImages();
    });
    //8. now test in the browser, great, go back to README.md
  }
}
