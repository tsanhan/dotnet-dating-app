import { User } from "./user";

//1. we make it params so we could give it initial values
export class UserParams {
  gender: string;
  minAge = 18;
  maxAge = 99;
  pageNumber = 1;
  pageSize = 5;


  constructor(user: User) {
    this.gender = user.gender === 'female' ? 'male' : 'female';
  }
  //2. make use of this, go to members.service.ts
}
