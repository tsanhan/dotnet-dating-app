import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {

  //1. first thing we need to add a route to this component
  // * go to app-routing.module.ts
  constructor() { }

  ngOnInit() {
  }

}
