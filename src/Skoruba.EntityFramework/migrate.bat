dotnet ef migrations  add init --context AdminIdentityDbContext -o Migrations/Npgsql
dotnet ef migrations  add init --context AdminConfigurationDbContext -o Migrations/Npgsql
dotnet ef migrations  add init --context AdminPersistedGrantDbContext -o Migrations/Npgsql
dotnet ef migrations  add init --context AdminLogDbContext -o Migrations/Npgsql
dotnet ef migrations  add init --context AdminAuditLogDbContext -o Migrations/Npgsql
dotnet ef migrations  add init --context DataProtectionDbContext -o Migrations/Npgsql


::dotnet ef database update --context ConfigurationDbContext
::dotnet ef database update --context PersistedGrantDbContext
::dotnet ef database update --context ApplicationDbContext