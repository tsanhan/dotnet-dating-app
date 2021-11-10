exporting to a shared module:
the app.module.ts can become really large, lets see what we can do about it.

- we'll create a shared module (for stuff being shared between other modules)
- we'll create a feature module (for feature related stuff)
    * we want to enable lazy loading (we download the code only when needed)


* create modules folder
* create a 'members' module there
* create a 'shared' module there
* go to shared.module.ts:
* go to members.module.ts:
