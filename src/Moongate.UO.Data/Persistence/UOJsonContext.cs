using System.Text.Json.Serialization;
using Moongate.UO.Data.Persistence.Entities;
using Moongate.UO.Data.Skills;
using Moongate.UO.Data.Types;

namespace Moongate.UO.Data.Persistence;

[JsonSerializable(typeof(UOAccountEntity))]
[JsonSerializable(typeof(UOAccountCharacterEntity))]
[JsonConverter(typeof(JsonStringEnumConverter<Stat>))]
[JsonSerializable(typeof(SkillInfo))]
[JsonSerializable(typeof(SkillInfo[]))]
[JsonSourceGenerationOptions(
    WriteIndented = false,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
public partial class UOJsonContext : JsonSerializerContext
{
}
