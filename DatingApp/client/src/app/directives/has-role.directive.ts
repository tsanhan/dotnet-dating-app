import { take } from 'rxjs/operators';
import { AccountService } from './../services/account.service';
import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { User } from '../models/user';

@Directive({
  //1. the selector is what we put in the element to bind the element to the directive
  selector: '[appHasRole]'//2. we'll be creating a structural directive so we'll use *appHasRole in the template
})
//11. get the time frame after we have access to the element and the class done constructing (implements OnInit)
export class HasRoleDirective implements OnInit {
  //9. we store the current user in a variable
  user: User;

  //10. now we'll be using this directive like this" *appHasRole="['Admin']"
  // * we want the client of this directive to pass in an array of roles
  // * we need this array to check against the user's roles
  // * the way this directive take an input property is by using the @Input decorator
  @Input() appHasRole: string[];


  //3. we'll inject some things we need to create a structural directive
  constructor(
    private viewContainerRef: ViewContainerRef, //4. if we look at the inline doc, we'll see it's a container for views.
    // in short this let us make the element a type of a hook for other html (templates) to added to or remove from

    private templateRef: TemplateRef<any>, //5. this is a reference to the template (a chunk of html) that's inside the element.
    //if we look at the inline doc, we'll see we can use it to create others like it.

    //6. remember that we removing or showing an element on the DOM (the Admin nav button)

    private accountService: AccountService //7. we need this service to check if the user has the role
  ) {
    // 8. get the current user
    this.accountService.currentUser$.pipe(
      take(1) // take 1 means we only want to take the first value, and that's it
      ).subscribe(user => {
      this.user = user;

    }
    );
  }
  ngOnInit(): void {
    //12. clear view if no roles or the user is not authenticated
    if(!this.user?.roles || this.user == null) {
      this.viewContainerRef.clear(); // simply clear the container from the views that can be in there
      return;
    }

    //13. check if the user has any of the roles passed in the directive via the appHasRole input property
    if (this.user.roles?.some(r => this.appHasRole.includes(r))) { //some is like Any in Linq
      //14. if they do, we'll show the element (the Admin nav button, what the templateRef is referencing)
      this.viewContainerRef.createEmbeddedView(this.templateRef);
    }

    //15. if they don't, we'll clear the container
    else {
      this.viewContainerRef.clear();
    }
    //16. lets use this, go to nav.component.html
  }


}
