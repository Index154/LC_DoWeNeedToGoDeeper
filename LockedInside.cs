using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using LethalNetworkAPI;

namespace LockedInside;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("LethalNetworkAPI")]
public class LockedInside : BaseUnityPlugin
{
    public static LockedInside Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger { get; private set; } = null!;
    internal static Harmony? Harmony { get; set; }
    internal static ConfigManager configManager = null!;
    public static LethalNetworkVariable<bool> locked = new LethalNetworkVariable<bool>(identifier: "lockedState");
    public static LethalNetworkVariable<bool> reverseMode = new LethalNetworkVariable<bool>(identifier: "reverseModeState");

    private void Awake()
    {
        Logger = base.Logger;
        Instance = this;

        configManager = new ConfigManager();
        configManager.Setup(Config);

        Patch();

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
    }

    internal static void Patch()
    {
        Harmony ??= new Harmony(MyPluginInfo.PLUGIN_GUID);

        Logger.LogDebug("Patching...");

        Harmony.PatchAll();

        Logger.LogDebug("Finished patching!");
    }

    internal static void Unpatch()
    {
        Logger.LogDebug("Unpatching...");

        Harmony?.UnpatchSelf();

        Logger.LogDebug("Finished unpatching!");
    }
}
