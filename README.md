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
| User | GET | [API]/user/[username] | [API]/user/icaro torres |
| Create | POST | [API]/createguild | [API]/createguild/ |
| Guilds | GET | [API]/guilds | |
| Guilds | GET | [API]/guilds/[count] | [API]/guilds/100 |
| Info | GET | [API]/guildinfo/[guildname] | [API]/guildinfo/myguild |
| Info | GET | [API]/userinfo/[username] | [API]/userinfo/myguild |
| Enter | GET | [API]/enterguild/[guildname]/[username] | [API]/enterguild/myguild/john doe |
| Leave | GET | [API]/leaveguild/[username]/[guildname] | [API]/leaveguild/john doe/myguild |
| Transfer | GET | [API]/transfer/[guildname]/[username] | [API]/transfer/myguild/jane doe |

> **Disclaimer**
> _All actions below fails with BadRequest if not receiving properly formatted requests, avoiding the need to add this as possible return of each described action in this section._

> **CreateGuild**
> Receives 1 json formatted param with two properties to create a new Guild as following:
``` js
{
  Id: [string], // representing the guild's name
  MasterId: [string] // representing the guildmaster's name
}
```
> if the user do not already exists, creates a brand new one setting it as a member and guildmaster of the resulting new Guild.
> Fails if guild already exists.
+ _Return the new Guild object with Created response._

> **Guilds**
> Receives 1 _optional_ param `(int count)` to return the Nth first Guilds or no params to return the first 20ths.
+ _Return a list of guilds found_.

> **UserInfo**
> Receives 1 params `(string username)` to check if user exists, else create a new one.
+ _Return a key-value pair object like one of following options, depending on operation success:_.
```js
{ "user created": [User Object Representation] }
//or
{ "user found": [User Object Representation] }
//or
{ "error":  "Fails on user [{username}]." }
```

> **GuildInfo**
> Receives 1 param `(string guildname)` display an JSON Object with the guildname, guildmaster and a list of members.
> Fails if the guild do not exists.
> if fail, returns an object containing `{ "error":  "guild not found."}`.
+ _Return expected_.
``` js
{
  "guild": {
    "name": [string guildname],
    "guildmaster": [string master name],
    "members" : [
      //... a list of member names
    ]
  }
}
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
