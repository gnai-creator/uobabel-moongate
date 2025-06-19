using Moongate.Core.Directories;
using Moongate.Core.Server.Types;
using Moongate.UO.Data.Bodies;
using Moongate.UO.Interfaces.FileLoaders;
using Serilog;

namespace Moongate.UO.FileLoaders;

public class BodyDataLoader : IFileLoader
{
    private readonly ILogger _logger = Log.ForContext<BodyDataLoader>();

    private readonly DirectoriesConfig _directoriesConfig;

    public BodyDataLoader(DirectoriesConfig directoriesConfig)
    {
        _directoriesConfig = directoriesConfig;
    }

    public async Task LoadAsync()
    {
        var bodyTable =  Path.Combine(_directoriesConfig[DirectoryType.Data], "bodyTable.cfg");

        if (!File.Exists(bodyTable))
        {
            _logger.Warning("Warning: Data/bodyTable.cfg does not exist");
            Body.Types = [];
        }

        using StreamReader ip = new StreamReader(bodyTable);
        Body.Types = new BodyType[0x1000];

        while (await ip.ReadLineAsync() is { } line)
        {
            if (line.Length == 0 || line.StartsWith('#'))
            {
                continue;
            }

            var split = line.Split('\t');

            if (int.TryParse(split[0], out var bodyID) && Enum.TryParse(split[1], true, out BodyType type) && bodyID >= 0 &&
                bodyID < Body.Types.Length)
            {
                Body.Types[bodyID] = type;
            }
            else
            {
                _logger.Warning("Warning: Invalid bodyTable entry:");
                _logger.Warning(line);
            }
        }

        _logger.Information("Loaded {Count} body types from {FilePath}", Body.Types.Length, bodyTable);

    }
}
