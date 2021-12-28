import { Injectable } from '@angular/core';
import {
  Router, Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot
} from '@angular/router';
import { Observable, of } from 'rxjs';
import { Member } from '../models/member';
import { MembersService } from '../services/members.service';

@Injectable({
  providedIn: 'root'
})
export class MemberDetailedResolver implements Resolve<Member> {//1. generic type: Member, we want to get Member

  //2. inject the member service
  constructor(private membersService: MembersService) { }


  //2. implement Resolve interface
  resolve(route: ActivatedRouteSnapshot):Observable<Member> {
    //3. we'll return an observable. no need to subscribe (the router will take care of that)
    return this.membersService.getMember(route.paramMap.get('username') as string);
  }
  //4. we add the resolver to a routing configuration.
  //  * go to members.module.ts

}
