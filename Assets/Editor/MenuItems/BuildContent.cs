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

[InitializeOnLoad]
public class BuildContent
{
    static BuildSettings buildSettings;
    static BuildContent()
    {
        ReloadBuildSettings();
    }

    [MenuItem("Shipbreaker/Reload Build Settings", priority = 1001)]
    static void ReloadBuildSettings()
    {
        var settingsText = File.ReadAllText(Path.Combine(Application.dataPath, "shipbreaker_settings.json"));
        buildSettings = Newtonsoft.Json.JsonConvert.DeserializeObject<BuildSettings>(settingsText);

        Debug.Log("Build settings reloaded");
    }

    [MenuItem("Shipbreaker/Build", priority = 2)]
    static void RunBuild()
    {
        AddressableAssetSettingsDefaultObject.Settings.activeProfileId = AddressableAssetSettingsDefaultObject.Settings.profileSettings.GetProfileId("Default");

        AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);
        bool success = string.IsNullOrEmpty(result.Error);

        if (success)
        {
            Debug.Log("Moving bundle");

            var contentBundle = Directory.GetFiles(Application.dataPath + "\\..\\BuiltShipContent")[0];
            File.Copy(
                contentBundle,
                Path.Combine(buildSettings.ShipbreakerPath, "BepInEx\\plugins\\TestProj", buildSettings.ShipPath, contentBundle.Split('\\').Last()),
                true
            );
            File.Delete(contentBundle);

            Debug.Log("Moving and modifying catalog");
            var catalog = File.ReadAllText(Application.dataPath + "\\..\\Library\\com.unity.addressables\\aa\\Windows\\catalog.json");

            catalog = catalog.Replace("BuiltShipContent\\\\", (Path.Combine(buildSettings.ShipbreakerPath, "BepInEx\\plugins\\TestProj\\", buildSettings.ShipPath) + "\\").Replace("\\", "\\\\"));

            File.WriteAllText(Path.Combine(buildSettings.ShipbreakerPath, "BepInEx\\plugins\\TestProj", buildSettings.ShipPath, "catalog.json"), catalog);

            LoadGameAssets.ReloadAssets();
            Debug.Log("Build Complete");
        }
        else
        {
            Debug.LogError("Addressables build error encountered: " + result.Error);
        }
    }

    [MenuItem("Shipbreaker/Build and run", priority = 3)]
    static void BuildAndRun()
    {
        RunBuild();
        System.Diagnostics.Process.Start(Path.Combine(buildSettings.ShipbreakerPath, "Shipbreaker.exe"));
    }

    [MenuItem("Shipbreaker/Update game catalog", priority = 1000)]
    static void UpdateGameCatalog()
    {
        var catalog = File.ReadAllText(Path.Combine(buildSettings.ShipbreakerPath, "Shipbreaker_Data\\StreamingAssets\\aa\\catalog.json"));

        catalog = catalog.Replace(@"{UnityEngine.AddressableAssets.Addressables.RuntimePath}/StandaloneWindows64\\", (Path.Combine(buildSettings.ShipbreakerPath, "Shipbreaker_Data\\StreamingAssets\\aa\\StandaloneWindows64") + "\\").Replace("\\", "\\\\"));

        File.WriteAllText(Application.dataPath + "\\..\\modded_catalog.json", catalog);
    }

    class BuildSettings
    {
        public string ShipbreakerPath;
        public string ShipPath;
    }
}
