import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  //2. add user as any( well add typescript later)
  users: any;

  constructor(/*1*/private http: HttpClient) { }

  ngOnInit(): void {
    this.getUsers(); // the answer (at the end)
  }

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  //3. recreate the getUsers method here
  getUsers() {
    this.http.get('https://localhost:5001/api/users')
      .subscribe(
        users => this.users = users,
        error => { console.log(error); }, () => { })
  }

  //4. go to register.component.ts to use the @Input decorator

}
