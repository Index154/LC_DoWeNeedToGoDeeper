using GameNetcodeStuff;
using HarmonyLib;

namespace DoWeNeedToGoDeeper.Patches;

[HarmonyPatch(typeof(PlayerControllerB))]

public class PlayerControllerBPatch {

    [HarmonyPatch("Awake")]
    [HarmonyPostfix]
    private static void AddDoorValue(PlayerControllerB __instance) {
        DoorString ds = __instance.gameObject.AddComponent<DoorString>();
    }

    [HarmonyPatch("KillPlayer")]
    [HarmonyPostfix]
    private static void ResetDoorValueOnDeath(PlayerControllerB __instance) {
        if(!__instance.isPlayerDead) return;
        DoorString ds = __instance.gameObject.GetComponent<DoorString>();
        ds.lastUsedDoor = "";
    }
}