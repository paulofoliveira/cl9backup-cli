using Cl9Backup.CLI.Domain.Entities;
using Cl9Backup.CLI.Domain.Persistence;
using McMaster.Extensions.CommandLineUtils;

namespace Cl9Backup.CLI
{
    [Command(Name = "cl9backup", Description = "Executa rotinas de backup e restore seguindo configurações em sua área gerenciado em https://backup.cl9.cloud/")]
    [HelpOption(Description = "Mostra informações de ajuda")]
    public class CliCommands
    {
        private readonly ILoginRepository _loginRepository;
        public CliCommands(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        [Option("-l|--login:[<COMMAND>]", CommandOptionType.SingleOrNoValue, Description = "Gerencia credenciais para execução do processo de Login nas chamadas da API", ShowInHelpText = true)]
        public (bool HasValue, string Command) Login { get; set; } = (false, Constants.Commands.Logins.List);
        public Task<int> OnExecute(CommandLineApplication app, IConsole console)
        {
            if (Login.HasValue)
            {
                if (string.IsNullOrEmpty(Login.Command) || Login.Command.Equals(Constants.Commands.Logins.List))
                {
                    console.WriteTitle("Listar Logins Cadastrados");

                    var logins = _loginRepository.GetAll();

                    if (!logins.Any())
                        console.WriteLine("Logins não encontrados!");
                    else
                        foreach (var item in logins.Select((login, index) => new { index, login }))
                            console.WriteLine($"{item.index} - {item.login.Email}");

                    return Task.FromResult(Constants.OK);
                }
                else if (Login.Command.Equals(Constants.Commands.Logins.Add))
                {
                    var nome = string.Empty;
                    var email = string.Empty;
                    var password = string.Empty;

                    console.WriteTitle("Credenciais do CL9 Backup");

                    while (string.IsNullOrEmpty(nome))
                    {
                        nome = Prompt.GetString("Nome:");

                        if (string.IsNullOrEmpty(nome))
                            console.WriteLine($"O campo Nome é obrigatório.");
                        else
                        {
                            if (_loginRepository.ExistByName(nome))
                            {
                                console.WriteLine($"O campo Nome já foi cadastrado em outro login. Tente com outro Nome.");
                                nome = string.Empty;
                            }
                        }
                    }

                    while (string.IsNullOrEmpty(email))
                    {
                        email = Prompt.GetString("Email:");

                        if (string.IsNullOrEmpty(email))
                            console.WriteLine($"O campo Email é obrigatório.");
                    }

                    while (string.IsNullOrEmpty(password))
                    {
                        password = Prompt.GetPassword("Senha:");

                        if (string.IsNullOrEmpty(password))
                            console.WriteLine($"O campo Senha é obrigatório.");
                    }

                    var login = new Login(nome, email, password);
                    console.WriteLine($"Armazenando credenciais para \"{login.Nome}\"...");

                    _loginRepository.Add(login);

                    console.WriteLine($"Credencial para \"{login.Nome}\" armazenada!");

                    return Task.FromResult(Constants.OK);
                }
                else
                {
                    console.WriteLine("Nenhum comando foi encontrado para --login. Verifique a documentação.");
                    return Task.FromResult(Constants.OK);
                }
            }

            return Task.FromResult(Constants.OK);
        }
    }
}
