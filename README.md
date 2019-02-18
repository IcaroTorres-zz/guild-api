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
**Disclaimer**
> + _All actions below fails with BadRequest if not receiving properly formatted requests, avoiding the need of adding this as possible return of each described action in this section._
> + _All responses are formated as JSON_

#### Actions, Methods and Endpoints. ([API] present below correspond to https://[server]/lumen.api).
| Action | Method | Endpoint URI format | Example |
| -------| -------| --------------------------------| -------------| 
| CreateUser | PUT | [API]/createuser/{username} | [API]/createuser/icaro torres |
| UserInfo | GET | [API]/userinfo/{username} | [API]/userinfo/icaro torres |
| CreateGuild | POST | [API]/createguild | [API]/createguild/ |
| GuildInfo | GET | [API]/guildinfo/{guildname} | [API]/guildinfo/myguild |
| Guilds | GET | [API]/guilds/{count=20} | [API]/guilds/100 |
| EnterGuild | PUT | [API]/enterguild/{guildname} | [API]/enterguild/myguild |
| LeaveGuild | DELETE | [API]/leaveguild/{guildname} | [API]/leaveguild/myguild |
| Transfer | PATCH | [API]/transfer/{guildname} | [API]/transfer/myguild/ |

> **CreateUser**
> Receives 1 params `(string username)` to create a new one and return it.
+ _Return a new User object with Created response status (201)._
```js
{
  "Id": [string], //username as the model's key
  "GuildId":  [string] // guildname's ForeignKey
  "IsGuildMaster": [nullable boolean],
  "Guild": [nullable Guild object representation]
}
```
> **UserInfo**
> Receives 1 params `(string username)` to get an existing user and return it.
+ _Return an User object got with Ok response status (200)._
```js
{
  "Id": [string], //username as the model's key
  "GuildId":  [string] // guildname's ForeignKey
  "IsGuildMaster": [nullable boolean],
  "Guild": [nullable Guild object representation]
}
```
> **CreateGuild**
> Receives 1 json formatted param with two properties to create a new Guild as following:
``` js
{
  "Id": [string], // representing the guild's name
  "MasterId": [string] // representing the guildmaster's name
}
```
> if the user do not already exists, creates a brand new one setting it as a member and guildmaster of the resulting new Guild.
> Fails if guild already exists.
+ _Return a new Guild object with Created response status (201)._

> **GuildInfo**
> Receives 1 param `(string guildname)` display a JSON Object with the guildname, guildmaster and a list of members.
> Fails if the guild do not exists.
> if fail, returns an object containing `{ "error":  "guild not found."}`.
+ _Return a new Guild object with OK response status (200)._
``` js
{
  "Id": [string], // guildname as the model's key
  "MasterId": [string], // guildmaster's name as ForeignKey 
  "Master": [User object representation] 
  "members" : [
      //... a list of User objects
  ]
}
```

> **Guilds**
> Receives 1 _optional_ param `(int count)` to return the Nth first Guilds or no params to return the first 20ths.
+ _Return a list of guilds found_.
``` js
// list of guilds found
[
  {
    "Id": [string], // guildname as the model's key
    "MasterId": [string], // guildmaster's name as ForeignKey 
    "Master": [User object representation] 
    "members" : [
      //... a list of User objects
    ]
  },
  // . . .
]
```

> **EnterGuild**
> Receives 2 params `(string guildname, string username)` to insert a new member to a guild.
>> Fails if: 
>+ guild or user not found;
>+ inserting a user currently in other guild;
>+ user already in the guild.
+ _Return a boolean corresponding the success status_.

> **LeaveGuild**
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
  public string Id { get; set; }
  public string GuildId { get; set; }
  public virtual Guild Guild { get; set; }
  public bool? IsGuildMaster { get => Guild?.MasterId.Equals(Id); }
}
```

#### Guild.cs
``` c#
public class Guild
{
  public string Id { get; set; }
  public string MasterId { get; set; }
  public virtual User Master { get; set; }
  public virtual ICollection<User> Members { get; set; }
}
```

#### LumenContext
``` c#
public class LumenContext : DbContext
{
  public DbSet<Guild> Guilds { get; set; }
  public DbSet<User> Users { get; set; }
  public LumenContext(DbContextOptions<LumenContext> options) : base(options) { }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    // explicitly needed to map this one-sided navigation property on Guild Entity
    modelBuilder.Entity<Guild>()
      .HasOne(g => g.Master)
      .WithOne() 
      .HasForeignKey<Guild>(g => g.MasterId);
      // the foreignKey here is needed cause there is no navigation property on the other relation size
  }
}
```

### Dependency Injection Setup (Startup.cs)
> This turns the IoC possible and the dependencies available to be injected (used on this project as Constructor type), On HomeController, Repositories and UnitOfWork.
> The Inmemory Provider is also setup here to register our context.
```c#
public void ConfigureServices(IServiceCollection services)
{
  // enabling UseLazyLoadingProxies, requires AddJsonOptions to handle navigation reference looping on json serialization
  services.AddMvc()
          .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
          .AddJsonOptions(options => options.SerializerSettings
                                            .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
  // your context dependency registration
  services.AddEntityFrameworkInMemoryDatabase()
          .AddDbContext<LumenContext>(options => options.UseLazyLoadingProxies()
                                                        .UseInMemoryDatabase("lumenInMemoryDB"));
  // your repositories and unit of work dependecy registration
  services.AddTransient<IGuildRepository, GuildRepository>(); 
  services.AddTransient<IUserRepository, UserRepository>();
  services.AddTransient<IUnitOfWork, UnitOfWork>();                             
}   
```
