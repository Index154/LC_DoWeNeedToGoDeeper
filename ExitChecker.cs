using System.Linq;

namespace LockedInside;

public static class ExitChecker {
    public static bool IsLastAlive() {
        var alivePeople = StartOfRound.Instance.allPlayerScripts.Count(playerScript =>
            !playerScript.isPlayerDead && playerScript.isPlayerControlled);

        if (alivePeople > 1)
            return false;

        return true;
    }
}