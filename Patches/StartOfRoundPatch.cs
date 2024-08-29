using System.Collections;
using HarmonyLib;
using UnityEngine;

namespace DoWeNeedToGoDeeper.Patches;

[HarmonyPatch(typeof(StartOfRound))]
internal class StartOfRoundPatches {
    static IEnumerator DelayedTip(float delay, string text){
        // Currently unused
        yield return new WaitForSeconds(delay);
        HUDManager.Instance.DisplayTip("Warning", text, isWarning: true);
    }

    [HarmonyPostfix]
    [HarmonyPatch("ResetStats")]
    public static void DisplayLockedAlert(){
        // Patching this function because it runs early during the landing process
        // A prefix patch on openingDoorsSequence does not work
        if(DoWeNeedToGoDeeper.locked.Value){
            string text = "Entrance control systems detected!\n(Main -> Fire)";
            if(DoWeNeedToGoDeeper.dynamicMode.Value) text = "Dynamic entrance control systems detected!\n(Any -> Opposite)";
            else if(DoWeNeedToGoDeeper.reverseMode.Value) text = "Corrupted entrance control systems detected!\n(Fire -> Main)";

            HUDManager.Instance.DisplayTip("Warning", text, isWarning: true);
        }
    }
}