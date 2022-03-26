namespace UPMVersionSetter;

public class Author
{
    public string Name { get; set; }
    public string? Email { get; set; }
    public string? Url { get; set; }

    public Author(string name, string? email, string? url)
    {
        Name = name;
        Email = email;
        Url = url;
    }
}