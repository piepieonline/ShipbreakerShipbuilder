using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[InitializeOnLoad]
public class LoadGameAssets
{
    static LoadGameAssets()
    {
        ReloadAssets();
    }

    static void ReloadAssets()
    {
        if (LoadAddressables.handle1.IsValid()) Addressables.Release(LoadAddressables.handle1);
        if (LoadAddressables.handle2.IsValid()) Addressables.Release(LoadAddressables.handle2);

        LoadAddressables.handle1 = Addressables.LoadContentCatalogAsync("D:\\Games\\Xbox\\Hardspace- Shipbreaker\\Content\\BepInEx\\plugins\\TestProj\\catalog.json", false);
        LoadAddressables.handle2 = Addressables.LoadContentCatalogAsync("D:\\UnityDev\\ShipbreakerModding\\modded_catalog.json", false);

        LoadAddressables.handle1.Completed += status => { Debug.Log($"Loading 1 complete: Valid: {status.IsValid()}"); };
        LoadAddressables.handle2.Completed += status => { Debug.Log($"Loading 2 complete: Valid: {status.IsValid()}"); };
    }
}
