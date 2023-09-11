namespace ReventTask.Domain;

[JsonSerializable(typeof(InboundLogDto))]
[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Default,
   PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class InboundLogDtoJsonContext : JsonSerializerContext
{

}
