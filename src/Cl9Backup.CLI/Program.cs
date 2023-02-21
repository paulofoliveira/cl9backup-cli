using Cl9Backup.CLI.Domain.Persistence;
using Cl9Backup.CLI.Infrastructure.Persistence;
using LiteDB;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Cl9Backup.CLI
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var services = new ServiceCollection()
                                .AddSingleton(c => new LiteDatabase(Constants.DATABASE_NAME))
                                .AddSingleton<ICredencialRepository, CredencialRepository>()
                                .AddSingleton(PhysicalConsole.Singleton)
                                .BuildServiceProvider();

            try
            {
                var app = new CommandLineApplication<CliCommands>();
                app.Conventions.UseDefaultConventions().UseConstructorInjection(services);
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
