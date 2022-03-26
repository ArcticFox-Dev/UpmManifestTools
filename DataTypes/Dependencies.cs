namespace UPMVersionSetter;

public class Dependencies
{
    public List<(string name, string version)> DependencyList { get; set; }

    public Dependencies(List<(string name, string version)> dependencyList)
    {
        DependencyList = dependencyList;
    }
}