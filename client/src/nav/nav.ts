import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../core/services/account-service';

@Component({
  selector: 'app-nav',
  imports: [FormsModule],
  templateUrl: './nav.html',
  styleUrl: './nav.css'
})
export class Nav {
  private accountService = inject(AccountService);
  protected creds: any = {};
  protected isLoggedIn = signal(false);

  login() {
    this.accountService.login(this.creds).subscribe({
      next: response => {
        console.log(response);
        this.isLoggedIn.set(true);
        this.creds = {};
      },
      error: error => alert(error.message)
    });
  }

  logout() {
    this.isLoggedIn.set(false);
    console.log('Logged out');
  }
}
