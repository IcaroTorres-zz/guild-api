# lumen.api
## Guild API using .netCore 2.2 and EF Core Inmemory Provider
___
### The API
> Developed with .Net Core 2.2, EF Core 2, Microsoft.EntityFrameworkCore.Inmemory package, Repository Pattern and Dependency Injection.

### Setup

Install [.NetCore SDK ](https://dotnet.microsoft.com/download "microsoft downloads")

in command prompt with .net CLI and git installed:
```
git clone https://github.com/icarotorres/lumen.api.git
dotnet restore
dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 2.2.1	
```


### Content

#### Actions, Methods and Endpoints
| Action | Method | Endpoint URI format |
| -------| -------| -------------------|
| Create | POST | https://[server]/lumen.api/guild/create |
| Guilds | GET | https://[server]/lumen.api/guild/guilds |
| Guilds | GET | https://[server]/lumen.api/guild/guilds?count=[int] |
| Info | GET | https://[server]/lumen.api/guild/info?guildname=[string] |
| Enter | PUT | https://[server]/lumen.api/guild/enter?guildname=[string]&username=[string] |
| Leave | DELETE | https://[server]/lumen.api/guild/leave?username=[string]&guildname=[string] |
| Transfer | PUT | https://[server]/lumen.api/guild/leave?guildname=[string]&username=[string] |
