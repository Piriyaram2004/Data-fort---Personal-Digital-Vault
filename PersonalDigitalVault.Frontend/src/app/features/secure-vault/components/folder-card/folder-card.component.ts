import { Component, EventEmitter, Input, Output } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Folder } from '../../models/folder.model';

@Component({
  selector: 'app-folder-card',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './folder-card.component.html',
  styleUrl: './folder-card.component.css'
})
export class FolderCardComponent {
  @Input() folder!: Folder;
  @Output() edit = new EventEmitter<Folder>();
  @Output() delete = new EventEmitter<string>();

  onEdit(event: Event): void {
    event.stopPropagation();
    this.edit.emit(this.folder);
  }

  onDelete(event: Event): void {
    event.stopPropagation();
    this.delete.emit(this.folder.id);
  }
}
