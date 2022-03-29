using CommandLine;

namespace DotNet.GitHubAction;

public class ActionInputs
{
    public ActionInputs()
    {
        if (Environment.GetEnvironmentVariable("GREETINGS") is { Length: > 0 } greetings)
        {
            Console.WriteLine(greetings);
        }
    }

    [Option('d', "dir",
        Required = true,
        HelpText = "The directory in which to look for package.json file.")]
    public string Directory { get; set; } = null!;
    
    [Option('a', "action",
        Required = true,
        HelpText = "The action to perform on the version. Allowed options: snapshot, patch, minor, major")]
    public string Action { get; set; } = null!;
    
    static void ParseAndAssign(string? value, Action<string> assign)
    {
        if (value is { Length: > 0 } && assign is not null)
        {
            assign(value.Split("/")[^1]);
        }
    }
}