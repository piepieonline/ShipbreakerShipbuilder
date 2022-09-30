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

    [MenuItem("Shipbreaker/Build", priority = 2)]
    static bool RunBuild()
    {
        AddressableAssetSettingsDefaultObject.Settings.activeProfileId = AddressableAssetSettingsDefaultObject.Settings.profileSettings.GetProfileId("Default");

        AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);
        bool success = string.IsNullOrEmpty(result.Error);

        if (success && VerifyBuildSettings())
        {
            var modPath = "BepInEx\\plugins\\ModdedShipLoader\\Ships";
            var shipPath = $"{buildSettings.ShipPath}.{buildSettings.Author}";

            Debug.Log("Creating or clearing build directory");

            if(Directory.Exists(Path.Combine(buildSettings.ShipbreakerPath, modPath, shipPath)))
            {
                foreach(var file in Directory.EnumerateFiles(Path.Combine(buildSettings.ShipbreakerPath, modPath, shipPath)))
                {
                    if(file.EndsWith(".json") || file.EndsWith(".bundle"))
                    {
                        File.Delete(file);
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(Path.Combine(buildSettings.ShipbreakerPath, modPath, shipPath));
            }

            Debug.Log("Moving bundle");

            var contentBundle = Directory.GetFiles(Application.dataPath + "\\..\\BuiltShipContent")[0];
            File.Copy(
                contentBundle,
                Path.Combine(buildSettings.ShipbreakerPath, modPath, shipPath, contentBundle.Split('\\').Last()),
                true
            );
            File.Delete(contentBundle);

            Debug.Log("Moving and modifying catalog");
            var catalog = File.ReadAllText(Application.dataPath + "\\..\\Library\\com.unity.addressables\\aa\\Windows\\catalog.json");

            catalog = catalog.Replace("BuiltShipContent\\\\", (Path.Combine(buildSettings.ShipbreakerPath, modPath, shipPath) + "\\").Replace("\\", "\\\\"));

            File.WriteAllText(Path.Combine(buildSettings.ShipbreakerPath, modPath, shipPath, "catalog.json"), catalog);

            LoadGameAssets.ReloadAssets();
            Debug.Log("Build Complete");
            return true;
        }
        else
        {
            Debug.LogError("Addressables build error encountered: " + result.Error);
            return false;
        }
    }

    [MenuItem("Shipbreaker/Build and run", priority = 3)]
    static void BuildAndRun()
    {
        if(RunBuild())
            System.Diagnostics.Process.Start(Path.Combine(buildSettings.ShipbreakerPath, "Shipbreaker.exe"));
    }

    [MenuItem("Shipbreaker/Update game catalog", priority = 1000)]
    static void UpdateGameCatalog()
    {
        var catalog = File.ReadAllText(Path.Combine(buildSettings.ShipbreakerPath, "Shipbreaker_Data\\StreamingAssets\\aa\\catalog.json"));

        catalog = catalog.Replace(@"{UnityEngine.AddressableAssets.Addressables.RuntimePath}\\StandaloneWindows64\\", (Path.Combine(buildSettings.ShipbreakerPath, "Shipbreaker_Data\\StreamingAssets\\aa\\StandaloneWindows64") + "\\").Replace("\\", "\\\\"));

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
        var settingsText = File.ReadAllText(Path.Combine(Application.dataPath, "../shipbreaker_settings.json"));

        buildSettings = null;
        try
        {
            buildSettings = Newtonsoft.Json.JsonConvert.DeserializeObject<BuildSettings>(settingsText);
        }
        catch (Newtonsoft.Json.JsonReaderException)
        {
            Debug.LogError("Invalid settings file (failed to read)");
            return;
        }

        if(VerifyBuildSettings())
        {
            Debug.Log("Build settings reloaded");
        }
        else
        {
            Debug.LogError("Build settings failed to load");
        }

    }

    static bool VerifyBuildSettings()
    {
        if(buildSettings.Author == null || buildSettings.Author == "")
        {
            Debug.LogError("Please provide an author name");
            return false;
        }
        else if(buildSettings.ShipPath == null || buildSettings.ShipPath == "")
        {
            Debug.LogError("Please provide a Ship Path");
            return false;
        }
        else if(buildSettings.ShipbreakerPath == null || buildSettings.ShipbreakerPath == "")
        {
            Debug.LogError("Please provide a path to Hardspace: Shipbreaker");
            return false;
        }
        else if(buildSettings.ShipbreakerPath[2] != '\\')
        {
            Debug.LogError("Please escape the path to Hardspace: Shipbreaker (Double backslashes \\\\, not single \\)");
            return false;
        }
        else if (!File.Exists(Path.Combine(buildSettings.ShipbreakerPath, "Shipbreaker.exe")))
        {
            Debug.LogError("Please provide a valid path to Hardspace: Shipbreaker (EXE not found)");
            return false;
        }

        return true;
    }

    public static string GetAuthorName()
    {
        return buildSettings.Author;
    }

    class BuildSettings
    {
        public string ShipbreakerPath;
        public string Author;
        public string ShipPath;
    }
}
