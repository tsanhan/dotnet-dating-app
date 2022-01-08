import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;

  constructor(private http:HttpClient) { }


  getUsersWithRoles(){
    return this.http.get<Partial<User[]>>(this.baseUrl + 'admin/users-with-roles');
  }

  // 1. implement the function to update the roles
  updateUserRoles(username: string, roles: string[]) {
    return this.http.post(`${this.baseUrl}admin/edit-roles/${username}?roles=${roles}`, {});
  }
  // 2. go to user-management.component.ts
}
