using System;
using BepInEx.Configuration;

namespace LockedInside;

public class ConfigManager {
    internal ConfigEntry<bool> allowExitIfLastOneAlive = null!;
    internal ConfigEntry<int> lockedChance = null!;
    internal ConfigEntry<int> reverseChance = null!;

    internal void Setup(ConfigFile configFile) {
        lockedChance = configFile.Bind("General", "Chance for mod to trigger", 10, new ConfigDescription("Percentage chance for the entrace to not allow exiting and fire exits to not allow entering for each day", new AcceptableValueRange<int>(0, 100), Array.Empty<object>()));

        reverseChance = configFile.Bind("General", "Chance for reverse door rolls", 25, new ConfigDescription("Percentage chance for the main entrance and fire exits to swap roles. Can only trigger if the above chance has triggered!", new AcceptableValueRange<int>(0, 100), Array.Empty<object>()));

        allowExitIfLastOneAlive = configFile.Bind("General", "Allow exit if last one alive", false, "If true, last player alive is always able to leave using any door");
    }
}