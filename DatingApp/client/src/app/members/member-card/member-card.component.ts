import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { Member } from 'src/app/models/member';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css'],
  // 1. options here (can view in inline doc)
  // a. encapsulation: ViewEncapsulation.ShadowDom: (same result as angular but natively in the browser) need to explicitly specify in the code we use (https://developer.mozilla.org/en-US/docs/Web/Web_Components/Using_shadow_DOM)
  // b. encapsulation: ViewEncapsulation.Emulated: default.
  // c. encapsulation: ViewEncapsulation.None: no encapsulation.
})
export class MemberCardComponent implements OnInit {
  @Input() member!: Member;
  constructor() { }

  ngOnInit(): void {
  }

}
