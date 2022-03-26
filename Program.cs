// See https://aka.ms/new-console-template for more information

using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DotNet.GitHubAction;
using Microsoft.Extensions.Logging;
using UPMVersionSetter;

Console.WriteLine("Hello, World!");

using IHost host = Host.CreateDefaultBuilder(args)
    .Build();

static TService Get<TService>(IHost host)
    where TService : notnull =>
    host.Services.GetRequiredService<TService>();

var parser = Parser.Default.ParseArguments<ActionInputs>(() => new(), args);
parser.WithNotParsed(
    errors =>
    {
        Get<ILoggerFactory>(host)
            .CreateLogger("DotNet.GitHubAction.Program")
            .LogError(
                string.Join(
                    Environment.NewLine, errors.Select(error => error.ToString())));
        
        Environment.Exit(2);
    });

await parser.WithParsedAsync(options => ProcessManifest(options, host));
await host.RunAsync();

static async Task ProcessManifest(ActionInputs inputs, IHost host)
{
    var logger = Get<ILoggerFactory>(host).CreateLogger("UpmManifestTools");
    var foundManifest = false;
    if(Directory.Exists(inputs.Directory))
    {
        foreach (var file in Directory.GetFiles(inputs.Directory))
        {
            if (file.EndsWith("package.json"))
            {
                foundManifest = true;
                PackageRewriter.SetupForSnapshot(file,logger);
            }
        }
    }

    Console.WriteLine($"::set-output name=updated-manifest::{foundManifest}");

    await Task.CompletedTask;

    Environment.Exit(0);
}