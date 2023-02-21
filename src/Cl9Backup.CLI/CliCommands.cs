using Cl9Backup.CLI.Domain.Entities;
using Cl9Backup.CLI.Domain.Persistence;
using Cl9Backup.CLI.Infrastructure.Api;
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
        private readonly Cl9BackupApiClient _apiClient;

        public CliCommands(IConfiguration configuration, IParametroRepository parametroRepository, Cl9BackupApiClient apiClient)
        {
            _configuration = configuration;
            _parametroRepository = parametroRepository;
            _apiClient = apiClient;
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

        public async Task<int> OnExecute(CommandLineApplication app, IConsole console)
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
                    apiHost = Prompt.GetString("Api HOST:", _configuration["ApiHost"]);

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

                var destinationParam = default(Parametro);
                var enterWithDestination = Prompt.GetYesNo("Registrar valor padrão para Destino?", false);

                if (enterWithDestination)
                {
                    var destinationInput = string.Empty;

                    while (string.IsNullOrEmpty(destinationInput))
                    {
                        destinationInput = Prompt.GetString("Destino:");

                        if (string.IsNullOrEmpty(destinationInput))
                            console.WriteLine($"O campo Destino é obrigatório.");
                    }

                    destinationParam = new Parametro() { Nome = "DEFAULT_DESTINATION", Valor = destinationInput };
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

                if (destinationParam != null)
                {
                    _parametroRepository.Add(destinationParam);
                }

                console.WriteLine($"Configurações persitidas com sucesso!");

                return Constants.OK;
            }

            if (Backup)
            {
                console.WriteTitle("Execução de Backup do CL9 Backup");

                if (string.IsNullOrEmpty(Destination))
                {
                    var destinationParam = _parametroRepository.GetByName("DEFAULT_DESTINATION");
                    Destination = destinationParam.Valor ?? string.Empty;
                }

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

                var userNameParam = _parametroRepository.GetByName("LOGIN");
                var passwordParam = _parametroRepository.GetByName("PASSWORD");

                var loginResult = await _apiClient.Login(userNameParam.Valor, passwordParam.Valor);

                if (loginResult == null)
                {
                    console.WriteLine("Resposta de autenticação na API de Backup não obtida. Encerrando execução...");
                    return Constants.OK;
                }

                var profileResult = await _apiClient.GetProfile(userNameParam.Valor, loginResult.SessionKey);

                if (profileResult == null)
                {
                    console.WriteLine("Resposta do Profile não obtido. Encerrando execução...");
                    return Constants.OK;
                }

                var destination = profileResult.Destinations.FirstOrDefault(x => x.Value.Description == Destination);
                var source = profileResult.Sources.FirstOrDefault(x => x.Value.Description == Source);
                var device = profileResult.Devices.FirstOrDefault(x => x.Value.FriendlyName == Device);



                console.WriteLine($"Executando backup do bucket \"{destination.Key}\" da fonte \"{source.Key}\" através do dispositivo \"{device.Key}\"...");
            }

            return Constants.OK;
        }
    }
}


