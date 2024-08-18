using System;
using System.Collections.Generic;
using BepInEx.Configuration;

namespace LockedInside;

public class ConfigManager {
    internal ConfigEntry<int> lockedChance = null!;
    internal ConfigEntry<int> reverseChance = null!;
    internal ConfigEntry<string> customLockedChances = null!;
    internal ConfigEntry<string> customReverseChances = null!;
    internal Dictionary<string, int> customLockedChancesDict = null!;
    internal Dictionary<string, int> customReverseChancesDict = null!;
    internal ConfigEntry<bool> allowExitIfLastOneAlive = null!;

    internal void Setup(ConfigFile configFile) {
        lockedChance = configFile.Bind("General", "Chance for mod to trigger", 10, new ConfigDescription("Default percentage chance for the entrace to not allow exiting and fire exits to not allow entering for a given day", new AcceptableValueRange<int>(0, 100), Array.Empty<object>()));

        reverseChance = configFile.Bind("General", "Chance for reverse door rolls", 25, new ConfigDescription("Default percentage chance for the main entrance and fire exits to \"swap roles\", making it so you have to enter through the exit and leave through the entrance instead. Can only trigger if the above chance has also been triggered", new AcceptableValueRange<int>(0, 100), Array.Empty<object>()));

        customLockedChances = configFile.Bind("General", "Moons with custom trigger chance", "", new ConfigDescription("Comma-separated list of moon names and their custom trigger chances. Example: 'Dine:50,Adamance:0,Experimentation:100'. Modded moons should also work"));

        customReverseChances = configFile.Bind("General", "Moons with custom reverse mode chance", "", new ConfigDescription("Comma-separated list of moons to get custom reverse mode chances. Example: 'Titan:5,Vow:25,March:90,Dine:15,Gratar:100'. Modded moons should also work"));

        allowExitIfLastOneAlive = configFile.Bind("General", "Allow exit if last one alive", false, "If true, the last player alive is always able to leave using any door");

        customLockedChancesDict = StrToDict(customLockedChances.Value);
        customReverseChancesDict = StrToDict(customReverseChances.Value);

        static Dictionary<string, int> StrToDict(string list){

            Dictionary<string, int> dict = new Dictionary<string, int>();

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