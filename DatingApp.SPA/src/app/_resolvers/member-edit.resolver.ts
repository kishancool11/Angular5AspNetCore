import { Resolve, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { User } from '../_models/User';
import { Injectable } from '@angular/core';
import { UserService } from '../_Services/User.service';
import { AlertifyService } from '../_Services/alertify.service';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/of';
import { Observable } from 'rxjs/Observable';
import { AuthService } from '../_Services/auth.service';

@Injectable()
export class MemberEditResolver implements Resolve<User> {
    constructor(private userService: UserService, private router: Router,
        private alertify: AlertifyService,
        private authServie: AuthService) {    }

        resolve(route: ActivatedRouteSnapshot): Observable<User> {
            return this.userService.getUser(this.authServie.decodedToken.nameid).catch(errror => {
                this.alertify.error('Problem recieving data');
                this.router.navigate(['/members']);
                return Observable.of(null);
            });
        }
}
