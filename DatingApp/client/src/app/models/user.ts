export interface User {
  username: string;
  token: string;
  photoUrl:string;
  // 1. missing knownAs here btw, this means I didn't used anywhere, i wanted to use it in the nav bar, but didn't 🤔.
  // 2. lets fix this for a sec. go to nav.component.html
  knownAs: string;

  gender: string;//3. add this
}
