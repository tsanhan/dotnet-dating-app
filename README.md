install ef package:

1. install 'nuget gallery' vscode extension.
2. shift + ctrl + p => nuget gallery => search Microsoft.EntityFrameworkCore.Sqlite
    * check out that there is SQLServer there too, if you want to use it it's ok, nothing will change in the code (ef loosely coupling the app code from the DB)
    * in API.csproj we'll see a new PackageReference
