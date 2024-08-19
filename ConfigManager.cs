using System;
using System.Collections.Generic;
using BepInEx.Configuration;

namespace DoWeNeedToGoDeeper;

public class ConfigManager {
    internal ConfigEntry<int> lockedChance = null!;
    internal ConfigEntry<string> customLockedChances = null!;
    internal Dictionary<string, int> customLockedChancesDict = null!;
    internal ConfigEntry<int> dynamicChance = null!;
    internal ConfigEntry<string> customDynamicChances = null!;
    internal Dictionary<string, int> customDynamicChancesDict = null!;
    internal ConfigEntry<int> reverseChance = null!;
    internal ConfigEntry<string> customReverseChances = null!;
    internal Dictionary<string, int> customReverseChancesDict = null!;
    internal ConfigEntry<bool> allowExitIfLastOneAlive = null!;
    internal ConfigEntry<bool> allowExitIfApparatusTaken = null!;
    internal ConfigEntry<bool> allowExitIfBreakerBoxDisabled = null!;

    internal void Setup(ConfigFile configFile) {
        lockedChance = configFile.Bind("Entrance control activation", "Global activation chance", 20, new ConfigDescription("Default percentage chance for the entrance controls to activate on any given day. Will make it so everyone can only enter through the main entrance and leave through the fire exits", new AcceptableValueRange<int>(0, 100), Array.Empty<object>()));

        customLockedChances = configFile.Bind("Entrance control activation", "Custom activation chances per moon", "", new ConfigDescription("Comma-separated list of moons with custom entrance controls activation chances. Example: 'Titan:0,Vow:25,March:90,Dine:5,Gratar:100'. This will override the global chance for the selected moons. Modded moons should also work"));

        dynamicChance = configFile.Bind("Dynamic mode", "Global dynamic mode chance", 40, new ConfigDescription("Default percentage chance for dynamic mode to activate on any given day where the entrance controls have already been activated. Will make it so everyone can enter using any door. Each player can then only exit through the type of door they did not enter through. If you teleport into the facility then you can use any door to leave", new AcceptableValueRange<int>(0, 100), Array.Empty<object>()));

        customDynamicChances = configFile.Bind("Dynamic mode", "Custom dynamic mode chances per moon", "", new ConfigDescription("Comma-separated list of moons with custom dynamic mode chances. Example: 'Titan:0,Vow:25,March:90,Dine:5,Gratar:100'. Modded moons should also work. This mode will take priority over corruption mode"));

        reverseChance = configFile.Bind("Corruption mode", "Global corruption chance", 20, new ConfigDescription("Default percentage chance for corruption mode to activate on any given day where the entrance controls have already been activated. Will make it so everyone can only enter through a fire exit and leave through the main entrance instead of the default behavior", new AcceptableValueRange<int>(0, 100), Array.Empty<object>()));

        customReverseChances = configFile.Bind("Corruption mode", "Custom corruption chances per moon", "", new ConfigDescription("Comma-separated list of moons with custom corruption chances. Example: 'Titan:0,Vow:25,March:90,Dine:5,Gratar:100'. This will override the global chance for the selected moons. Modded moons should also work"));

        allowExitIfLastOneAlive = configFile.Bind("Other", "Allow exit if last one alive", false, "If true, the last player alive is always able to leave using any door");

        allowExitIfApparatusTaken = configFile.Bind("Other", "Allow exit after Apparatus is grabbed", false, "If true, everyone can exit using any door after the Apparatus is taken by a player");

        allowExitIfBreakerBoxDisabled = configFile.Bind("Other", "Allow exit if the breaker box is without power", false, "If true, everyone can exit using any door while power in the facility is turned off using the breaker box");

        customLockedChancesDict = StrToDict(customLockedChances.Value);
        customReverseChancesDict = StrToDict(customReverseChances.Value);
        customDynamicChancesDict = StrToDict(customDynamicChances.Value);
        
        static Dictionary<string, int> StrToDict(string list){
        
            Dictionary<string, int> dict = [];

            foreach(string st in list.ToLowerInvariant().Split(",")){
                string[] split = st.Split(":");
                if(split.Length > 1){
                    int value = 0;
                    try{
                        value = Int32.Parse(split[1]);
                    }finally{
                        if(value < 0){value = 0;}
                        if(value > 100){value = 100;}
                        if(!dict.ContainsKey(split[0])){ dict.Add(split[0], value); }
                    }
                }
            }
            return dict;
        }
    }
}