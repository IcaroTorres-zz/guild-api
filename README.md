# lumen.api
## Guild API using .netCore 2.2 and EF Core Inmemory Provider

### The API
> **Sample API** Developed with **.Net Core 2.2**, **EF Core 2**, **Microsoft.EntityFrameworkCore.Inmemory** package, **Repository Pattern** and **Dependency Injection**, as a **Technical test** proposed by **[Lumen Games](https://lumen.games/ "Lumen Games")**.

### Setup

Install [.NetCore SDK ](https://dotnet.microsoft.com/download "microsoft downloads")
_(...See the official .Net Core Docs to advanced configuration on <projectname>.csproj to enhance compilation and enviroments options.)_

In command prompt with .net CLI and git installed:
```
git clone https://github.com/icarotorres/lumen.api.git
dotnet restore
dotnet run
```
If you are using VS Code, configure your VS Code Debugger with _.vscode_ folder on your project root folder, pres `F5` and select `.Net` as your running target option. It will ask you to create a build task, generating a file like the example below.

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

To compile files without running:
`dotnet build`
and publish production files:
`dotnet publish`
___

### Content

#### Actions, Methods and Endpoints. ([API] present below correspond to https://[server]/lumen.api).
| Action | Method | Endpoint URI format | Example |
| -------| -------| --------------------------------| -------------| 
| Index | GET | [API] | |
| User | GET | [API]/user/[username] | [API]/user/icaro torres |
| Create | GET | [API]/create/[guildname]/[mastername] |[API]/create/myguild/icaro torres |
| Guilds | GET | [API]/guilds | |
| Guilds | GET | [API]/guilds/[count] | [API]/guilds/100 |
| Info | GET | [API]/info/[guildname] | [API]/info/myguild |
| Enter | GET | [API]/enter/[guildname]/[username] | [API]/enter/myguild/john doe |
| Leave | GET | [API]/leave/[username]/[guildname] | [API]/john doe/myguild |
| Transfer | GET | [API]/transfer/[guildname]/[username] | [API]/transfer/myguild/jane doe |

> **Index or "/"**
> Receives no params .
+ _Return a JSON representation of the current data in memory_.

> **User**
> Receives 1 params `(string username)` to check if user exists, else create a new one.
+ _Return a string message about the success status_.

> **Create**
> Receives 2 params `(string guildname, string mastername)` to create a new Guild.
> if the user do not already exists, creates a brand new one setting it as a member and guildmaster of the resulting new Guild.
> Fails if guild already exists.
+ _Return a boolean corresponding the the success status_.

> **Guilds**
> Receives 1 param `(int count)` to return the Nth first Guilds or no params to return the first 20ths.
+ _Return a list of guilds found_.

> **Info**
> Receives 1 param `(string guildname)` display an JSON Object with the guildname, guildmaster and a list of members.
> Fails if the guild do not exists.
> if fail, returns an object containing `{ "error":  "guild not found."}`.
+ _Return expected_.
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
> **Enter**
> Receives 2 params `(string guildname, string username)` to insert a new member to a guild.
>> Fails if: 
>+ guild or user not found;
>+ inserting a user currently in other guild;
>+ user already in the guild.
+ _Return a boolean corresponding the success status_.

> **Leave**
> Receives 2 params `(string username, string guildname,)` to remove a member from a guild.
>> Fails if: 
>+ guild or user not found;
>+ trying to remove a user currently out of the guild;
>+ user is guildmaster (nedding to Trasfer the guild ownership to other member) and is not the last one to quit the Guild.
+ _Return a boolean corresponding the success status_.

> **Transfer**
> Receives 2 params `(string guildname, string username,)` to pass the guild ownership position to another member from the guild.
> Need to be performed before a guildmaster leaves a guild.
>> Fails if: 
>+ guild or user not found;
>+ trying to remove a user currently out of the guild;
>+ user is guildmaster (nedding to Trasfer the guild ownership to other member).
+ _Return a boolean corresponding the success status_.

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
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseInMemoryDatabase("yourInMemoryDbName");
  }
```

### Dependency Injection Setup (Startup.cs)
> This turns the IoC possible and the dependencies available to be injected (used on this project as Constructor type), On HomeController, Repositories and UnitOfWork.
> The Inmemory Provider is also setup here to register our context.
```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
    // your context dependency registration
    services.AddEntityFrameworkInMemoryDatabase()
            .AddDbContext<LumenContext>((serviceProvider, options)
            => options.UseInMemoryDatabase("lumenInMemoryDB")
                      .UseInternalServiceProvider(serviceProvider));
    
    // your repositories and unit of work dependecy registration
    services.AddTransient<IGuildRepository, GuildRepository>(); 
    services.AddTransient<IUserRepository, UserRepository>();
    services.AddTransient<IUnitOfWork, UnitOfWork>();                             
}   
```
