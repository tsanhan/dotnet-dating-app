import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root' //1. an injectable singleton (does not destroys until we close our app)
})
export class AccountService {

  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) { }

  login(model: any){
    return this.http.post(this.baseUrl + 'account/login', model);
  }
}
