using System.Text.Json;
using System.Text.Json.Serialization;

namespace UPMVersionSetter;

public class DependenciesJsonConverter : JsonConverter<Dependencies>
{
    public override Dependencies Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        var dependencyList = new List<(string name, string version)>();
        while (reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType == JsonTokenType.StartObject) reader.Read();

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var name = reader.GetString() ?? String.Empty;
                reader.Read();
                var version = reader.GetString() ?? String.Empty;
                reader.Read();
                dependencyList.Add((name, version));
            }
        }

        return new Dependencies(dependencyList);
    }

    public override void Write(
        Utf8JsonWriter writer,
        Dependencies semanticVersion,
        JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        foreach (var tuple in semanticVersion.DependencyList)
        {
            writer.WritePropertyName(tuple.name);
            writer.WriteStringValue(tuple.version);
        }
        writer.WriteEndObject();
    }
}