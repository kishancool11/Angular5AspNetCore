import { Component, OnInit, EventEmitter, Input, Output } from '@angular/core';
import { AuthService } from '../_Services/auth.service';
import { AlertifyService } from '../_Services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};
  @Output() CancelRegister = new EventEmitter();
  constructor(private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }


  register() {
   this.authService.register(this.model).subscribe(() => {
    this.alertify.success('Register Success');
   }, error => {
    this.alertify.error(error);
   });
  }

  cancel() {
    this.CancelRegister.emit(false);
  }
}
