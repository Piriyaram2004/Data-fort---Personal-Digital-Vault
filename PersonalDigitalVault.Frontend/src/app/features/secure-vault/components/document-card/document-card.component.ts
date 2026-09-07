import { Component, EventEmitter, Input, Output } from '@angular/core';
import { RouterLink } from '@angular/router';
import { DocumentItem } from '../../models/document.model';
import { FileSizePipe } from '../../../../shared/pipes/file-size.pipe';
import { IntegrityBadgeComponent } from '../integrity-badge/integrity-badge.component';

@Component({
  selector: 'app-document-card',
  standalone: true,
  imports: [RouterLink, FileSizePipe, IntegrityBadgeComponent],
  templateUrl: './document-card.component.html',
  styleUrl: './document-card.component.css'
})
export class DocumentCardComponent {
  @Input() document!: DocumentItem;
  @Output() download = new EventEmitter<DocumentItem>();
  @Output() delete = new EventEmitter<string>();

  onDownload(event: Event): void {
    event.stopPropagation();
    this.download.emit(this.document);
  }

  onDelete(event: Event): void {
    event.stopPropagation();
    this.delete.emit(this.document.id);
  }
}
