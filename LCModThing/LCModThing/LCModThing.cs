using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using LobbyCompatibility.Attributes;
using LobbyCompatibility.Enums;
using System.Reflection;
using UnityEngine;

namespace LCModThing
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency("BMX.LobbyCompatibility", BepInDependency.DependencyFlags.HardDependency)]
    [LobbyCompatibility(CompatibilityLevel.ClientOnly, VersionStrictness.None)]
    public class LCModThing : BaseUnityPlugin
    {
        public static LCModThing Instance { get; private set; } = null!;
        internal new static ManualLogSource Logger { get; private set; } = null!;
        internal static Harmony? Harmony { get; set; }

        private void Awake()
        {
            Logger = base.Logger;
            Instance = this;

            NetcodePatcher();
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

        private void NetcodePatcher()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                    if (attributes.Length > 0)
                    {
                        method.Invoke(null, null);
                    }
                }
            }
        }
    }
}
