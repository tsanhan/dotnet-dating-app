import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  //1. populate the base url from the environment file
  baseUrl = environment.apiUrl;

  constructor(private http:HttpClient) { }


  //2. create a method to get all users
  getUsersWithRoles(){
    return this.http.get<Partial<User[]>>(this.baseUrl + 'admin/users-with-roles');
  }
  //3. go to the user-management.component.ts
}
