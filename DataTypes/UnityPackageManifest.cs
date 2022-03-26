namespace UPMVersionSetter;

public class UnityPackageManifest
{
    public UnityPackageManifest()
    {
        Name = "empty";
        Version = new SemanticVersion(0,0,0);
    }

    #region Mandatory

    public string Name { get; set; }
    public SemanticVersion Version { get; set; }

    #endregion

    #region Optional

    public string? Description { get; set; }
    public string? DisplayName { get; set; }
    public string? Unity { get; set; }
    
    public Author? Author { get; set; }
    
    public string? ChangelogUrl { get; set; }
    
    public Dependencies? Dependencies { get; set; }
    
    public string? DocumentationUrl { get; set; }

    public bool? HideInEditor { get; set; }
    
    public string[]? Keywords { get; set; }
    
    public string? License { get; set; }
    
    public string? LicenseUrl {get; set; }

    public Sample[]? Samples { get; set; }

    public string? Type { get; set; }
    
    public string? UnityRelease { get; set; }

    #endregion
}