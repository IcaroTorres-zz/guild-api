# lumen.api
## Guild API using .netCore 2.2 and EF Core Inmemory Provider
___
### The API
> Developed with .Net Core 2.2, EF Core 2, Microsoft.EntityFrameworkCore.Inmemory package, Repository Pattern and Dependency Injection.

### Setup

Install [.NetCore SDK ](https://dotnet.microsoft.com/download "microsoft downloads")

in command prompt with .net CLI and git installed:
>__(...See the official .Net Core Docs to advanced configuration on <projectname>.csproj to enhance compilation and enviroments options.)__
```
git clone https://github.com/icarotorres/lumen.api.git
dotnet restore
dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 2.2.1	

to compile files without running:
dotnet build
and publish production files:
dotnet publish

dotnet run
or configure your VS CODE Debugger with .vscode folder on project root:
```
#### tasks.json
``` js
{
    "version": "2.0.0",
    "tasks": [
        {
            "label": "build",
            "command": "dotnet",
            "type": "process",
            "args": [
                "build",
                "${workspaceFolder}/lumen.api.csproj"
            ],
            "problemMatcher": "$msCompile"
        }
    ]
}
```
___

### Content

#### Actions, Methods and Endpoints
| Action | Method | Endpoint URI format |
| -------| -------| -------------------|
| Index | GET | https://[server]/lumen.api/ |
| User | GET | https://[server]/lumen.api/user/[username] |
| Create | GET | https://[server]/lumen.api/create/[guildname]/[mastername] |
| Guilds | GET | https://[server]/lumen.api/guilds |
| Guilds | GET | https://[server]/lumen.api/guilds/[count] |
| Info | GET | https://[server]/lumen.api/info/[guildname] |
| Enter | GET | https://[server]/lumen.api/enter/[guildname]/[username] |
| Leave | GET | https://[server]/lumen.api/leave/[username]/[guildname] |
| Transfer | GET | https://[server]/lumen.api/transfer/[guildname]/[username] |

> **Index or / (bonus)**
> Receives no params .
> _Return a JSON representation of the current data in memory_.

> **User (bonus)**
> Receives 1 params `(string username)` to get an existing user or create a new one.
> _Return a string message about the success status_.

> **Create**
> Receives 2 params `(string guildname, string mastername)` to create a new Guild.
> if the user do not already exists, creates a brand new one setting it as a member and guildmaster of the resulting new Guild.
> Fails if guild already exists.
> _Return a boolean corresponding the the success status_.
___
> **Guilds**
> Receives 1 param `(int count)` to return the Nth first Guilds or no params to return the first 20ths.
> _Return a list of guilds found_.
___
> **Info**
> Receives 1 param `(string guildname)` display an JSON Object with the guildname, guildmaster and a list of members.
> Fails if the guild not exists.
> if fail, returns an object containing `{ "error":  "guild not found."}`.
> _Return expected_.
``` js
{
  "guild": {
    "name": "<the guildname>",
    "guildmaster": "<the guildmaster's name>",
    "members" : [
      //... a list of member names
    ]
  }
}
```
___
> **Enter**
> Receives 2 params `(string guildname, string username)` to insert a new member to a guild.
> Fails if: 
+ guild or user not found;
+ inserting a user currently in other guild;
+ user already in the guild.
> _Return a boolean corresponding the success status_.
___

> **Leave**
> Receives 2 params `(string username, string guildname,)` to remove a member from a guild.
> Fails if: 
+ guild or user not found;
+ trying to remove a user currently out of the guild;
+ user is guildmaster (nedding to Trasfer the guild ownership to other member) and is not the last one to quit the Guild.
> _Return a boolean corresponding the success status_.
___
> **Transfer (bonus)**
> Receives 2 params `(string guildname, string username,)` to pass the guild ownership position to another member from the guild.
> Need to be performed before a guildmaster leaves a guild.
> Fails if: 
+ guild or user not found;
+ trying to remove a user currently out of the guild;
+ user is guildmaster (nedding to Trasfer the guild ownership to other member).
> _Return a boolean corresponding the success status_.
___
### Model representation & Context
#### User.cs
``` c#
public class User
{
  [Key]
  public string Name { get; set; }
  public string GuildName { get; set; }
  public bool IsGuildMaster { get; set; }
}
```

#### Guild.cs
``` c#
public class Guild
{
  [Key]
  public string Name { get; set; }
  public string MasterName { get; set; }
  public HashSet<User> Members { get; set; }
}
```

#### LumenContext
``` c#
  public class LumenContext : DbContext
  {
    public DbSet<Guild> Guilds { get; set; }
    public DbSet<User> Users { get; set; }
    public LumenContext(DbContextOptions<LumenContext> options) : base(options) { }
    public LumenContext() {}
    protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseInMemoryDatabase("lumenInMemoryDB");
  }
```

### Dependency Injection Setup (Startup.cs)
> this turns the IoC possible and the dependencies available to be injected (used on this project as Constructor type), On HomeController, Repositories and UnitOfWork.
> The Inmemory Provider is also setup here to register our context.
```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

    services.AddEntityFrameworkInMemoryDatabase()
            .AddDbContext<LumenContext>((serviceProvider, options) => options.UseInMemoryDatabase("lumenInMemoryDB")
                                                                             .UseInternalServiceProvider(serviceProvider));
    services.AddTransient<IGuildRepository, GuildRepository>(); 
    services.AddTransient<IUserRepository, UserRepository>();
    services.AddTransient<IUnitOfWork, UnitOfWork>();                             
}   
```
