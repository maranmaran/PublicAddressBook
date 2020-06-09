import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MessageType } from '@microsoft/signalr';
import { MediaType } from 'src/models/enums/media-type.enum';

@Component({
  selector: 'app-media-dialog',
  templateUrl: './media-dialog.component.html',
  styleUrls: ['./media-dialog.component.scss']
})
export class MediaDialogComponent implements OnInit {

  constructor(
    private dialogRef: MatDialogRef<MediaDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { type: MessageType | MediaType, sourceUrl: string }) { }

  mediaType = MediaType;

  ngOnInit() {
  }

}
