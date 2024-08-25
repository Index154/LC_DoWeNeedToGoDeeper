using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace DoWeNeedToGoDeeper.Patches;

[HarmonyPatch(typeof(RoundManager))]
internal class RoundManagerPatches {

    [HarmonyPostfix]
    [HarmonyPatch("LoadNewLevel")]
    public static void RollForLocking(){

        int lockedChanceTemp = DoWeNeedToGoDeeper.configManager.lockedChance.Value;
        int reverseChanceTemp = DoWeNeedToGoDeeper.configManager.reverseChance.Value;
        int dynamicChanceTemp = DoWeNeedToGoDeeper.configManager.dynamicChance.Value;

        // Go through custom locked chance list
        foreach(KeyValuePair<string, int> kvp in DoWeNeedToGoDeeper.configManager.customLockedChancesDict){
            if(kvp.Key != "" && StartOfRound.Instance.currentLevel.name.ToLowerInvariant().Contains(kvp.Key)){
                lockedChanceTemp = kvp.Value;
            }
        }
        // Go through custom dynamic mode chance list
        foreach(KeyValuePair<string, int> kvp in DoWeNeedToGoDeeper.configManager.customDynamicChancesDict){
            if(kvp.Key != "" && StartOfRound.Instance.currentLevel.name.ToLowerInvariant().Contains(kvp.Key)){
                dynamicChanceTemp = kvp.Value;
            }
        }
        // Go through custom reverse mode chance list
        foreach(KeyValuePair<string, int> kvp in DoWeNeedToGoDeeper.configManager.customReverseChancesDict){
            if(kvp.Key != "" && StartOfRound.Instance.currentLevel.name.ToLowerInvariant().Contains(kvp.Key)){
                reverseChanceTemp = kvp.Value;
            }
        }

        DoWeNeedToGoDeeper.Logger.LogDebug("LevelName = " + StartOfRound.Instance.currentLevel.name);
        DoWeNeedToGoDeeper.Logger.LogDebug("LockChance = " + lockedChanceTemp);
        DoWeNeedToGoDeeper.Logger.LogDebug("ReverseChance = " + reverseChanceTemp);
        DoWeNeedToGoDeeper.Logger.LogDebug("DynamicChance = " + dynamicChanceTemp);

        int lockedRoll = Random.Range(1, 101);
        if(lockedRoll <= lockedChanceTemp){

            if(!StartOfRound.Instance.currentLevel.name.ToLowerInvariant().Contains("company")){

                DoWeNeedToGoDeeper.locked.Value = true;

                int specialModeRoll = Random.Range(1, 101);
                if(specialModeRoll <= dynamicChanceTemp){
                    DoWeNeedToGoDeeper.reverseMode.Value = false;
                    DoWeNeedToGoDeeper.dynamicMode.Value = true;
                    HUDManager.Instance.AddTextToChatOnServer("<color=yellow>Dynamic entrance control systems are active!</color>");
                }else if(specialModeRoll <= dynamicChanceTemp + reverseChanceTemp){
                    DoWeNeedToGoDeeper.reverseMode.Value = true;
                    DoWeNeedToGoDeeper.dynamicMode.Value = false;
                    HUDManager.Instance.AddTextToChatOnServer("<color=red>Entrance control systems are active but corrupted!</color>");
                }else{
                    DoWeNeedToGoDeeper.reverseMode.Value = false;
                    DoWeNeedToGoDeeper.dynamicMode.Value = false;
                    HUDManager.Instance.AddTextToChatOnServer("<color=green>Entrance control systems are active!</color>");
                }
            }
            
        }else{
            DoWeNeedToGoDeeper.locked.Value = false;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch("SwitchPower")]
    public static void TrackBreakerState(bool on){
        if(!DoWeNeedToGoDeeper.locked.Value) return;
        if(!DoWeNeedToGoDeeper.configManager.disableIfBreakerBoxDisabled.Value) return;
        
        if(on) DoWeNeedToGoDeeper.breakerOff.Value = false;
        else DoWeNeedToGoDeeper.breakerOff.Value = true;
    }
}