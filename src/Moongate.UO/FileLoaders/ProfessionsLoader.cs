using System.Text.Json;
using Moongate.Core.Directories;
using Moongate.Core.Extensions.Strings;
using Moongate.Core.Json;
using Moongate.Core.Server.Types;
using Moongate.UO.Data.Json;
using Moongate.UO.Data.Professions;
using Moongate.UO.Data.Skills;
using Moongate.UO.Data.Types;
using Moongate.UO.Interfaces.FileLoaders;
using Serilog;

namespace Moongate.UO.FileLoaders;

public class ProfessionsLoader : IFileLoader
{
    private readonly ILogger _logger = Log.ForContext<ProfessionsLoader>();
    private readonly DirectoriesConfig _directoriesConfig;

    public ProfessionsLoader(DirectoriesConfig directoriesConfig)
    {
        _directoriesConfig = directoriesConfig;
    }

    public async Task LoadAsync()
    {

        var file = Path.Combine(_directoriesConfig[DirectoryType.Data], "Professions", "professions.json");

        await LoadFromJsonAsync(file);

        ProfessionInfo.Professions[0] = new ProfessionInfo
        {
            Name = "Advanced Skills"
        };

        _logger.Information("Loaded {Count} professions from {FilePath}", ProfessionInfo.Professions.Length, "professions.json");
    }


    /// <summary>
    /// Loads professions from the new JSON format
    /// </summary>
    private async Task LoadFromJsonAsync(string filePath)
    {
        try
        {
            var professionsData = JsonUtils.DeserializeFromFile<JsonProfessionsRoot>(filePath);

            if (professionsData?.Professions == null || professionsData.Professions.Length == 0)
            {
                _logger.Warning("No professions found in JSON file {FilePath}", filePath);
                ProfessionInfo.Professions = new ProfessionInfo[1];
                return;
            }

            /// Find the maximum profession ID to size the array correctly
            var maxProfId = professionsData.Professions.Max(p => p.Desc);
            ProfessionInfo.Professions = new ProfessionInfo[maxProfId + 1];

            /// Process each profession from JSON
            foreach (var jsonProf in professionsData.Professions)
            {
                var prof = CreateProfessionFromJson(jsonProf);

                prof.FixSkills(); /// Adjust skills array if needed
                ProfessionInfo.Professions[prof.ID] = prof;
            }

            ProfessionInfo.Professions[0] = new ProfessionInfo
            {
                Name = "Advanced Skills"
            };

            _logger.Information("Successfully loaded professions from JSON file {FilePath}", filePath);
        }
        catch (JsonException ex)
        {
            _logger.Error(ex, "Failed to parse JSON professions file {FilePath}", filePath);
            throw;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error loading professions from JSON file {FilePath}", filePath);
            throw;
        }
    }

    /// <summary>
    /// Creates a ProfessionInfo object from JSON data
    /// </summary>
    private static ProfessionInfo CreateProfessionFromJson(JsonProfession jsonProf)
    {
        var prof = new ProfessionInfo
        {
            Name = jsonProf.TrueName ?? jsonProf.Name,
            ID = jsonProf.Desc,
            NameID = jsonProf.NameId,
            DescID = jsonProf.DescId,
            TopLevel = jsonProf.TopLevel,
            GumpID = jsonProf.Gump
        };

        /// Process skills
        if (jsonProf.Skills != null)
        {
            for (int i = 0; i < jsonProf.Skills.Length && i < prof.Skills.Length; i++)
            {
                var skill = jsonProf.Skills[i];
                if (ProfessionInfo.TryGetSkillName(skill.Name, out var skillName))
                {
                    prof.Skills[i] = (skillName, (byte)skill.Value);
                }
            }
        }

        /// Process stats
        if (jsonProf.Stats != null)
        {
            foreach (var stat in jsonProf.Stats)
            {
                if (Enum.TryParse(stat.Type, out StatType statType))
                {
                    prof.Stats[(int)statType >> 1] = (byte)stat.Value;
                }
            }
        }

        return prof;
    }
}
