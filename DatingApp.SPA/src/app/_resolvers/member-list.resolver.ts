import { Resolve, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { User } from '../_models/User';
import { Injectable } from '@angular/core';
import { UserService } from '../_Services/User.service';
import { AlertifyService } from '../_Services/alertify.service';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/of';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class MemberListResolver implements Resolve<User[]> {
    constructor(private userService: UserService, private router: Router,
        private alertify: AlertifyService) {    }

        resolve(route: ActivatedRouteSnapshot): Observable<User[]> {
            return this.userService.getUsers().catch(errror => {
                this.alertify.error('Problem recieving data');
                this.router.navigate(['/members']);
                return Observable.of(null);
            });
        }
}
