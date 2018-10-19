import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertifyService.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelEvent = new EventEmitter();
  model: any = { };
  constructor(private authService: AuthService, private alertifyservice: AlertifyService) { }

  ngOnInit() {
  }

  register() {
    console.log(this.model);
    this.authService.register(this.model).subscribe(success => {
      this.alertifyservice.success('registered successfully'); },
      error => this.alertifyservice.error(error));
  }
  cancel() {
    this.cancelEvent.emit(false);
  }
}
