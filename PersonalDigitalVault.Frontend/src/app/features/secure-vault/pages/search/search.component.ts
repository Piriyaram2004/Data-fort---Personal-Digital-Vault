import {
  Component,
  OnDestroy,
  OnInit,
  inject
} from '@angular/core';

import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

import {
  EMPTY,
  Subject,
  Subscription,
  debounceTime,
  distinctUntilChanged,
  switchMap
} from 'rxjs';

import {
  SearchService,
  VaultSearchResults
} from '../../services/search.service';

import { LoadingComponent } from '../../../../shared/components/loading/loading.component';
import { EmptyStateComponent } from '../../../../shared/components/empty-state/empty-state.component';

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [
    FormsModule,
    RouterLink,
    LoadingComponent,
    EmptyStateComponent
  ],
  templateUrl: './search.component.html',
  styleUrl: './search.component.css'
})
export class SearchComponent implements OnInit, OnDestroy {

  private searchService = inject(SearchService);

  private searchSubject = new Subject<string>();
  private searchSubscription?: Subscription;

  searchQuery = '';
  isLoading = false;
  hasSearched = false;
  errorMessage = '';

  results: VaultSearchResults = {
    folders: [],
    documents: [],
    credentials: []
  };

  ngOnInit(): void {

    this.searchSubscription = this.searchSubject
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),

        switchMap(searchTerm => {

          if (!searchTerm) {
            this.isLoading = false;
            return EMPTY;
          }

          this.isLoading = true;
          this.errorMessage = '';

          return this.searchService.searchVault(searchTerm);
        })
      )
      .subscribe({
        next: (results: VaultSearchResults) => {

          this.results = results;
          this.isLoading = false;
        },

        error: (error) => {

          console.error('Failed to search vault:', error);

          this.results = {
            folders: [],
            documents: [],
            credentials: []
          };

          this.isLoading = false;
          this.errorMessage = 'Unable to search the vault.';
        }
      });
  }

  onSearchInput(value: string): void {

    this.searchQuery = value;

    const searchTerm = value.trim();

    /*
     * If the user removes the search text or
     * enters fewer than 2 characters:
     * - clear old results
     * - hide search results
     * - cancel the previous search request
     */
    if (searchTerm.length < 2) {

      this.hasSearched = false;
      this.isLoading = false;
      this.errorMessage = '';

      this.results = {
        folders: [],
        documents: [],
        credentials: []
      };

      // Cancels any previous pending/in-flight search
      this.searchSubject.next('');

      return;
    }

    this.hasSearched = true;

    this.searchSubject.next(searchTerm);
  }

  onSearch(): void {

    const searchTerm = this.searchQuery.trim();

    if (searchTerm.length < 2) {
      return;
    }

    this.hasSearched = true;

    this.searchSubject.next(searchTerm);
  }

  ngOnDestroy(): void {
    this.searchSubscription?.unsubscribe();
  }
}