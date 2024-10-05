using System.IO;
using UnityEngine;

public class Settings
{
    public static BuildSettings buildSettings { get; private set; }

    public static void ReloadBuildSettings()
    {
        var settingsText = File.ReadAllText(Path.GetFullPath(Path.Combine(Application.dataPath, "..", "shipbreaker_settings.json")));

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
        // show all the applicable messages, not just one-by-one
        bool valid = true;

        if(buildSettings.Author == null || buildSettings.Author == "")
        {
            Debug.LogError("Please provide an author name");
            valid = false;
        }
        else if(buildSettings.ShipbreakerPath == null || buildSettings.ShipbreakerPath == "")
        {
            Debug.LogError("Please provide the full path to Hardspace: Shipbreaker, and make sure it uses double backslashes (example: \"C:\\\\GOG Games\\\\Hardspace Shipbreaker\")");
            valid = false;
        }
        
        if (SystemInfo.operatingSystem.ToLower().Contains("linux"))
        {
            if (!buildSettings.ShipbreakerPath.Contains("/") || buildSettings.ShipbreakerPath.Contains("\\"))
            {
                Debug.LogError("Please provide the full Linux path to Hardspace: Shipbreaker (example: \"/home/myUser/games/gog_games/drive_c/GOG Games/Hardspace Shipbreaker\")");
                valid = false;
            }
            if (buildSettings.WindowsShipbreakerPathOnLinux == null || buildSettings.WindowsShipbreakerPathOnLinux == "" || !buildSettings.WindowsShipbreakerPathOnLinux.Contains("\\"))
            {
                Debug.LogError("Please provide WindowsShipbreakerPathOnLinux as the Windows path seen by the game, and make sure it uses double backslashes (example: \"C:\\\\GOG Games\\\\Hardspace Shipbreaker\")");
                valid = false;
            }
        }
        else
        {
            if(buildSettings.ShipbreakerPath[2] != '\\')
            {
                Debug.LogError("Please escape the path to Hardspace: Shipbreaker (Double backslashes \\\\, not single \\)");
                valid = false;
            }
        }
        
        if (!File.Exists(Path.Combine(buildSettings.ShipbreakerPath, "Shipbreaker.exe")))
        {
            Debug.LogError("Please provide a valid path to Hardspace: Shipbreaker (EXE not found)");
            valid = false;
        }
        else if (!Directory.Exists(Path.Combine(buildSettings.ShipbreakerPath, "BepInEx")))
        {
            Debug.LogError("Please ensure BepInEx is installed (BepInEx folder not found)");
            valid = false;
        }
        else if (!Directory.Exists(Path.Combine(buildSettings.ShipbreakerPath, "BepInEx", "plugins", "ModdedShipLoader")))
        {
            Debug.LogError("Please ensure the ModdedShipLoader is installed (plugin folder not found)");
            valid = false;
        }
        else if (!Directory.Exists(Path.Combine(buildSettings.ShipbreakerPath, "BepInEx", "patchers", "ModdedShipLoaderPatcher")))
        {
            Debug.LogError("Please ensure the ModdedShipLoaderPatcher is installed (patcher folder not found)");
            valid = false;
        }

        return valid;
    }

    public static string GetAuthorName()
    {
        return buildSettings.Author;
    }

    public class BuildSettings
    {
        public string ShipbreakerPath;
        public string WindowsShipbreakerPathOnLinux;
        public string Author;
    }
}
