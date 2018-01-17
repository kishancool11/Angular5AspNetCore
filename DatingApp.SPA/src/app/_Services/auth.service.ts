import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { Observable } from 'rxjs/Observable';
import { tokenNotExpired, JwtHelper } from 'angular2-jwt';

@Injectable()
export class AuthService {
    userToken: any;
    decodedToken: any;
    jwtHelper: JwtHelper = new JwtHelper();

    baseUrl = 'http://localhost:5000/api/auth/';
    constructor(private http: Http) {}

 login(model: any) {
     return this.http.post(this.baseUrl + 'login', model, this.requestOptions()).map((response: Response) => {
            const user = response.json();
            if (user) {
                localStorage.setItem('token', user.token);
                this.decodedToken = this.jwtHelper.decodeToken(user.token);
                console.log(this.decodedToken);
                this.userToken = user.token;
            }
        }).catch(this.handleError);
 }

 register(model: any) {
    return this.http.post(this.baseUrl + 'register', model, this.requestOptions()).catch(this.handleError);
 }

 loggedIn() {
     return tokenNotExpired('token');
 }

 private requestOptions() {
    const headers = new Headers({'content-type' : 'application/json'});
    return new RequestOptions({headers : headers});
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