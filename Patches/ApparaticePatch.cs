using HarmonyLib;

namespace DoWeNeedToGoDeeper.Patches;

[HarmonyPatch(typeof(LungProp))]
public class ApparaticePatch {
    
    [HarmonyPrefix, HarmonyPatch(nameof(LungProp.EquipItem))]
    internal static void turnOffLockedMode(LungProp __instance, ref bool ___isLungDocked){
        if (!__instance.IsHost) return;
        if (!___isLungDocked) return;
        if (!DoWeNeedToGoDeeper.configManager.disableIfApparatusTaken.Value) return;
        
        DoWeNeedToGoDeeper.locked.Value = false;
    }
}