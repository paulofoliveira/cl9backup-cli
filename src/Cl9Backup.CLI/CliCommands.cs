using Cl9Backup.CLI.Domain.Entities;
using Cl9Backup.CLI.Domain.Persistence;
using McMaster.Extensions.CommandLineUtils;

namespace Cl9Backup.CLI
{
    [Command(Name = "cl9backup", Description = "Executa rotinas de backup e restore seguindo configurações em sua área gerenciado em https://backup.cl9.cloud/")]
    [HelpOption(Description = "Mostra informações de ajuda")]
    public class CliCommands
    {
        private readonly IParametroRepository _parametroRepository;
        public CliCommands(IParametroRepository parametroRepository)
        {
            _parametroRepository = parametroRepository;
        }

        [Option("-c|--config", CommandOptionType.NoValue, Description = "Realiza as configurações necessárias para execuções de Backup/Restore", ShowInHelpText = true)]
        public bool Config { get; set; }

        public Task<int> OnExecute(CommandLineApplication app, IConsole console)
        {
            if (!_parametroRepository.IsConfigured() || Config)
            {
                var apiHost = string.Empty;
                var login = string.Empty;
                var senha = string.Empty;

                console.WriteTitle("Configurar no CL9 Backup");

                while (string.IsNullOrEmpty(apiHost))
                {
                    // TODO: Puxar defaultValue do appsettings.json

                    apiHost = Prompt.GetString("Api HOST:", "https://localhost:44376/");

                    if (string.IsNullOrEmpty(apiHost))
                        console.WriteLine($"O campo Api HOST é obrigatório.");
                }

                while (string.IsNullOrEmpty(login))
                {
                    login = Prompt.GetString("Login:");

                    if (string.IsNullOrEmpty(login))
                        console.WriteLine($"O campo Login é obrigatório.");
                }

                while (string.IsNullOrEmpty(senha))
                {
                    senha = Prompt.GetPassword("Senha:");

                    if (string.IsNullOrEmpty(senha))
                        console.WriteLine($"O campo Senha é obrigatório.");
                }

                console.WriteLine($"Armazenando configurações de CL9 Backup...");

                _parametroRepository.Add(new Parametro() { Nome = "API_HOST", Valor = apiHost });
                _parametroRepository.Add(new Parametro() { Nome = "LOGIN", Valor = login });
                _parametroRepository.Add(new Parametro() { Nome = "PASSWORD", Valor = senha });

                console.WriteLine($"Configurações armazenadas!");

                return Task.FromResult(Constants.OK);
            }

            return Task.FromResult(Constants.OK);
        }
    }
}
