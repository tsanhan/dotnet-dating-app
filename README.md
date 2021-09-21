
we'll create a folder named DatingApp and get into it:
`mkdir DatingApp`
`cd DatingApp`

we'll start with a check if out dotnet install was successful:
`dotnet --info`: all SDKs and runtime installed
'Host' is the version we'll be using
'.NET SDK' is the sdk we'll be using

as long we have dotnet core 5 we r fine!

- create .net api project:
`dotnet -h` for help
- we'll bw using options `new` and `sln`

`dotnet new -h` will show us the help for this command.
we see `-l` that interests me... so
`dotnet new -l` will show all the templates available in dotnet 5

first we want to create a sln file (a container for our projects) and a project template too.

now some confusion about dotnet naming:
- .net framework is the old .net (windows only) and it's ended on 4 (microsoft abandoning it)
- .net core (all platforms) came and started on version 1 then 2 and 3 (the future of .net)
- so microsoft just renamed .net core => .net 5 (less confusion 🤔)
- the templates are still named in .net core but in the future i believe they fix this.

ok so new solution file:
`dotnet new sln`: containing folder name => name of the solution file;
vscode is opening a folder but vs can do more use on the sln file.

`dotnet new webapi -o API` (-o: output folder)
`dotnet sln add API`: to add our api project into out solution
