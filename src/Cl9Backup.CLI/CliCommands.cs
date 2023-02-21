using Cl9Backup.CLI.Domain.Entities;
using Cl9Backup.CLI.Domain.Persistence;
using McMaster.Extensions.CommandLineUtils;

namespace Cl9Backup.CLI
{
    [Command(Name = "cl9backup", Description = "Executa rotinas de backup e restore seguindo configurações em sua área gerenciado em https://backup.cl9.cloud/")]
    [HelpOption(Description = "Mostra informações de ajuda")]
    public class CliCommands
    {
        private readonly ICredencialRepository _credencialRepository;
        public CliCommands(ICredencialRepository credencialRepository)
        {
            _credencialRepository = credencialRepository;
        }

        [Option("-l|--login:[<COMMAND>]", CommandOptionType.SingleOrNoValue, Description = "Gerencia credenciais para execução do processo de Login nas chamadas da API", ShowInHelpText = true)]
        public (bool HasValue, string Command) Login { get; set; } = (false, Constants.Commands.Logins.List);
        public Task<int> OnExecute(CommandLineApplication app, IConsole console)
        {
            if (Login.HasValue)
            {
                if (string.IsNullOrEmpty(Login.Command) || Login.Command.Equals(Constants.Commands.Logins.List))
                {
                    console.WriteTitle("Listar Credenciais Cadastradas");

                    var credenciais = _credencialRepository.GetAll();

                    if (!credenciais.Any())
                        console.WriteLine("Credenciais não encontradas!");
                    else
                        foreach (var item in credenciais.Select((credencial, index) => new { index, credencial }))
                            console.WriteLine($"{item.index} - {item.credencial.Nome} - {item.credencial.Email}");

                    return Task.FromResult(Constants.OK);
                }
                else if (Login.Command.Equals(Constants.Commands.Logins.Add))
                {
                    var nome = string.Empty;
                    var email = string.Empty;
                    var senha = string.Empty;

                    console.WriteTitle("Adicionar Credencial no CL9 Backup");

                    while (string.IsNullOrEmpty(nome))
                    {
                        nome = Prompt.GetString("Nome:");

                        if (string.IsNullOrEmpty(nome))
                            console.WriteLine($"O campo Nome é obrigatório.");
                        else
                        {
                            if (_credencialRepository.ExistByName(nome))
                            {
                                console.WriteLine($"O campo Nome já foi cadastrado em outra credencial. Tente com outro Nome.");
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

                    while (string.IsNullOrEmpty(senha))
                    {
                        senha = Prompt.GetPassword("Senha:");

                        if (string.IsNullOrEmpty(senha))
                            console.WriteLine($"O campo Senha é obrigatório.");
                    }

                    var login = new Credencial(nome, email, senha);
                    console.WriteLine($"Armazenando credenciais para \"{login.Nome}\"...");

                    _credencialRepository.Add(login);

                    console.WriteLine($"Credencial \"{login.Nome}\" armazenada!");

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
