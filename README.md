# Guild.api

> A simple Rest API made in .net core, with operations (_GET, POST, PUT, PATCH, DELETE_) over `Guild` URI _/api/guilds/v1_

## Sample API Developed with :

+ [x]**.Net Core 2.2**;
+ [x]**EF Core 2.2.6**;
+ [x]**Microsoft.EntityFrameworkCore.Sqlite 2.2.6** package;
+ [x]**Dependency Injection**;
+ [x]**Repository Pattern**;
+ [x]**Unit of Work**;
+ []**Hateoas**;
+ []**Distributed Cache (Redis)**.

## Setup

### Requirements:

+ [.NetCore SDK](https://dotnet.microsoft.com/download "microsoft downloads");
+ [GIT](https://git-scm.com/downloads "git downloads");
+ [Redis](https://redis.io/download "redis downloads").


### Clone and Restore

```
$ git clone https://github.com/icarotorres/guild.api.git
$ dotnet restore --project Api
```
### Running the project

> If you are using VS Code, configure your VS Code Debugger with _.vscode_ folder on your project root folder, pres `F5` and select `.Net Core Launch (web)` as your running target option. It will ask to create a build task, generating a file like following.

tasks.json:
``` json
{
    "version": "2.0.0",
    "tasks": [
        {
            "label": "build",
            "command": "dotnet",
            "type": "process",
            "args": [
                "build",
                "${workspaceFolder}/guild.api.csproj"
            ],
            "problemMatcher": "$msCompile"
        }
    ]
}
```

> Compile project with `dotnet build` and Publish production folder with `dotnet publish`.

```
$ redis-server
$ dotnet run
```

## Aditional Setup

### Redis Installation (Non Windows)

> Download, extract and compile Redis with:

```
$ wget http://download.redis.io/releases/redis-5.0.7.tar.gz
$ tar xzf redis-5.0.7.tar.gz
$ cd redis-5.0.7
$ make
```

> The binaries that are now compiled are available in the src directory. Run Redis with:

```
$ src/redis-server
```

### Redis Installation (Windows)

> Download a compiled windows version from [dmajkic / redis](https://github.com/dmajkic/redis/downloads "github dmajkic/redis download packages"). Set Redis on your environment variables and run `redis-server` on prompt like below:

```
$ redis-server
```

> this will start your Redis local server with default settings.

### Adding Redis to your project

> Configure an entry for your settings in your pppsetings.json. Following there is an example:

```json
{
  //...
  "RedisCacheSettings": {
    // other settings...
    "ConnectionString": "localhost,port: 6379,password=your_redis_password!"
  },
  //...
}
```

### Sqlite

> To add Sqlite to the project you need to register on your dependenci injection services through the method `ConfigureServices` in the `Startup.cs`.
You can do it in many ways. Following there is an example on how to do so:

```c#
// ...
	public void ConfigureServices(IServiceCollection services)
	{
		services
			// ... other services registratins

			// DbContext dependency registration
			.AddEntityFrameworkSqlite()
			.AddDbContext<YourContext>(options => options.UseSqlite(yourSqlConnectionString));
			// ... other services registration
	}
// ...

```

> Examples of ConnectionStrings for absolute and relative paths:

```c#
	// example for using absolute path using some apsettings key:value congifs
	var SqliteAbsolutePathConnectionString = $"Data Source={AppHost.ContentRootPath}\\{Configuration["SqliteSettings:SourceName"]}";

	// example for using relative path of running application (bin/Debug/.../someName.Db)
	var SqliteRelaivePathConnectionString = Configuration["SqliteSettings:ConnectionString"];
```
