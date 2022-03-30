using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace UPMVersionSetter;

public class PackageRewriter
{
    public static void SetupForSnapshot(string path, ILogger logger)
    {
        UnityPackageManifest? manifest = GetManifest(path, logger, out var options);
        
        manifest.Version.PrereleaseVersion = "Snapshot";
        manifest.Version.BuildMetadata = DateTime.Now.ToString("yyyyMMdd") +
                                         DateTime.Now.TimeOfDay.ToString(@"hh\.mm\.ss")
                                             .Replace(".", "");

        var jsonString = JsonSerializer.Serialize(manifest, options: options);
        File.WriteAllText(path,jsonString);
    }

    public static void BumpPatchVersion(string path, ILogger logger)
    {
        var manifest = GetManifest(path, logger, out var options);

        manifest.Version.Patch += 1;
        manifest.Version.PrereleaseVersion = null;
        manifest.Version.BuildMetadata = null;

        var jsonString = JsonSerializer.Serialize(manifest, options: options);
        File.WriteAllText(path,jsonString);
    }

    public static void BumpMinorVersion(string path, ILogger logger)
    {
        var manifest = GetManifest(path, logger, out var options);

        manifest.Version.Minor += 1;
        manifest.Version.Patch = 0;
        manifest.Version.PrereleaseVersion = null;
        manifest.Version.BuildMetadata = null;

        var jsonString = JsonSerializer.Serialize(manifest, options: options);
        File.WriteAllText(path,jsonString);
    }

    public static void BumpMajorVersion(string path, ILogger logger)
    {
        var manifest = GetManifest(path, logger, out var options);

        manifest.Version.Major += 1;
        manifest.Version.Minor = 0;
        manifest.Version.Patch = 0;
        manifest.Version.PrereleaseVersion = null;
        manifest.Version.BuildMetadata = null;

        var jsonString = JsonSerializer.Serialize(manifest, options: options);
        File.WriteAllText(path,jsonString);
    }

    private static UnityPackageManifest GetManifest(string path, ILogger logger, out JsonSerializerOptions options)
    {
        UnityPackageManifest? manifest = null;
        options = new JsonSerializerOptions
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
            logger.LogError("Could not find the package.json file in the provided directory. ({directory})", path);
            Environment.Exit(2);
        }

        if (manifest == null)
        {
            logger.LogError(
                "Parsing of the package.json file failed. Make sure that it follows the Unity's official manifest schema.");
            Environment.Exit(2);
        }

        return manifest;
    }
}