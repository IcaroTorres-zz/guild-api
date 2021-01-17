# Guild.api

# Description

A REST API with resources representing Guilds, Members, Invites and Memberships, developed in .Net Core including:

- [x] **.Net Core 3.1.3**;
- [x] **Microsoft.EntityFrameworkCore**;
- [x] **Microsoft.EntityFrameworkCore.SQLite** package;
- [x] **Authentication and JWT**;
- [x] **Domain-Driven Design**;
- [x] **Repositories**;
- [x] **Unit of Work**;
- [x] **Null Objects**;
- [x] **FluentValidations**;
- [x] **MediatR Request/Response Pipelines**;
- [x] **HATEOAS**;
- [x] **Cache**.
- [x] **Hateoas**;
- [x] **Distributed Cache (Redis)**.

# Table of Contents

1. **[Title](#guildapi "Title")**
2. **[Description](#description "Description")**
3. **[Table of Contents](#table-of-contents "Table of Contents")**
4. **[Installation](#installation "Installation")**

    1. **[Requirements](#requirements "Requirements")**
    2. **[Clone and Restore](#clone-and-restore "Clone and Restore")**
    3. **[Redis Installation (Non Windows)](#redis-installation-non-windows "Redis Installation (Non Windows)")**
    4. **[Redis Installation (Windows)](#redis-installation-windows "Redis Installation (Windows)")**
    5. **[Redis in the project](#redis-in-the-project "Redis in the project")**
    6. **[SQLite in the project](#sqlite-in-the-project "SQLite in the project")**

5. **[Usage](#usage "Usage")**
6. **[Contributing](#contributing "Contributing")**
7. **[Credits](#credits "Credits")**

# Installation

## Requirements:

This project require installation of some other tools. You need to have **[GIT](https://git-scm.com/downloads "git downloads")** to clone this repo, install **[.Net Core SDK](https://dotnet.microsoft.com/download "microsoft downloads")** to work with .net cross-platform development environment and use `dotnet cli` commands to restore the project and get all packages and dependencies needed properly installed, including **[EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/ "nuget gallery")**, **[SQLite](https://www.nuget.org/packages/System.Data.SQLite "nuget gallery")** and **[StackExchangeRedis](https://www.nuget.org/packages/Microsoft.Extensions.Caching.StackExchangeRedis "nuget gallery")**. At last but not least, you will need **[Redis](https://redis.io/download "redis downloads")** installed on your system and run it before starts the project execution.

## Clone and Restore

The restore command will provide installations for needed packages.

    $ dotnet restore Application

## Redis Installation (Non Windows)

Download, extract somewhere you want and compile Redis with:

    $ wget http://download.redis.io/releases/redis-5.0.7.tar.gz
    $ tar xzf redis-5.0.7.tar.gz
    $ cd redis-5.0.7
    $ make

The binaries that are now compiled are available in the src directory. Run Redis with:

    $ src/redis-server

## Redis Installation (Windows)

 - You can download it directly from **[Redis](https://redis.io/download "redis downloads")** official downloads page, compile with **[Make](http://gnuwin32.sourceforge.net/packages/make.htm "Make page")** for Windows similarly to linux installation;
 - Acquire it using **[Chocolatey](https://chocolatey.org/install "Chocolatey")** and installing Redis-64 package with `choco install redis-64` in Powershell;
 - Get a compiled Windows version from [dmajkic / redis](https://github.com/dmajkic/redis/downloads "github dmajkic/redis download packages") and set Redis on your environment variables, and use following command to run a basic configuration of redis on prompt like below:
 
 		$ redis-server

This will start your Redis local server with default settings.

## Redis in the project

Configure an entry for your settings in your `appsettings.json`. Following there is an example:

```json
{
  "RedisCacheSettings": {
    "ConnectionString": "localhost,port: 6379,password=your_redis_password!",
    "Enabled": true
  }
}
```

## SQLite in the project

To add SQLite to the project you need to register on your dependency injection services through the method `ConfigureServices` in the `Startup.cs`.

You can do it in many ways. Following there is an example on how to do so:

Startup.cs

```c#
public void ConfigureServices(IServiceCollection services)
{
    services
      .AddDbContext<YourContext>(options => options.UseSQLite(yourSqlConnectionString));
}
```

You can use `appsettings.json` to set absolute or relative paths for SQLite. If you want to get the absolute path of your application host, you can change default `Startup` class constructor adding the `IWebHostEnvironment` as parameter like hereinafter:

Startup.cs

```c#
public Startup(IConfiguration configuration, IWebHostEnvironment env)
{
    Configuration = configuration;
    Environment = env;
}

public IConfiguration Configuration { get; }
public IHostingEnvironment Environment { get; }
```

And alter the context registration in the `ConfigureServices` method of `Startup.cs` class. Code below is mounting sql connection using `Environment.ContentRootPath` and `Configuration` key/value from `appsettings.json`.


Startup.cs

```c#
var SQLiteAbsolutePathConnectionString = $"Data Source={Environment.ContentRootPath}\\{Configuration["SQLiteSettings:SourceName"]}";

services.AddDbContext<YourContext>(options => options.UseSQLite(SQLiteAbsolutePathConnectionString));
```

#  Usage

If you are using VS Code, configure your VS Code Debugger with _.vscode_ folder on your project root folder, pres `F5` and select `.Net Core Launch (web)` as your running target option. It will ask to create a build task, generating a file like following.

tasks.json:

```json

{
  "version": "2.0.0",
  "tasks": [
    {
      "label": "build",
      "command": "dotnet",
      "type": "process",
      "args": [
        "build",
        "${workspaceFolder}/Application/Application.csproj"
      ],
      "problemMatcher": "$msCompile"
    }
  ]
}
```

You can **Compile** project with `dotnet build Application` and **Publish** production folder with `dotnet publish Application`.

To run, start your Redis server instance and run your project like example below:

> Using windows environment variable or accessing your compiled src directory.

    $ redis-server

> Inside the project directory.

    $ dotnet run Application

# Contributing

Feel free to *Fork* this repo and send a *Pull Request* with your ideas and improvements, turning this proof of concept any better.

# Credits

This project was conceived by me, [@icarotorres](https://github.com/icarotorres "author's profile") : icaro.stuart@gmail.com, then owner of this repository.
