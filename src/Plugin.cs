using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using PredictiveMissile.Settings;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using PredictiveMissile.MissileSpacing;

namespace PredictiveMissile
{
    public class PluginInfo
    {
        public const string PLUGIN_GUID = "com.shard.predictive_missile";
        public const string PLUGIN_NAME = "PredictiveMissile";
        public const string PLUGIN_VERSION = "1.0.0";
        public const string PROCESS_NAME = "Star Valor.exe";
    }

    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInProcess(PluginInfo.PROCESS_NAME)]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource Log;
        public static ConfigOptions Settings = new();

        public void Awake()
        {
            //Find every class with the [HarmonyPatch] decoration and load it
            GetHarmonyAssemblies().ForEach(LoadHarmonyAssembly);

            Log = Logger;

            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            ConfigOptions.LoadConfig(Config);
        }

        public List<Type> GetHarmonyAssemblies()
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly
                    .GetTypes()
                    .Where(t => t
                        .IsDefined(typeof(HarmonyPatch))
                    )
                )
                .ToList();
        }

        public void LoadHarmonyAssembly(Type type)
        {
            Harmony.CreateAndPatchAll(type);
        }

        public void FixedUpdate()
        {
            //Only want to run this once per FixedUpdate
            Spacing.FixedUpdate();
        }

    }
}
