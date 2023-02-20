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
    [HelpOption(Description = "Mostra informações de ajuda")]
    public class CliCommands
    {
        [Option("-l | --login", CommandOptionType.NoValue, Description = "Guarda credenciais para execução do processo de Login nas chamadas da API", ShowInHelpText = true)]
        public bool Login { get; set; }
        public Task<int> OnExecute(CommandLineApplication app, IConsole console)
        {
            if (Login)
            {
                var name = string.Empty;
                var login = string.Empty;
                var password = string.Empty;

                console.WriteLine($"***** Credenciais do CL9 Backup *****");

                while (string.IsNullOrEmpty(name))
                {
                    name = Prompt.GetString("Nome:");

                    if (string.IsNullOrEmpty(name))
                        console.WriteLine($"O campo Nome é obrigatório.");
                }

                while (string.IsNullOrEmpty(login))
                {
                    login = Prompt.GetString("Login:");

                    if (string.IsNullOrEmpty(login))
                        console.WriteLine($"O campo Login é obrigatório.");
                }

                while (string.IsNullOrEmpty(password))
                {
                    password = Prompt.GetPassword("Senha:");

                    if (string.IsNullOrEmpty(password))
                        console.WriteLine($"O campo Senha é obrigatório.");
                }

                console.WriteLine($"Armazenando credenciais...");

                // TODO: Persiste na Local Storage.

                return Task.FromResult(OK);
            }

            return Task.FromResult(OK);
        }
    }
}