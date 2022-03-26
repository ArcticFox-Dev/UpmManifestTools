using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace UPMVersionSetter;

public class PackageRewriter
{
    public static void SetupForSnapshot(string path, ILogger logger)
    {
        UnityPackageManifest? manifest = null;
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            Converters =
            {
                new SemanticVersionJsonConverter(),
                new DependenciesJsonConverter()
            }
        };
        if (File.Exists(path))
        {
            var file = File.ReadAllText(path);
            manifest = JsonSerializer.Deserialize<UnityPackageManifest>(file, options);
        }
        else
        {
            logger.LogError("Could not find the package.json file in the provided directory. ({directory})",path);
            Environment.Exit(2);
        }

        if (manifest == null)
        {
            logger.LogError("Parsing of the package.json file failed. Make sure that it follows the Unity's official manifest schema.");
            Environment.Exit(2);
        }
        
        manifest.Version.PrereleaseVersion = "Snapshot";
        manifest.Version.BuildMetadata = DateTime.Now.ToString("yyyyMMdd");

        var jsonString = JsonSerializer.Serialize(manifest, options: options);
        File.WriteAllText(path,jsonString);
    }
}