Switching the DB Server to PostGres:

* let's look at the providers that a re available in EF (https://docs.microsoft.com/en-us/ef/core/providers/?tabs=dotnet-core-cli):

* we can see the different options in the link above.
* we'll use PostgreSQL, because that's what Heroku gives us.
* now, to use PostgreSQL we won't install it, we'll use docker!
    * what is docker?
    * it kind of virtualization but with shared kenel (if you on linux)
    * it's actually a mini OS with nothing but what you need to run a specific app (postgres in our case)
    * [talk a bit about layers in docker]
    * some terminology:
        docker image: the actual image that you can run
        docker container: a running instance of a docker image (you can run multiple containers of the same image)
        docker hub: a free place where you can find docker images
        docker registry: a place where you can upload your images (docker hub is a registry)

* install docker for your OS here: https://docs.docker.com/install/
* get the postgres image from docker hub: https://hub.docker.com/_/postgres
    * we can see different versions of postgres to use (these are tags)
* make suer the docker daemon is running
* run the command `docker pull postgres`
* if we want the latest or a specific tag (version) we can use the command `docker pull postgres:<tag>`
* anyway, under 'start a postgres instance' we'll see a simple command to run:
* run `docker run --name postgres -p 5432:5432 -e POSTGRES_USER=appuser -e POSTGRES_PASSWORD=Pa$$w0rd -d postgres:latest`. lets break this down:
    * --name postgres: name the container postgres
    * -p 5432:5432: port mapping (5432 is the default port for postgres)
    * -e POSTGRES_USER=appuser: set the user to appuser
    * -e POSTGRES_PASSWORD=Pa$$w0rd: set the password to Pa$$w0rd
    * postgres:latest: run the image postgres:latest
    * -d: run container in detached mode, in background  (no terminal) 

* now, we can run the command `docker ps` to see the container


* we can also see in the docker app the container running
    * in the Inspect tab in the docker app we can see the environment variables.
    * we can see postgres don't like $ sign, so it changed it, we'll need to use THIS to login.

* ok so how can we see what's in the server (the tables themselves)? 
* well we have 2 options:
    1. use a vscode extension like we did with sqlite, easy!
    2. install a tool (something like ssms for postgres), it's called 'pgadmin'

    * for 2: you can download, install and use, you can also use another docker (https://hub.docker.com/r/dpage/pgadmin4) to have this app ad a docker container.

* the pgadmin is great but I'll stick with less work and just use the vscode extension (postgres)
* after install the postgres vscode extension I can Ctrl+Shift+P and select the 'PostgreSQL: New Connection'
    * use the data from the docker app for the connection (user and password)
    * the server in on localhost:5432

up next: Changing the DB Server in our app



