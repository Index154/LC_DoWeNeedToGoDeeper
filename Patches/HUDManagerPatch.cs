using HarmonyLib;

namespace LockedInside.Patches;

[HarmonyPatch(typeof(HUDManager))]
public class HUDManagerPatch {
    [HarmonyPatch("HoldInteractionFill")]
    [HarmonyPostfix]
    private static void DestroyEntranceExitAbility(ref bool __result) {
        if (!__result)
            return;
        if (!LockedInside.stateManager.locked)
            return;

        var localPlayer = HUDManager.Instance.playersManager.localPlayerController;
        var interactTrigger = localPlayer.hoveringOverTrigger;
        if (interactTrigger == null)
            return;

        var entranceTeleport = interactTrigger.gameObject.GetComponent<EntranceTeleport>();
        if (entranceTeleport == null)
            return;

        if (entranceTeleport.entranceId == 0) {
            if (entranceTeleport.isEntranceToBuilding && !LockedInside.stateManager.reverseMode)
                return;

            if (!entranceTeleport.isEntranceToBuilding && LockedInside.stateManager.reverseMode)
                return;

            if (!entranceTeleport.isEntranceToBuilding && ExitChecker.IsLastAlive() && LockedInside.configManager.allowExitIfLastOneAlive.Value)
                return;

            __result = false;
            interactTrigger.currentCooldownValue = 1F;

            HUDManager.Instance.DisplayTip("Entrance control", "Passage denied. Please seek an alternate gate");
            return;
        }

        if (!entranceTeleport.isEntranceToBuilding && !LockedInside.stateManager.reverseMode)
            return;
        
        if (entranceTeleport.isEntranceToBuilding && LockedInside.stateManager.reverseMode)
            return;
        
        if (!entranceTeleport.isEntranceToBuilding && ExitChecker.IsLastAlive() && LockedInside.configManager.allowExitIfLastOneAlive.Value)
                return;

        __result = false;
        interactTrigger.currentCooldownValue = 1F;
        HUDManager.Instance.DisplayTip("Entrance control", "Passage denied. Please seek an alternate gate");
    }
}