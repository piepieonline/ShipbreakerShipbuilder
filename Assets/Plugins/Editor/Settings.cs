using System.IO;
using UnityEngine;

public class Settings
{
    public static BuildSettings buildSettings { get; private set; }

    public static void ReloadBuildSettings()
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

    public static bool VerifyBuildSettings()
    {
        if(buildSettings.Author == null || buildSettings.Author == "")
        {
            Debug.LogError("Please provide an author name");
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
        else if (!Directory.Exists(Path.Combine(buildSettings.ShipbreakerPath, "BepInEx")))
        {
            Debug.LogError("Please ensure BepInEx is installed (BepInEx folder not found)");
            return false;
        }
        else if (!Directory.Exists(Path.Combine(buildSettings.ShipbreakerPath, "BepInEx")))
        {
            Debug.LogError("Please ensure BepInEx is installed (BepInEx folder not found)");
            return false;
        }
        else if (!Directory.Exists(Path.Combine(buildSettings.ShipbreakerPath, "BepInEx", "plugins", "ModdedShipLoader")))
        {
            Debug.LogError("Please ensure the ModdedShipLoader is installed (plugin folder not found)");
            return false;
        }
        else if (!Directory.Exists(Path.Combine(buildSettings.ShipbreakerPath, "BepInEx", "patchers", "ModdedShipLoaderPatcher")))
        {
            Debug.LogError("Please ensure the ModdedShipLoaderPatcher is installed (patcher folder not found)");
            return false;
        }

        return true;
    }

    public static string GetAuthorName()
    {
        return buildSettings.Author;
    }

    public class BuildSettings
    {
        public string ShipbreakerPath;
        public string Author;
    }
}
