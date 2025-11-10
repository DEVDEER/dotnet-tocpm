using System.Reflection;
using System.Text;

using devdeer.tools.tocpm.Commands;

using Spectre.Console.Cli;

var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(3);
Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;
var app = new CommandApp();
app.Configure(
    config =>
    {
        if (!string.IsNullOrEmpty(version))
        {
            config.SetApplicationVersion(version);
        }
        config.SetApplicationName("tocpm");
        config.AddCommand<SimulateCommand>("simulate")
            .WithDescription("Simulates the operation at the provided location and writes the results to the console.")
            .WithExample("simulate", ".");
        config.AddCommand<ExecuteCommand>("execute")
            .WithDescription("Executes a real run at the specified location.")
            .WithExample("execute", @"C:\temp\project")
            .WithExample("execute", @"C:\temp\project", "-f");
    });
var result = app.Run(args);
return result;
