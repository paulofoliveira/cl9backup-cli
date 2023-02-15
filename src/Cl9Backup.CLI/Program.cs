using McMaster.Extensions.CommandLineUtils;

public class Program
{
    public const int EXCEPTION = 1;
    public const int OK = 0;

    public static async Task<int> Main(string[] args)
    {
        try
        {
            return await CommandLineApplication.ExecuteAsync<CliCommands>(args);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(ex.Message);
            Console.ResetColor();
            return EXCEPTION;
        }
    }

    [Command(Name = "cl9backup", Description = "Executa rotinas de backup e restore seguindo configurações em sua área gerenciado em https://backup.cl9.cloud/")]
    [HelpOption]
    public class CliCommands
    {
        public Task<int> OnExecute(CommandLineApplication app, IConsole console)
        {
            return Task.FromResult(OK);
        }
    }
}