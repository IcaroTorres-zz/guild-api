# Guild.api
Sample API Developed with 
+ **.Net Core 2.2**;
+ **EF Core 2**;
+ **Microsoft.EntityFrameworkCore.Inmemory** package;
+ **Dependency Injection**;
+ **Repository Pattern**.

# Setup

### Requirements
+ [.NetCore SDK](https://dotnet.microsoft.com/download "microsoft downloads");
+ [GIT](https://git-scm.com/downloads "git downloads").

On prompt:
```
$ git clone https://github.com/icarotorres/guild.api.git
$ dotnet restore
$ dotnet run
```
If you are using VS Code, configure your VS Code Debugger with _.vscode_ folder on your project root folder, pres `F5` and select `.Net Core Launch (web)` as your running target option. It will ask to create a build task, generating a file like following.

tasks.json:
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
                "${workspaceFolder}/guild.api.csproj"
            ],
            "problemMatcher": "$msCompile"
        }
    ]
}
```

Compile project with `dotnet build` and Publish production folder with `dotnet publish`.

# Resources
#### Disclaimer
+ All responses are formated as JSON;
+ [API] Will represent the root api URL. Example: *http://localhost:5000/api*.

## Guilds
### Creating a guild.
+ Method: `POST`
+ URI: `[API]/guilds`
+ Controller Action: `CreateGuild`
+ Params:
  + name: string
  + masterName: string
+ Sample request:
```
$ curl -i -X POST http://localhost:5000/api/guilds \
-d '{"name": "a", "masterName": "u1"}' \
-H 'Content-type: application/json'
```
> Expected output sample:
> + Response header
```
HTTP/1.1 201 Created
Date: <Response DateTime>
Content-Type: application/json; charset=utf-8
Server: Kestrel
Transfer-Encoding: chunked
Location: api/guilds/a
```
> + Response body
```
{
	"name":"a",
	"masterName":"u1",
	"master":{"name":"u1","guildName":"a","isGuildMaster":true},
	"members":[{"name":"u1","guildName":"a","isGuildMaster":true}]
}
```

---

### Guild Info.
+ Method: `GET`
+ URI: `[API]/guilds/:name`
+ Controller Action: `GuildInfo`
+ Params: `name: string`
+ Sample request:
```
$ curl -i http://localhost:5000/api/guilds/a
```

> Expected output sample:
> + Response header
```
HTTP/1.1 200 OK
Date: <Response DateTime>
Content-Type: application/json; charset=utf-8
Server: Kestrel
Transfer-Encoding: chunked
```
> + Response body
```
{
	"master":{"name":"u2","guildName":"a","isGuildMaster":true},
	"members":[
		{"name":"u2","guildName":"a","isGuildMaster":true},
		{"name":"u1","guildName":"a","isGuildMaster":false}
	],
	"name":"a",
	"masterName":"u2"
}
```

---

### List Guilds.
+ Method: `GET`
+ URI: `[API]/guilds/list/:count=20`
+ Controller Action: `Guilds`
+ Params: `count: int`
+ Sample request:
```
$ curl -i http://localhost:5000/api/guilds/list/3
```

> Expected output sample:
> + Response header
```
HTTP/1.1 200 OK
Date: <Response DateTime>
Content-Type: application/json; charset=utf-8
Server: Kestrel
Transfer-Encoding: chunked
```
> + Response body
```
[
	{
		"master":{"name":"u2","guildName":"a","isGuildMaster":true},
		"members":[
			{"name":"u2","guildName":"a","isGuildMaster":true},
			{"name":"u1","guildName":"a","isGuildMaster":false}
		],
		"name":"a",
		"masterName":"u2"
	},
	{
		"master":{"name":"u3","guildName":"b","isGuildMaster":true},
		"members":[{"name":"u3","guildName":"b","isGuildMaster":true}],
		"name":"b",
		"masterName":"u3"
	},
	{
		"master":{"name":"u5","guildName":"c","isGuildMaster":true},
		"members":[{"name":"u5","guildName":"c","isGuildMaster":true}],
		"name":"c",
		"masterName":"u5"
	}
]
```

---
### Guild Update.
+ Method: `PUT`
+ URI: `[API]/guilds/:name`
+ Controller Action: `UpdateGuild`
+ Params:
  + `name: string`
  + `payload: GuildForm`
+ Sample request:
```
$ curl -i -X PUT http://localhost:5000/api/guilds/a \
-d '{"name": "a", "masterName": "u2", "members": ["u1", "u2"]}' \
-H 'Content-type: application/json'
```
> Expected output sample:
> + Response header
```
HTTP/1.1 200 OK
Date: <Response DateTime>
Content-Type: application/json; charset=utf-8
Server: Kestrel
Transfer-Encoding: chunked
```
> + Response body
```
{
  "name":"a",
  "masterName":"u2",
  "master":{
    "name":"u2",
    "guildName":"a",
    "isGuildMaster":true
  },
  "members":[
    {
      "name":"u1",
      "guildName":"a",
      "isGuildMaster":false
    },
    {
      "name":"u2",
      "guildName":"a",
      "isGuildMaster":true
    }
  ]
}
```
---
### Add member to guild.
+ Method: `PATCH`
+ URI: `[API]/guilds/:name`
+ Controller Action(s): `PatchGuild > UpdateMembers`
+ Params:
  + `name: string`
  + `payload: {"addedMember": string}`
+ Sample request:
```
$ curl -i -X PATCH http://localhost:5000/api/guilds/a \
-d '{"addedMember": "u9"}' \
-H 'Content-type: application/json'
```
> Expected output sample:
> + Response header
```
HTTP/1.1 200 OK
Date: <Response DateTime>
Content-Type: application/json; charset=utf-8
Server: Kestrel
Transfer-Encoding: chunked
```
> + Response body
```true```
---

### Remove member from guild.
+ Method: `PATCH`
+ URI: `[API]/guilds/:name`
+ Controller Action(s): `PatchGuild > UpdateMembers`
+ Params:
  + `name: string`
  + `payload: {"removedMember": string}`
+ Sample request:
```
$ curl -i -X PATCH http://localhost:5000/api/guilds/a \
-d '{"removedMember": "u9"}' \
-H 'Content-type: application/json'
```
> Expected output sample:
> + Response header
```
HTTP/1.1 200 OK
Date: <Response DateTime>
Content-Type: application/json; charset=utf-8
Server: Kestrel
Transfer-Encoding: chunked
```
> + Response body

```true```

---

### Transfer guild master ownership.
+ Method: `PATCH`
+ URI: `[API]/guilds/:name`
+ Controller Action(s): `PatchGuild > Transfer`
+ Params:
  + `name: string`
  + `payload: {"newMasterName": string}`
+ Sample request:
```
$ curl -i -X PATCH http://localhost:5000/api/guilds/a \
-d '{"newMasterName": "u3"}' \
-H 'Content-type: application/json'
```
> Expected output sample:
> + Response header
```
HTTP/1.1 200 OK
Date: <Response DateTime>
Content-Type: application/json; charset=utf-8
Server: Kestrel
Transfer-Encoding: chunked
```
> + Response body

```true```

---

### Delete Guild.
+ Method: `DELETE`
+ URI: `[API]/guilds/:name`
+ Controller Action(s): `DeleteGuild`
+ Params: `name: string`
+ Sample request:
```
$ curl -i -X DELETE http://localhost:5000/api/guilds/a
```
> Expected output sample:
> + Response header
```
HTTP/1.1 200 OK
Date: <Response DateTime>
Content-Type: application/json; charset=utf-8
Server: Kestrel
Transfer-Encoding: chunked
```
> + Response body

```true```

---

### Models
#### User.cs
```c#
public class User
{
  [Key]
  public string Name { get; set; }
  public string GuildName { get; set; }
  [ForeignKey("GuildName")]
  public virtual Guild Guild { get; set; }
  public bool IsGuildMaster { get => Guild?.MasterName?.Equals(Name) ?? false; }
}
```

#### Guild.cs
```c#
public class Guild
{
  [Key]
  public string Name { get; set; }
  public string MasterName { get; set; }
  [ForeignKey("MasterName")]
  public virtual User Master { get; set; }
  [InverseProperty("Guild")]
  public virtual ICollection<User> Members { get; set; }
}
```

### DTOs
#### UserForm.cs
```c#
public class UserForm
{
  public string Name { get; set; }
  public string GuildName { get; set; }
}
```

#### GuildForm.cs
```c#
public class GuildForm
{
  public string Name { get; set; }
  public string MasterName { get; set; }
  public List<string> Members { get; set; }
}
```

### ApiContext
```c#
public class ApiContext : DbContext
{
  public DbSet<Guild> Guilds { get; set; }
  public DbSet<User> Users { get; set; }
  public ApiContext(DbContextOptions<ApiContext> options): base(options){}  
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    // explicitly needed to map this one-sided navigation property on Guild Entity
    modelBuilder.Entity<Guild>()
      .HasOne(g => g.Master)
      .WithOne()
      .HasForeignKey<Guild>(g => g.MasterName);
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
          .AddDbContext<ApiContext>(options => options.UseLazyLoadingProxies()
                                                        .UseInMemoryDatabase("ApiInMemoryDB"));
  // your repositories and unit of work dependecy registration
  services.AddTransient<IGuildRepository, GuildRepository>(); 
  services.AddTransient<IUserRepository, UserRepository>();
  services.AddTransient<IUnitOfWork, UnitOfWork>();                             
}   
```