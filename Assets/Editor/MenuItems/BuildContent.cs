using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Build;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class BuildContent
{
    public const int MANIFEST_VERSION = 1;

    [MenuItem("Shipbreaker/Build", priority = 2)]
    static bool RunBuild()
    {
        AddressableAssetSettingsDefaultObject.Settings.activeProfileId = AddressableAssetSettingsDefaultObject.Settings.profileSettings.GetProfileId("Default");

        AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);
        bool success = string.IsNullOrEmpty(result.Error);

        if (success && Settings.VerifyBuildSettings())
        {
            var manifest = GenerateManifest();

            // Move the default bundle, if it exists
            var defaultCatalogBundle = Directory.GetFiles(Application.dataPath + "\\..\\BuiltShipContent", "*.bundle", SearchOption.TopDirectoryOnly).FirstOrDefault();
            if(defaultCatalogBundle != null)
            {
                MoveShipBundle("Common", Application.dataPath + "\\..\\Library\\com.unity.addressables\\aa\\Windows\\catalog.json", defaultCatalogBundle, manifest);
            }

            // Move each custom bundle
            foreach(var shipDirectory in Directory.GetDirectories(Application.dataPath + "\\..\\BuiltShipContent"))
            {
                var shipName = Path.GetFileName(shipDirectory);
                MoveShipBundle(shipName, shipDirectory + "\\" + shipName + ".json", shipDirectory + "\\" + shipName + "_assets_all.bundle", manifest);
            }

            LoadGameAssets.ReloadAssets();
            Debug.Log("Build Complete");
            return true;
        }
        else
        {
            Debug.LogError("Build Failed!");
            Debug.LogError("Addressables build error encountered: " + result.Error);
            Debug.LogError("If you are stuck, contact Piepieonline on the Shipbreaker discord (#modding-discussion)");
            return false;
        }
    }

    private static Manifest GenerateManifest()
    {
        var baseVersionList = new List<string>();
        string[] typesToSearch = File.ReadAllLines(Path.Combine(Settings.buildSettings.ShipbreakerPath, "BepInEx", "patchers", "ModdedShipLoaderPatcher", "TypesToModify.txt"));

        foreach(var typeString in typesToSearch)
        {
            var type = Type.GetType($"BBI.Unity.Game.{typeString}, BBI.Unity.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", true);
            foreach(var assetGUID in AssetDatabase.FindAssets($"t:{typeString}", null))
            {
                var asset = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(assetGUID), type);
                var val = ((string)type.GetField("AssetBasis").GetValue(asset));
                if(val != null && val != "")
                {
                    baseVersionList.Add($"[{assetGUID}]");
                }
            }
        }

        return new Manifest() { baseOverrides = baseVersionList.ToArray() };
    }

    private static void MoveShipBundle(string shipName, string catalogPath, string bundlePath, Manifest manifest)
    {
        var modPath = "BepInEx\\plugins\\ModdedShipLoader\\Ships";
        var shipPath = $"{shipName}.{Settings.buildSettings.Author}";

        if(!Directory.Exists(Path.Combine(Settings.buildSettings.ShipbreakerPath, modPath, shipPath)))
        {
            Debug.Log($"{shipName} - Creating build directory");
            Directory.CreateDirectory(Path.Combine(Settings.buildSettings.ShipbreakerPath, modPath, shipPath));
        }

        Debug.Log($"{shipName} - Moving bundle");

        File.Copy(
            bundlePath,
            Path.Combine(Settings.buildSettings.ShipbreakerPath, modPath, shipPath, bundlePath.Split('\\').Last()),
            true
        );
        // File.Delete(bundlePath);

        Debug.Log($"{shipName} - Moving and modifying catalog");
        var catalog = JObject.Parse(File.ReadAllText(catalogPath));
        var internalIds = (JArray)catalog.SelectToken("$.m_InternalIds");

        for(int i = 0; i < internalIds.Count(); i++)
        {
            if(internalIds[i].ToString().Contains("common_assets_all.bundle"))
            {
                internalIds[i].Replace(Path.Combine(Settings.buildSettings.ShipbreakerPath, modPath, $"Common.{Settings.buildSettings.Author}", "common_assets_all.bundle"));
            }
            else if(internalIds[i].ToString().Contains(".bundle"))
            {
                internalIds[i].Replace(Path.Combine(Settings.buildSettings.ShipbreakerPath, modPath, shipPath, shipName + "_assets_all.bundle"));
            }
        }

        File.WriteAllText(Path.Combine(Settings.buildSettings.ShipbreakerPath, modPath, shipPath, "catalog.json"), catalog.ToString());

        // File.Delete(catalogPath);

        Debug.Log($"{shipName} - Writing manifest");
        File.WriteAllText(Path.Combine(Settings.buildSettings.ShipbreakerPath, modPath, shipPath, "manifest.json"), JsonConvert.SerializeObject(manifest));
    }

    [MenuItem("Shipbreaker/Build and run", priority = 3)]
    static void BuildAndRun()
    {
        if(RunBuild())
            System.Diagnostics.Process.Start(Path.Combine(Settings.buildSettings.ShipbreakerPath, "Shipbreaker.exe"));
    }

    [MenuItem("Shipbreaker/Update game catalog", priority = 1000)]
    static void UpdateGameCatalog()
    {
        var catalog = File.ReadAllText(Path.Combine(Settings.buildSettings.ShipbreakerPath, "Shipbreaker_Data\\StreamingAssets\\aa\\catalog.json"));

        catalog = catalog.Replace(@"{UnityEngine.AddressableAssets.Addressables.RuntimePath}\\StandaloneWindows64\\", (Path.Combine(Settings.buildSettings.ShipbreakerPath, "Shipbreaker_Data\\StreamingAssets\\aa\\StandaloneWindows64") + "\\").Replace("\\", "\\\\"));

        var path = System.IO.Path.GetFullPath(Application.dataPath + "\\..\\modded_catalog.json");

        File.WriteAllText(path, catalog);
        Debug.Log($"Game catalog recreated and written to {path}");
    }

    [MenuItem("Shipbreaker/Update known assets", priority = 1001)]
    static void UpdateKnownAssets()
    {
        List<string> output = new List<string>() { "{" };
        var knownLocations = new Dictionary<string, bool>();
        foreach (var loc in UnityEngine.AddressableAssets.Addressables.ResourceLocators)
        {
            foreach (var key in loc.Keys)
            {
                if (!loc.Locate(key, typeof(object), out var resourceLocations))
                    continue;

                if (key.ToString().Length == 32)
                {
                    output.Add($"\"{key}\": \"{resourceLocations.First().InternalId}\",");
                }
            }
        }

        output[output.Count - 1] = output[output.Count - 1].TrimEnd(',');
        output.Add("}");

        var path = System.IO.Path.GetFullPath(Application.dataPath + "\\..\\known_assets.json");

        File.WriteAllLines(path, output);
        Debug.Log($"Known asset list recreated and written to {path}");
    }

    [MenuItem("Shipbreaker/Reload Build Settings", priority = 1002)]
    static void ReloadBuildSettings()
    {
        Settings.ReloadBuildSettings();
    }

    public class Manifest
    {
        public readonly int version = MANIFEST_VERSION;
        public string[] baseOverrides;
    }
}
