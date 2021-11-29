import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/models/member';
import { User } from 'src/app/models/user';
import { AccountService } from 'src/app/services/account.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {

  @Input() member: Member;

  //1. add properties
  uploader: FileUploader;
  hasBaseDropZoneOver: boolean = false;
  baseUrl = environment.apiUrl;
  user: User;


  constructor(private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  ngOnInit(): void {
    this.initializeUploader();
  }

  //2. add method
  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/add-photo',
      authToken: 'Bearer ' + this.user.token, // this is not using the interceptor internally, it's separate so we add token
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true, //remove from dropzone after upload
      autoUpload: false, //don't upload automatically, we'll make the user click a button
      maxFileSize: 10 * 1024 * 1024 //10mb max file size for the free tier (i think 🤔)
    });

    this.uploader.onAfterAddingFile = (file) => {
      // we need the file credentials to be false,
      // otherwise we'll need to change our API CORS config and allow credentials to go up with our request.
      //We don't need to because we're using the Bearer taken to send our credentials with this file and then.
      file.withCredentials = false;
    };

    //what we want to do after the upload is complete successfully
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const photo = JSON.parse(response);
        this.member.photos.push(photo);
      }
    };
  }

  //3. add method to set our dropzone (hasBaseDropZoneOver)
  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }



}
