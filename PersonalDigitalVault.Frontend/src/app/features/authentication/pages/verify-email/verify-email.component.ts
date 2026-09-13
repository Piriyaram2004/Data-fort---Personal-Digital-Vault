import {
  ChangeDetectorRef,
  Component,
  OnInit,
  inject
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-verify-email',
  standalone: true,
  imports: [],
  templateUrl: './verify-email.html',
  styleUrl: './verify-email.css'
})
export class VerifyEmailComponent implements OnInit {

  private route = inject(ActivatedRoute);
  private authService = inject(AuthService);
  private cdr = inject(ChangeDetectorRef);

  message = 'Verifying your email...';
  isLoading = true;
  isSuccess = false;

  ngOnInit(): void {

    console.log('VERIFY COMPONENT LOADED');

    const token = this.route.snapshot.queryParamMap.get('token');

    console.log('TOKEN:', token);

    if (!token) {
      this.isLoading = false;
      this.message = 'Invalid verification link.';

      this.cdr.detectChanges();
      return;
    }

    console.log('CALLING VERIFY API');

    this.authService.verifyEmail(token).subscribe({

      next: (response) => {

        console.log('VERIFY SUCCESS:', response);

        this.isLoading = false;
        this.isSuccess = true;
        this.message =
          response.message || 'Email verified successfully.';

        this.cdr.detectChanges();
      },

      error: (err) => {

        console.log('VERIFY ERROR:', err);

        this.isLoading = false;
        this.isSuccess = false;
        this.message =
          err.error?.message ||
          'Invalid or expired verification link.';

        this.cdr.detectChanges();
      }
    });
  }
}