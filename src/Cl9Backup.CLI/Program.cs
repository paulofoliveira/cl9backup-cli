using Cl9Backup.CLI.Domain.Persistence;
using Cl9Backup.CLI.Infrastructure.Api;
using Cl9Backup.CLI.Infrastructure.Persistence;
using LiteDB;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Cl9Backup.CLI
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                               .AddJsonFile("appsettings.json")
                               .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json")
                               .AddEnvironmentVariables()
                               .Build();

            var services = new ServiceCollection()
                                .AddSingleton(c => new LiteDatabase(Constants.DATABASE_NAME))
                                .AddSingleton<IParametroRepository, ParametroRepository>()
                                .AddSingleton(PhysicalConsole.Singleton)
                                .AddSingleton(configuration)
                                .AddSingleton(new JsonSerializerOptions() { PropertyNamingPolicy = null });

            services.AddHttpClient<Cl9BackupApiClient>((sp, client) =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var hostUri = new Uri(config["ApiHost"]);
                var baseAddress = new Uri(hostUri, "api/v1");
                client.BaseAddress = baseAddress;
            });

            var serviceProvider = services.BuildServiceProvider();

            try
            {
                var app = new CommandLineApplication<CliCommands>();
                app.Conventions.UseDefaultConventions().UseConstructorInjection(serviceProvider);
                return await app.ExecuteAsync(args);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine(ex.Message);
                Console.ResetColor();
                return Constants.EXCEPTION;
            }
        }
    }
}
