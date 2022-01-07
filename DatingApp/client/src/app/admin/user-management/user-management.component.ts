import { AdminService } from './../../services/admin.service';
import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {

  //1. populate the users partial array
  users: Partial<User[]> = [];

  //2. inject the admin service
  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    //4. call the getUsersWithRoles() method
    this.getUsersWithRoles();
  }
  //3. a method for the getUsersWithRoles() method
  getUsersWithRoles() {
    this.adminService.getUsersWithRoles().subscribe((users: Partial<User[]>) => {
      this.users = users;
    });
  }
  //5. go to the html
}
