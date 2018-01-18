import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from '../../_models/User';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../../_Services/alertify.service';
import { NgForm } from '@angular/forms';
import { UserService } from '../../_Services/User.service';
import { AuthService } from '../../_Services/auth.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  user: User;
  @ViewChild('editForm') editForm: NgForm;

  constructor(private route: ActivatedRoute, private alertify: AlertifyService,
        private userService: UserService,
        private authService: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data  => {
      this.user = data['user'];
    });
  }


  updateuser() {
    this.userService.updateUser(
      this.authService.decodedToken.nameid, this.user)
          .subscribe(next => {
            this.alertify.success('Profile Updated Successfully');
            this.editForm.reset(this.user);
          }, ee => this.alertify.error(ee));
  }

}
