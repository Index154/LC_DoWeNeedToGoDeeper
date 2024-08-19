using GameNetcodeStuff;
using HarmonyLib;

namespace DoWeNeedToGoDeeper.Patches;

[HarmonyPatch(typeof(ShipTeleporter))]

public class ShipTeleporterPatch {

    [HarmonyPatch("SetPlayerTeleporterId")]
    [HarmonyPostfix]
    private static void ResetDoorValueOnBeamUp(PlayerControllerB playerScript, int teleporterId) {
        if(!playerScript.isInsideFactory && teleporterId == -1){
            DoorString ds = playerScript.gameObject.GetComponent<DoorString>();
            ds.lastUsedDoor = "";
        }
    }
}