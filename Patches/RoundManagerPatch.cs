using HarmonyLib;
using UnityEngine;

namespace LockedInside.Patches;

[HarmonyPatch(typeof(RoundManager))]
internal class RoundManagerPatches {
    [HarmonyPostfix]
    [HarmonyPatch("LoadNewLevel")]
    public static void RollForLocking(){

        int lockedRoll = Random.Range(1, 101);
        if(lockedRoll <= LockedInside.configManager.lockedChance.Value && StartOfRound.Instance.currentLevel.name != "CompanyBuildingLevel"){

            LockedInside.Logger.LogDebug("Locking");
            LockedInside.stateManager.locked = true;

            int reverseRoll = Random.Range(1, 101);
            if(reverseRoll <= LockedInside.configManager.reverseChance.Value){
                LockedInside.stateManager.reverseMode = true;
                HUDManager.Instance.AddTextToChatOnServer("<color=red>Entrance control systems are active and corrupted!</color>");
            }else{
                LockedInside.stateManager.reverseMode = false;
                HUDManager.Instance.AddTextToChatOnServer("<color=red>Entrance control systems are active!</color>");
            }
        }else{
            LockedInside.Logger.LogDebug("Unlocking");
            LockedInside.stateManager.locked = false;
            HUDManager.Instance.AddTextToChatOnServer("<color=white>Entrance control systems are inactive</color>");
        }
    }
}