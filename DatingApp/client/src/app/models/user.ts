export interface User {
  username: string;
  token: string;
  photoUrl:string;
  knownAs: string;
  gender: string;

  //1. add a property for roles
  roles: string[];
  //2. go to fix the typing issue in jwt.interceptor.ts
  //3. go back to account.service.ts, point 4
}
