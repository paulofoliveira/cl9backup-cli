using Cl9Backup.CLI.Domain.Entities;
using Cl9Backup.CLI.Domain.Persistence;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;

namespace Cl9Backup.CLI
{
    [Command(Name = "cl9backup", Description = "Executa rotinas de backup e restore seguindo configurações em sua área gerenciado em https://backup.cl9.cloud/")]
    [HelpOption(Description = "Mostra informações de ajuda")]
    public class CliCommands
    {
        private readonly IConfiguration _configuration;
        private readonly IParametroRepository _parametroRepository;
        public CliCommands(IConfiguration configuration, IParametroRepository parametroRepository)
        {
            _configuration = configuration;
            _parametroRepository = parametroRepository;
        }

        [Option("-c|--config", CommandOptionType.NoValue, Description = "Realiza as configurações necessárias para execuções de Backup/Restore. Serve também como \"reset\" caso precise atualizar as configuraçoes", ShowInHelpText = true)]
        public bool Config { get; set; }

        [Option("-b|--backup", CommandOptionType.NoValue, Description = "Executa operação de Backup conforme as configurações de destino, fonte e dispositivo", ShowInHelpText = true)]
        public bool Backup { get; set; }

        [Option("-d|--destination", CommandOptionType.SingleValue, Description = "Nome do Destino (Bucket)", ShowInHelpText = true)]
        public string? Destination { get; set; }

        [Option("-s|--source", CommandOptionType.SingleValue, Description = "Nome da Fonte do Backup", ShowInHelpText = true)]
        public string? Source { get; set; }

        [Option("-dv|--device", CommandOptionType.SingleValue, Description = "Nome do dispositivo conectado", ShowInHelpText = true)]
        public string? Device { get; set; }

        public Task<int> OnExecute(CommandLineApplication app, IConsole console)
        {
            var isConfigured = _parametroRepository.IsConfigured();

            if (!isConfigured || Config)
            {
                var apiHost = string.Empty;
                var login = string.Empty;
                var senha = string.Empty;

                console.WriteTitle("Configuração do CL9 Backup");

                while (string.IsNullOrEmpty(apiHost))
                {
                    apiHost = Prompt.GetString("Api HOST:", _configuration["DefaultApiHost"]);

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

                if (isConfigured)
                {
                    console.WriteLine("Limpando parâmetros antigos...");
                    _parametroRepository.ClearCollection();
                }

                console.WriteLine($"Armazenando {(isConfigured ? "novas " : "")}configurações no CL9 Backup...");

                _parametroRepository.Add(new Parametro() { Nome = "API_HOST", Valor = apiHost });
                _parametroRepository.Add(new Parametro() { Nome = "LOGIN", Valor = login });
                _parametroRepository.Add(new Parametro() { Nome = "PASSWORD", Valor = senha });

                console.WriteLine($"Configurações armazenadas!");

                return Task.FromResult(Constants.OK);
            }

            if (Backup)
            {
                console.WriteTitle("Execução de Backup do CL9 Backup");

                while (string.IsNullOrEmpty(Destination))
                {
                    Destination = Prompt.GetString("Destino:");
                    if (string.IsNullOrEmpty(Destination))
                        console.WriteLine($"O campo Destino é obrigatório.");
                }

                while (string.IsNullOrEmpty(Source))
                {
                    Source = Prompt.GetString("Fonte:");
                    if (string.IsNullOrEmpty(Source))
                        console.WriteLine($"O campo Fonte é obrigatório.");
                }

                while (string.IsNullOrEmpty(Device))
                {
                    Device = Prompt.GetString("Dispositivo:");
                    if (string.IsNullOrEmpty(Device))
                        console.WriteLine($"O campo Dispositivo é obrigatório.");
                }

                // TODO: Autenticar na API para obtenção da SessionKey.
                // TODO: Chamar endpoint GetProfileAndHash para recuperar valores de Destination, Source e Device.

                console.WriteLine($"Executando backup para {Destination} da fonte {Source} através do dispositivo {Device}...");
            }

            return Task.FromResult(Constants.OK);
        }
    }
}


