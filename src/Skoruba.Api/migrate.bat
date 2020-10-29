dotnet ef migrations  add init --context AdminIdentityDbContext -o Migrations/Npgsql/identity
::dotnet ef migrations  add init --context AdminConfigurationDbContext -o Migrations/Npgsql/configs
::dotnet ef migrations  add init --context AdminPersistedGrantDbContext -o Migrations/Npgsql/grants
::dotnet ef migrations  add init --context AdminLogDbContext -o Migrations/Npgsql/logs
::dotnet ef migrations  add init --context AdminAuditLogDbContext -o Migrations/Npgsql/audits
::dotnet ef migrations  add init --context DataProtectionDbContext -o Migrations/Npgsql/dataProtections


::dotnet ef database update --context ConfigurationDbContext
::dotnet ef database update --context PersistedGrantDbContext
::dotnet ef database update --context ApplicationDbContext