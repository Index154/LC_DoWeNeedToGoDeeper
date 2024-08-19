using HarmonyLib;

namespace DoWeNeedToGoDeeper.Patches;

[HarmonyPatch(typeof(HUDManager))]

public class HUDManagerPatch {
    [HarmonyPatch("HoldInteractionFill")]
    [HarmonyPostfix]
    private static void DestroyEntranceExitAbility(ref bool __result) {
        if (!__result) return;
        if (!DoWeNeedToGoDeeper.locked.Value) return;

        var localPlayer = HUDManager.Instance.playersManager.localPlayerController;
        DoorString ds = localPlayer.gameObject.GetComponent<DoorString>();
        var interactTrigger = localPlayer.hoveringOverTrigger;
        if (interactTrigger == null) return;

        var entranceTeleport = interactTrigger.gameObject.GetComponent<EntranceTeleport>();
        if (entranceTeleport == null) return;

        var action = "leave";
        var door = "fire exit";
        var corrupted = "Passage restricted. ";
        var blockInteract = false;
        if(entranceTeleport.entranceId == 0){
            // Main entrance
            if(entranceTeleport.isEntranceToBuilding){
                // Entering
                action = "enter";
                if(DoWeNeedToGoDeeper.reverseMode.Value){
                    // Reverse mode
                    blockInteract = true;
                    corrupted = "PAss<ge rrestrc?! .Ex%Enter--";
                    door += "?.";
                }
            }else{
                // Leaving
                blockInteract = true;
                if(DoWeNeedToGoDeeper.reverseMode.Value){
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
                if(DoWeNeedToGoDeeper.reverseMode.Value){
                    // Reverse mode
                    return;
                }
            }else{
                // Leaving
                if(DoWeNeedToGoDeeper.reverseMode.Value){
                    // Reverse mode
                    blockInteract = true;
                    corrupted = "PAssa ge rrestrc?! .Ex%Enter--";
                    door += "?.";
                }
            }
        }

        // Dynamic mode logic
        if(DoWeNeedToGoDeeper.dynamicMode.Value) {
            blockInteract = false;
            if(action == "enter"){
                // Save this entrance type for the current player
                if(entranceTeleport.entranceId == 0){
                    ds.lastUsedDoor = "main";
                }else{
                    ds.lastUsedDoor = "exit";
                }
            }else{
                // Prevent leaving if the door type is the same as the one the player used to enter. Also reset the saved entrance type
                if(entranceTeleport.entranceId == 0){
                    if(ds.lastUsedDoor == "main") {blockInteract = true; door = "fire exit";}
                    else ds.lastUsedDoor = "";
                }else{
                    if(ds.lastUsedDoor == "exit") {blockInteract = true; door = "main entrance";}
                    else ds.lastUsedDoor = "";
                }
            }
        }

        if(action == "leave" && ExitChecker.IsLastAlive() && DoWeNeedToGoDeeper.configManager.allowExitIfLastOneAlive.Value) return;

        var HUDMessage = corrupted + "Please " + action + " using a designated " + door;
        if(blockInteract){
            __result = false;
            interactTrigger.currentCooldownValue = 1F;

            HUDManager.Instance.DisplayTip("Entrance control", HUDMessage);
            return;
        }
    }
}