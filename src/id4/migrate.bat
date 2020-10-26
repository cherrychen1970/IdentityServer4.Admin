dotnet ef migrations  add init --context NpgsqlApplicationDbContext -o Migrations/Npgsql/users
dotnet ef migrations  add init --context NpgsqlConfigurationDbContext -o Migrations/Npgsql/configs
dotnet ef migrations  add init --context NpgsqlPersistedGrantDbContext -o Migrations/Npgsql/grants

::dotnet ef database update --context ConfigurationDbContext
::dotnet ef database update --context PersistedGrantDbContext
::dotnet ef database update --context ApplicationDbContext