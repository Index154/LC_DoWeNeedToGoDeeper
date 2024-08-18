using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace LockedInside.Patches;

[HarmonyPatch(typeof(RoundManager))]
internal class RoundManagerPatches {

    [HarmonyPostfix]
    [HarmonyPatch("LoadNewLevel")]
    public static void RollForLocking(){

        int lockedChanceTemp = LockedInside.configManager.lockedChance.Value;
        int reverseChanceTemp = LockedInside.configManager.reverseChance.Value;

        // Go through custom locked chance list
        foreach(KeyValuePair<string, int> kvp in LockedInside.configManager.customLockedChancesDict){
            if(kvp.Key != "" && StartOfRound.Instance.currentLevel.name.ToLowerInvariant().Contains(kvp.Key)){
                lockedChanceTemp = kvp.Value;
            }
        }

        // Go through custom reverse mode chance list
        foreach(KeyValuePair<string, int> kvp in LockedInside.configManager.customReverseChancesDict){
            if(kvp.Key != "" && StartOfRound.Instance.currentLevel.name.ToLowerInvariant().Contains(kvp.Key)){
                reverseChanceTemp = kvp.Value;
            }
        }

        LockedInside.Logger.LogInfo("LevelName = " + StartOfRound.Instance.currentLevel.name);
        LockedInside.Logger.LogInfo("LockChance = " + lockedChanceTemp);
        LockedInside.Logger.LogInfo("ReverseChance = " + reverseChanceTemp);

        int lockedRoll = Random.Range(1, 101);
        if(lockedRoll <= lockedChanceTemp){

            if(!StartOfRound.Instance.currentLevel.name.ToLowerInvariant().Contains("company")){

                LockedInside.locked.Value = true;

                int reverseRoll = Random.Range(1, 101);
                if(reverseRoll <= reverseChanceTemp){
                    LockedInside.reverseMode.Value = true;
                    HUDManager.Instance.AddTextToChatOnServer("<color=red>Entrance control systems are active and corrupted!</color>");
                }else{
                    LockedInside.reverseMode.Value = false;
                    HUDManager.Instance.AddTextToChatOnServer("<color=red>Entrance control systems are active!</color>");
                }

            }
            
        }else{
            LockedInside.locked.Value = false;
            HUDManager.Instance.AddTextToChatOnServer("<color=white>Entrance control systems are inactive</color>");
        }
    }
}