import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SearchService, VaultSearchResults } from '../../services/search.service';
import { FolderCardComponent } from '../../components/folder-card/folder-card.component';
import { DocumentCardComponent } from '../../components/document-card/document-card.component';
import { CredentialCardComponent } from '../../components/credential-card/credential-card.component';
import { LoadingComponent } from '../../../../shared/components/loading/loading.component';
import { EmptyStateComponent } from '../../../../shared/components/empty-state/empty-state.component';

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [
    FormsModule,
    FolderCardComponent,
    DocumentCardComponent,
    CredentialCardComponent,
    LoadingComponent,
    EmptyStateComponent
  ],
  templateUrl: './search.component.html',
  styleUrl: './search.component.css'
})
export class SearchComponent {
  private searchService = inject(SearchService);

  searchQuery = '';
  isLoading = false;
  hasSearched = false;
  results: VaultSearchResults = {
    folders: [],
    documents: [],
    credentials: []
  };

  onSearch(): void {
    if (!this.searchQuery.trim()) return;

    this.isLoading = true;
    this.hasSearched = true;

    this.searchService.searchVault(this.searchQuery.trim()).subscribe({
      next: (res) => {
        this.isLoading = false;
        if (res.success && res.data) {
          this.results = res.data;
        } else {
          this.results = { folders: [], documents: [], credentials: [] };
        }
      },
      error: () => {
        this.isLoading = false;
        this.results = { folders: [], documents: [], credentials: [] };
      }
    });
  }
}
