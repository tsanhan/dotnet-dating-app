import { User } from "./user";


export class UserParams {
  gender: string;
  minAge = 18;
  maxAge = 99;
  pageNumber = 1;
  pageSize = 5;
  orderBy = 'lastActive';//1. add this (this is also the default in the server)
  //2. go to member-list.component.html

  constructor(user: User) {
    this.gender = user.gender === 'female' ? 'male' : 'female';
  }

}
