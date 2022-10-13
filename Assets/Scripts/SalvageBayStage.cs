using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

/*
    Load the in-game bay, to find the right place to put the ship
*/
public class SalvageBayStage : MonoBehaviour
{
    public bool reload = false;

    GameObject bay;
    GameObject jack;
    GameObject visibleBay;
    GameObject visibleJack;

    void OnValidate()
    {
        if(reload)
        {
            reload = false;
            LoadBay();
        }
    }

    void LoadBay()
    {
        if(bay == null || jack == null)
        {
            Addressables.LoadAssetsAsync<GameObject>(((IEnumerable)new AssetReferenceGameObject[] {new AssetReferenceGameObject("4e97499d6abdd314a83597595300db8d"), new AssetReferenceGameObject("9d2adb8555f4e794d874d4c67f643547") }), go => {
                if(go.name == "PRF_HexStation_Composite")
                    bay = go;
                if(go.name == "PRF_MasterJack")
                    jack = go;
                
                if(bay != null && jack != null)
                    RenderBay();
            }, Addressables.MergeMode.Union);
        }
        else
        {
            RenderBay();
        }
    }

    void RenderBay()
    {
        visibleBay = GameObject.Instantiate(bay, new Vector3(21.83f, 11.8f, 35.9f), Quaternion.identity, transform);
        visibleJack = GameObject.Instantiate(jack, new Vector3(22.35f, 9.5f, 112.9f), Quaternion.Euler(0, -90, 0), transform);
    }
}
