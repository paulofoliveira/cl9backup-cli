using McMaster.Extensions.CommandLineUtils;

namespace Cl9Backup.CLI
{
    public static class ConsoleExtensions
    {
        public static IConsole WriteTitle(this IConsole console, string title)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentNullException(nameof(title));

            return console.WriteLine($"\n***** {title} *****\n");
        }
    }
}
