Adding a paged list class:

so what are we going to be page re response in user repository's GetMembersAsync method.
but we wont be adding .Skip() or .Take() to to this method because it win't be DRY.

so we;ll create a class file PagedList.cs in the helpers folder, go there.

up next: adding a helper class to add the data from PagedList.cs to the response.

