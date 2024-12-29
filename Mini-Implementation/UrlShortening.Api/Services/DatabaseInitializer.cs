
using Dapper;
using Npgsql;

namespace URL_Shortener.Services;

public class DatabaseInitializer(
    NpgsqlDataSource dataSource,
    IConfiguration configuration,
    ILogger<DatabaseInitializer> logger
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await CreateDatabaseIfNotExists(stoppingToken);
            await InitializeSchema(stoppingToken);
            logger.LogInformation("Database initialization completed successfully");
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Error initializing database");
            throw;
        }
    }

    private async Task CreateDatabaseIfNotExists(CancellationToken cancellationToken)
    {
        var connectionString = configuration.GetConnectionString("url-shortener");
        var builder = new NpgsqlConnectionStringBuilder(connectionString);
        string databaseName = builder.Database!;
        builder.Database = "postgres";

        await using var connection = new NpgsqlConnection(builder.ToString());
        await connection.OpenAsync(cancellationToken);

        bool databaseExist = await connection.ExecuteScalarAsync<bool>(
            "Select exists(Select 1 from pg_database where datname = @databaseName)", new { databaseName });

        if(!databaseExist)
        {
            logger.LogInformation("Creating database {DatabaseName}", databaseName);
            await connection.ExecuteAsync($"Create Database \"{databaseName}\"");
        }
    }

    private async Task InitializeSchema(CancellationToken cancellationToken)
    {
        const string createTableSql = """
            
            Create Table if not exists shortened_urls(
            id Serial Primary Key,
            short_code Varchar(10) Unique Not Null,
            original_url Text Not Null,
            created_at Timestamp with time zone default current_timestamp
            );
            
            Create Index If Not Exists idx_short_code On shortened_urls(short_code);
            
            Create Table If Not Exists url_visits(
            id Serial Primary Key,
            short_code Varchar(10) Not Null,
            visited_at Timestamp with time zone default current_timestamp,
            user_agent Text,
            referer Text,
            Foreign Key(short_code) Reference shortened_urls(short_code)
            );
            
            Create Index If Not Exists idx_visits_short_code On url_visits(short_code);
            
            """;
        await using var command = dataSource.CreateCommand(createTableSql);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
