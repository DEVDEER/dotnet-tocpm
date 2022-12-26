namespace devdeer.tools.tocpm
{
    using System.Reflection;

    using Spectre.Console;

    public static class Greeter
    {
        #region methods

        public static void WriteLogo(ConsoleColor color = ConsoleColor.Gray)
        {
            var logo = File.ReadAllText("logo.txt");
            Console.ForegroundColor = color;
            Console.WriteLine(logo);
            Console.ResetColor();
        }

        #endregion

        public static void WriteHeader()
        {
            AnsiConsole.Markup("[underline red]Hello[/] World!");
            var versionString = Assembly.GetEntryAssembly()
                ?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion;
            Console.WriteLine($"DEVDEER CPM solution converter v{versionString}");
            Console.WriteLine("\nThis tool converts a solution using the classic Nuget reference with versions of packages inside of csproj-files into one which uses Nuget CPM keeping the current package references and versions.");
            Console.WriteLine("\nMore information is available at https://github.com/DEVDEER/dotnet-tocpm.");
            Console.WriteLine("\nusage: tocpm <command> [<args>]");
        }
    }
}