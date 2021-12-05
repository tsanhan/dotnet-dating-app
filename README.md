Client side registration:
lets start with the register.component.ts, go there.

ok lets test in the browser the full registration process.
look our for:
1. what happens to the user's photo (we don't have one)
2. do we have the user details
3. does the photo upload work on first photo?
4. does the photo updates everywhere it needs to be updated?

ok so go to the registration page:
1. fill the form and register
2. the navbar has no image and no default image... 
 lets fix this: go to nav.component.html
3. try to edit the users's profile, cool works!.
4. upload a first image, it;s being uploaded but not updated in the different locations in the site...
    * after refresh we have the main iimage in the 'your profile' main image but not on top
    * to fix this go to photo-editor.component.ts in initializeUploader method, after we uploaded the image
5. after fixing 4 register a new user and upload a new first image, it works!

up next: section summery.