using Azure.Data.AppConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;

namespace CompanyEmployees.ContextFactory;

public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
{
    private readonly ConfigurationClient _configurationClient;
    public RepositoryContextFactory()
    {
        // Assuming the connection string to Azure AppConfig is stored in an environment variable
        var connectionString = Environment.GetEnvironmentVariable("AZURE_APPCONFIG_CONNECTIONSTRING");
        _configurationClient = new ConfigurationClient(connectionString);
    }
    public RepositoryContext CreateDbContext(string[] args)
    {
        // Fetch the SQL connection string from Azure AppConfig
        var sqlConnectionStringConfiguration = _configurationClient.GetConfigurationSetting("TestApp:Settings:ConnectionString");

        var builder = new DbContextOptionsBuilder<RepositoryContext>()
            .UseSqlServer(sqlConnectionStringConfiguration.Value.Value,
                b => b.MigrationsAssembly("CompanyEmployees"));

        return new RepositoryContext(builder.Options);
    }
}