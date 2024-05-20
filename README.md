## To properly configure the development environment

First, it's important to load the user secrets into the API project.

`Right-click -> TuPencaUy.Core.API -> Manage user secrets`

And it should look like this:
```json
{
  "ConnectionStrings": {
    "Platform": "Server={Server name};Database={Db name};User Id={User nickname};Password={User password};TrustServerCertificate=true;Trusted_Connection=True;MultipleActiveResultSets=true",
    "DefaultTenant": "Server={Server name};Database=Tenant_DB;User Id={User nickname};Password={User password};TrustServerCertificate=true;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "JwtSettings": {
    "Issuer": "http://localhost:4200",
    "Audience": "http://localhost:4200",
    "Key": "{Secret key}",
    "MinutesTokenLifeTime": 30
  },
  "Auth0": {
    "SecretKey": "{auth0 secret key}"
  } 
}
```
>  **Server name:** This is the name of the current SQL server, for example: *`localhost\\SQLEXPRESS`*
> 
>   **Db name:** This is the name your central database will take when it is created.

> [!IMPORTANT]
> It is important to ensure that the user has owner permissions on the server.

Then run it in Visual Studio and the API will be up and running.
