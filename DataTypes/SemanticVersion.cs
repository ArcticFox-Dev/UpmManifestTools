namespace UPMVersionSetter;

public class SemanticVersion
{
    public int Major { get; set; }
    public int Minor { get; set; }
    public int Patch { get; set; }
    public string? PrereleaseVersion { get; set; }
    public string? BuildMetadata { get; set; }

    public SemanticVersion(int major, int minor, int patch, string? prereleaseVersion = null, string? buildMetadata = null)
    {
        Major = major;
        Minor = minor;
        Patch = patch;
        PrereleaseVersion = prereleaseVersion;
        BuildMetadata = buildMetadata;
    }
}