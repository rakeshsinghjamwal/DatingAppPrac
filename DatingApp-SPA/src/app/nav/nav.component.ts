import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertifyService.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};
  constructor(public authService: AuthService, private alertifyService: AlertifyService ) { }

  ngOnInit() {}

  login() {
    this.authService.login(this.model).subscribe( next => {
      this.alertifyService.success('Logged in successfully');
    }, error => {
      console.log(error);
      this.alertifyService.error(error);
    });
  }

  IsLoggedIn() {
    return this.authService.loggedIn();
  }

  Logout() {
    localStorage.removeItem('token');
    this.alertifyService.message('logged out');
  }
}
