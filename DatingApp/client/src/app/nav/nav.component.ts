import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  //1. using 2 way binding (updating the view <==> component)
  model: any = {};
  constructor() { }

  ngOnInit(): void {

  }

  //2. on login lets see what the model holds
  //go to the view
  login() {
    console.log(this.model);

  }

}
