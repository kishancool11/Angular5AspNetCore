import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_Services/auth.service';
import { error } from 'selenium-webdriver';
import { AlertifyService } from '../_Services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  photoUrl: string;
  model: any = {};
  constructor(public authSerive: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.authSerive.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
  }

  login() {
   this.authSerive.login(this.model)
   .subscribe(data => {
     this.alertify.success('logged in successfully');
    }, e => {
      this.alertify.error(e);
    }, () => {
      this.router.navigate(['/members']);
    });
  }

  logout() {
    this.authSerive.userToken = null;
    localStorage.removeItem('token');
    this.authSerive.currentUser = null;
    localStorage.removeItem('user');
    this.alertify.message('logged out');
    this.router.navigate(['/home']);
  }

  loggedIn() {
    return this.authSerive.loggedIn();
  }
}
