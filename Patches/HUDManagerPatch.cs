using System;
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

        var action = "leave";
        var door = "fire exit";
        var corrupted = "Passage restricted. ";
        var blockInteract = false;
        if(entranceTeleport.entranceId == 0){
            // Main entrance
            if(entranceTeleport.isEntranceToBuilding){
                // Entering
                action = "enter";
                if(LockedInside.stateManager.reverseMode){
                    // Reverse mode
                    blockInteract = true;
                    corrupted = "PAss<ge rrestrc?! .Ex%Enter--";
                    door += "?.";
                }
            }else{
                // Leaving
                blockInteract = true;
                if(LockedInside.stateManager.reverseMode){
                    // Reverse mode
                    return;
                }
            }
        }else{
            // Fire exit
            door = "main entrance";
            if(entranceTeleport.isEntranceToBuilding){
                // Entering
                action = "enter";
                blockInteract = true;
                if(LockedInside.stateManager.reverseMode){
                    // Reverse mode
                    return;
                }
            }else{
                // Leaving
                if(LockedInside.stateManager.reverseMode){
                    // Reverse mode
                    blockInteract = true;
                    corrupted = "PAssa ge rrestrc?! .Ex%Enter--";
                    door += "?.";
                }
            }
        }

        if(action.Equals("leave") && ExitChecker.IsLastAlive() && LockedInside.configManager.allowExitIfLastOneAlive.Value)
            return;

        var HUDMessage = corrupted + "Please " + action + " using a designated " + door;
        if(blockInteract){
            __result = false;
            interactTrigger.currentCooldownValue = 1F;

            HUDManager.Instance.DisplayTip("Entrance control", HUDMessage);
            return;
        }
    }
}