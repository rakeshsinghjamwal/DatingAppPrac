import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertifyService.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};
  constructor(public authService: AuthService, private alertifyService: AlertifyService, private router: Router ) { }

  ngOnInit() {}

  login() {
    this.authService.login(this.model).subscribe( next => {
      this.alertifyService.success('Logged in successfully');
    }, error => {
      console.log(error);
      this.alertifyService.error(error);
    },
    () => this.router.navigate(['/members']));
  }

  IsLoggedIn() {
    return this.authService.loggedIn();
  }

  Logout() {
    localStorage.removeItem('token');
    this.alertifyService.message('logged out');
    this.router.navigate(['/home']);
  }
}
