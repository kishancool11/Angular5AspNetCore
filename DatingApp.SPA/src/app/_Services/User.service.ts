import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.prod';
import { Http, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { Observable } from 'rxjs/Observable';
import { User } from '../_models/User';
import { AuthHttp } from 'angular2-jwt';

@Injectable()
export class UserService {
baseUrl = environment.apiUrl;

constructor(private authHttp: AuthHttp) { }

    getUsers(): Observable<User[]> {
        return this.authHttp
        .get(this.baseUrl + 'users')
        .map( reponse => <User[]>reponse.json())
        .catch(this.handleError);
    }

    getUser(): Observable<User> {
        return this.authHttp
        .get(this.baseUrl + 'users')
        .map( reponse => <User[]>reponse.json())
        .catch(this.handleError);
    }

    private handleError(error: any) {
        const applicationError = error.headers.get('Application-Error');
        if (applicationError) {
            return Observable.throw(applicationError);
        }
        const serverError = error.json();
        let modelStateErrors = '';
        if (serverError) {
            for (const key in serverError) {
                if (serverError[key]) {
                    modelStateErrors += serverError[key] + '\n';
               }
            }
        }
        return Observable.throw(modelStateErrors || 'Server error');
    }
}

