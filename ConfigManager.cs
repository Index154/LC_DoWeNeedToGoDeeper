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
    internal ConfigEntry<bool> disableIfApparatusTaken = null!;
    internal ConfigEntry<bool> disableIfBreakerBoxDisabled = null!;

    internal void Setup(ConfigFile configFile) {
        lockedChance = configFile.Bind("1. Entrance control activation", "Global activation chance", 20, new ConfigDescription("Default percentage chance for the entrance control systems to activate on any given day. Will make it so by default everyone can only enter through the main entrance and leave through the fire exits. The other modes will override this behavior", new AcceptableValueRange<int>(0, 100), Array.Empty<object>()));

        customLockedChances = configFile.Bind("1. Entrance control activation", "Custom activation chances per moon", "", new ConfigDescription("Comma-separated list of moons with custom entrance control activation chances. Example: 'Titan:0,Vow:25,March:90,Dine:5,Gratar:100'. This will override the global chance for the selected moons. Modded moons should also work"));

        dynamicChance = configFile.Bind("2. Dynamic mode", "Global dynamic mode chance", 50, new ConfigDescription("Default percentage chance for dynamic mode to activate on any given day where the entrance control systems have already been activated. Will make it so everyone can enter using any door. Each player can then only exit through the type of door they did not enter through. If you teleport into the facility then you can use any door to leave. This mode will take priority over corruption mode", new AcceptableValueRange<int>(0, 100), Array.Empty<object>()));

        customDynamicChances = configFile.Bind("2. Dynamic mode", "Custom dynamic mode chances per moon", "", new ConfigDescription("Comma-separated list of moons with custom dynamic mode chances. Example: 'Titan:0,Vow:25,March:90,Dine:5,Gratar:100'. Modded moons should also work"));

        reverseChance = configFile.Bind("3. Corruption mode", "Global corruption chance", 10, new ConfigDescription("Default percentage chance for corruption mode to activate on any given day where the entrance control systems have already been activated. Will make it so everyone can only enter through a fire exit and leave through the main entrance, basically reversing the default behavior", new AcceptableValueRange<int>(0, 100), Array.Empty<object>()));

        customReverseChances = configFile.Bind("3. Corruption mode", "Custom corruption chances per moon", "", new ConfigDescription("Comma-separated list of moons with custom corruption chances. Example: 'Titan:0,Vow:25,March:90,Dine:5,Gratar:100'. This will override the global chance for the selected moons. Modded moons should also work"));

        allowExitIfLastOneAlive = configFile.Bind("4. Other", "Allow exit if last one alive", false, "If true, the last player alive is always able to leave using any door");

        disableIfApparatusTaken = configFile.Bind("4. Other", "Disable entrance controls after Apparatus is grabbed", false, "If true, everyone can exit and enter using any door after the Apparatus is taken by a player");

        disableIfBreakerBoxDisabled = configFile.Bind("4. Other", "Disable entrance controls while the breaker box is turned off", false, "If true, everyone can exit and enter using any door while power in the facility is turned off through the breaker box (or if the moon started with the power turned off)");

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