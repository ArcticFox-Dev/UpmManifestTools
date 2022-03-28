using System.Text.Json;
using System.Text.Json.Serialization;

namespace UPMVersionSetter;

public class SemanticVersionJsonConverter : JsonConverter<SemanticVersion>
{
    private const string InvalidData = "Invalid string passed into sem ver parser";
    
    public override SemanticVersion Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        var body = reader.GetString();
        var (major, minor, patch) = TryGetSemanticVersion(body);
        var prereleaseVersion = TryGetPrereleaseVersion(body);
        var buildMetadata = TryGetBuildMetadata(body);
        
        return new SemanticVersion(major, minor, patch, prereleaseVersion, buildMetadata);
    }

    public override void Write(
        Utf8JsonWriter writer,
        SemanticVersion semanticVersion,
        JsonSerializerOptions options)
    {
        var semVer = $"{semanticVersion.Major}.{semanticVersion.Minor}.{semanticVersion.Patch}";
        var prereleaseVer = semanticVersion.PrereleaseVersion != null ? $"-{semanticVersion.PrereleaseVersion}" : "";
        var buildMetadata = semanticVersion.BuildMetadata != null ?  $".{semanticVersion.BuildMetadata}" : "";
        writer.WriteStringValue($"{semVer}{prereleaseVer}{buildMetadata}");
    }

    private static (int major,int minor, int patch) TryGetSemanticVersion(string? body)
    {
        if (body == null) return (0,0,0);
        var trimmedBody = body;
        if (body.Contains('-'))
        {
            trimmedBody = body.Substring(0, body.IndexOf('-'));
        }
        var versionDigits = trimmedBody.Split('.');
        if(versionDigits.Length != 3) throw new InvalidDataException(InvalidData);
        var major = Int32.Parse(versionDigits[0]);
        var minor = Int32.Parse(versionDigits[1]);
        var patch = Int32.Parse(versionDigits[2]);

        return (major, minor, patch);
    }

    private static string? TryGetPrereleaseVersion(string? body)
    {
        if (body == null || !body.Contains('-')) return null;
        var trimmedBody = body.Substring(body.IndexOf('-') + 1);
        trimmedBody = trimmedBody.Substring(0, trimmedBody.IndexOf('_'));
        return trimmedBody;
    }

    private static string? TryGetBuildMetadata(string? body)
    {
        if (body == null || body.Split('.').Length < 4) return null;
        return body.Substring(body.LastIndexOf('.'));
    }
}