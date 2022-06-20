#if UNITY_EDITOR

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using BBI.Unity.Game;


using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadAddressables : MonoBehaviour
{
    public bool loadGO = false;
    public bool loadSO = false;
    public bool loadBundle = false;

    public string address;

    public GameObject loaded;
    public ScriptableObject toClone;

    public static LoadAddressables instance;

    // public List<UnityEngine.Object> loadedObjects = new List<UnityEngine.Object>();
    public List<GameObject> loadedObjects = new List<GameObject>();

    public static Dictionary<string, AssetBundle> loadedBundles = new Dictionary<string, AssetBundle>();

    public static AsyncOperationHandle<UnityEngine.AddressableAssets.ResourceLocators.IResourceLocator> handle1;
    public static AsyncOperationHandle<UnityEngine.AddressableAssets.ResourceLocators.IResourceLocator> handle2;

    public static AsyncOperationHandle<GameObject> GameObjectInstantiateReady(AsyncOperationHandle<GameObject> arg)
    {
        Debug.Log($"doin' work {System.DateTime.Now}");

        if(arg.Result.TryGetComponent<BBI.Unity.Game.AddressableLoader>(out var loader))
        {
            for(int i = 0; i < loader.refs.Count; i++)
            {
                Addressables.ResourceManager.CreateChainOperation<GameObject, GameObject>(
                    Addressables.InstantiateAsync(new AssetReferenceGameObject(loader.refs[i]), arg.Result.transform.GetChild(i)), GameObjectInstantiateReady)
                ;
            }
        }

        arg.Result.hideFlags = HideFlags.DontSaveInEditor;

        Debug.Log($"done work {System.DateTime.Now}");
        
        return Addressables.ResourceManager.CreateCompletedOperation(arg.Result, string.Empty);
    }

    /*
    public static AsyncOperationHandle<GameObject> GameObjectLoadReady(AsyncOperationHandle<GameObject> arg)
    {
        Debug.Log($"doin' work {System.DateTime.Now}");

        if (arg.Result.TryGetComponent<BBI.Unity.Game.AddressableLoader>(out var loader))
        {
            for (int i = 0; i < loader.refs.Count; i++)
            {
                Addressables.ResourceManager.CreateChainOperation<GameObject, GameObject>(
                    Addressables.LoadAssetAsync<GameObject>(new AssetReferenceGameObject(loader.refs[i]), arg.Result.transform.GetChild(i)), GameObjectLoadReady)
                ;
            }
        }

        arg.Result.hideFlags = HideFlags.DontSaveInEditor;

        Debug.Log($"done work {System.DateTime.Now}");

        return Addressables.ResourceManager.CreateCompletedOperation(arg.Result, string.Empty);
    }
    */

    void OnValidate()
    {
        instance = this;

        address = address.Trim();

        if (loadGO)
        {
            // Addressables.ResourceManager.CreateChainOperation<GameObject, GameObject>(Addressables.InstantiateAsync(new AssetReferenceGameObject(address)), GameObjectInstantiateReady);
            // Addressables.ResourceManager.CreateChainOperation<GameObject, GameObject>(Addressables.LoadAssetAsync<GameObject>(new AssetReferenceGameObject(address)), GameObjectLoadReady);

            UnityEditor.SceneManagement.StageUtility.GoToStage(new CustomStage(), true);

            
            Addressables.LoadAssetAsync<GameObject>(new AssetReferenceGameObject(address)).Completed += res =>
            {
                CustomStage.go = res.Result;
                UnityEditor.SceneManagement.StageUtility.GoToStage(new CustomStage(), true);
                // UnityEditor.AssetDatabase.OpenAsset(res.Result);
            };
            
            /*
            Addressables.ResourceManager.StartOperation<GameObject>(new CustomOp(), Addressables.LoadAssetAsync<GameObject>(new AssetReferenceGameObject(address))).Completed += res =>
            {
                Debug.Log($"spawning {System.DateTime.Now}");
                Instantiate(res.Result);
            };
            */

            // Addressables.InstantiateAsync(new AssetReferenceGameObject(address));
            loadGO = false;
        }

        if (loadSO)
        {
            Addressables.LoadAssetAsync<ScriptableObject>(new AssetReferenceScriptableObject(address)).Completed += res =>
            {
                toClone = res.Result;
            };
            loadSO = false;
        }

        if(loadBundle)
        {
            loadBundle = false;

            Addressables.LoadAssetAsync<UnityEngine.Object>(new AssetReference(address)).Completed += res =>
            {
                Debug.Log(res.Result);
            };
        }
    }

    public static string[] bundles = new string[] {
        "animation_assets_all_9a8a3f2db3e66a55adf4bd7589e77e97.bundle",
"blueprints_assets_all_4a519e9c54a97543acf98b89d2724a93.bundle",
"commontextures_assets_all_8176c693bdc932ddf20a3dc3cf71a440.bundle",
"companies_assets_company_aquarius_c91e006ee2f20bbe972dd0bcafcbfff2.bundle",
"companies_assets_company_bpm_e5c3ef2207cd68cb52b4a57cdae91d59.bundle",
"companies_assets_company_delhi_dee9de8fe6d019b3190438b7d4d7408b.bundle",
"companies_assets_company_directways_7d59b69ccce396a161c37df1dff1cf41.bundle",
"companies_assets_company_expedient_974f7fbb60329a2880d253acbb7d8019.bundle",
"companies_assets_company_ferrosrojo_3947861afe5ec0a94d1ef064e872f1f7.bundle",
"companies_assets_company_hanzo_572ba1056a72886d533da5c0980f56bf.bundle",
"companies_assets_company_helix_d57517da6fe7da4caf7b9a705dcef2c4.bundle",
"companies_assets_company_hudson_fc53e035d24239a12735f57835e29dea.bundle",
"companies_assets_company_imperial_45e0920c2af0b696108e3306bea41b90.bundle",
"companies_assets_company_interlink_026eab00f848986f62a82604eeb015e3.bundle",
"companies_assets_company_klyneco_9c411c230728764a132f43c101c820b6.bundle",
"companies_assets_company_lynxrail_fff57a3047b53bce20c3fcc5528de231.bundle",
"companies_assets_company_lynxsalvage_bd68860e086f4e8699cbeee962443ce7.bundle",
"companies_assets_company_nova_857e3d7ffe3b5d7ee54759271c65e154.bundle",
"companies_assets_company_samsara_e771a6d87a9fc9ff73a9803ea1d17446.bundle",
"companies_assets_company_tiber_f8cfef9a81cc6b82e43c57c6a97d9f2f.bundle",
"companies_assets__cb2dc024ac9cf0af14ba310749305140.bundle",
"core_assets_all_5c1416ca2659a4fbf22fd587fa92f852.bundle",
"decals-modulelist_assets_all_b15edeb76c6244c7ff72436307881ec2.bundle",
"decals-unsorted_assets_all_948b7f9f71ff3970ec6f9c6007b8dfb0.bundle",
"defaultlocalgroup_monoscripts_14b6cb5686ea50e07c51b9258a89b1a6.bundle",
"defaultlocalgroup_unitybuiltinshaders_785bfb600640884d8763e8e2efb730cd.bundle",
"dev-assets_assets_all_56ab90504f2c3b8ecc07a641a288fc3e.bundle",
"dirtyair_assets_all_4c41cda10571d996e8e2f6b47da53cc2.bundle",
"hab-spacetruck_assets_all_3c5157f9452d10f648c736c9f1399c26.bundle",
"hab-ui_assets_all_98b93ed314ae686be2adbd1b58818250.bundle",
"levels_assets_all_28c11886d6053f2299fca1350473f7fd.bundle",
"materials-debug_assets_all_6aff17cc3a060ce8419855bfb384a326.bundle",
"materials-gameplay_assets_all_98a0f0751455f39be2e8245bc07f8df4.bundle",
"materials-scanmode_assets_all_98fb1e2f57637ad2f058ad5a39511e1f.bundle",
"narrative-dialogue_assets_all_5a464d707204a457d009136be1b752fd.bundle",
"narrative_assets_all_add2372327dee0fa999e3e56ff0fb085.bundle",
"physics_assets_all_1ba72ba613f8d812200b4da25306632c.bundle",
"pickups_assets_all_813eb5eb7b1a89c6b6ed5ea2b77b36a6.bundle",
"player-base_assets_all_ca7dbb36488fe9a3248880ed422c50c0.bundle",
"player-tools_assets_all_ee2dd9f07e5a25b800cf39c0020eb865.bundle",
"portmorrigan_assets_all_7b0e160474d84f5f4cf47c1f015e82d6.bundle",
"props-kitbash_assets_all_8fac8ebd27bc05c9cecbcaf042ce9cc1.bundle",
"props-modulelist_assets_all_e67da482c84e6390af338d16eba98d6c.bundle",
"props-smallprops_assets_all_2f8af180ae57020438d18c07c34863e1.bundle",
"rendering-base_assets_all_7b3e36dceeca4b7cc4e0a7ab36585fbb.bundle",
"rendering-postprocessing_assets_all_836d67828080d7e8cc08d4d2a50a8d09.bundle",
"scenes-common_scenes_base_session_container_57c7e2bed6a3adf965dc22b4a008428f.bundle",
"scenes-devscenes_scenes_elemental_test_95b24fd6351f8cb41f456e0ec83f1720.bundle",
"scenes-devscenes_scenes_empty_4b8b318336bb3747c694e3e827949b06.bundle",
"scenes-devscenes_scenes_sc_qabed_c2d01842a77a7fb1df85d9875d58c290.bundle",
"scenes-devscenes_scenes_sc_qa_autoperftest_520f0983e8788e0ca619a9e7bdbcb76b.bundle",
"scenes-devscenes_scenes_sc_subsystem_test_25865f5f224a030cfa2b9d154641500c.bundle",
"scenes-frontend_scenes_master_frontend_e16238e835a59a3ba96ce346151cc260.bundle",
"scenes-frontend_scenes_splash_screen_f70466af5cddbb3b7fe5f452482fc5fe.bundle",
"scenes-gameplay_scenes_all_4924f7ee4512e97d0839f524ee019b32.bundle",
"shaders_assets_all_0829be482f69c82097c8125b89e93f24.bundle",
"ship-base-atlas_assets_all_2293730c583ae519eb44de29168b830b.bundle",
"ship-base-gecko_assets_all_0adb8024897c014ae97030d42b04c8b6.bundle",
"ship-base-javelin_assets_all_884b3c89ed883aa62b648a32e0722462.bundle",
"ship-base-mackerel_assets_all_146d78cf6350b954cef04c8e8b80113b.bundle",
"ship-base_assets_all_02caf5d845cb2ef5058ca332c3998bb7.bundle",
"ship-ghostship_assets_all_fc742b82b417863315f1c985f4379e6d.bundle",
"ship-kitbash-airlock_assets_all_5f233c919641a55808c55cf3c4916a95.bundle",
"ship-kitbash-atlas_structuralbeamskit_assets_all_742f2c06fa9938f77482e0adc1456a85.bundle",
"ship-kitbash-atmosphereregulator_assets_all_60f8f9e3647086ff81137ba201105f88.bundle",
"ship-kitbash-cargorack_assets_all_ad55ea87ecdd56ee9b946ef86e30b4dd.bundle",
"ship-kitbash-chairs_assets_all_c3bdde8efa9394d79434120a70f922e9.bundle",
"ship-kitbash-cockpita_assets_all_b1c1c55de1573533f774cf22fe72e719.bundle",
"ship-kitbash-cockpit_assets_all_d1c5bce5a10e2a4d80ca29b9fa8b8a0a.bundle",
"ship-kitbash-computers_assets_all_61fda1825a67fe356db14a7b90d711fd.bundle",
"ship-kitbash-crates_assets_all_8f8776dbecde8093aafc97a1cf4a96d9.bundle",
"ship-kitbash-cutpoints_assets_all_4fad163ff17d1affee4fad06d60cf29c.bundle",
"ship-kitbash-hoist_assets_all_44057d2eb56eceecccf32fd301e0b9ca.bundle",
"ship-kitbash-hydroponiccabinet_assets_all_39554e91f92b78ac8d6bd6f4cb5c2f4a.bundle",
"ship-kitbash-junctionbox_assets_all_d42f99a34c64d8e4144b19bfdcab9630.bundle",
"ship-kitbash-ladderrailings_assets_all_8ada3b2cdeb2d55296c2d4df05309db9.bundle",
"ship-kitbash-lights_assets_all_d249f33bf9e96a4df582bcacb846f70f.bundle",
"ship-kitbash-materialsandtexturescommon_assets_all_b71700966e715fa56a0c7602ec2e1353.bundle",
"ship-kitbash-modelscommon_assets_all_113f0127df39d1baa13fa289ed88fd12.bundle",
"ship-kitbash-panels_assets_all_6751d6c2f2e7999e6743c4810e5b62cb.bundle",
"ship-kitbash-pipes_assets_all_1fafe80814d25189574801c4fc0af3d6.bundle",
"ship-kitbash-plugs_assets_all_7cdee7a0a1bc72d32058627b7ffa2f52.bundle",
"ship-kitbash-possiblyunused_assets_all_5a12327b8be413b49c490a20233f0a15.bundle",
"ship-kitbash-reinforcementbeams_assets_all_caf1f10dbf9fae47394a9618392fe58a.bundle",
"ship-kitbash-terminals_assets_all_4c48af493b72018c578f7eb7e3b1144b.bundle",
"ship-kitbash-thuster_assets_all_3921abd0e80373e56ea1b677e548d47f.bundle",
"ship-kitbash-unsorted_assets_all_c94d0d864acc9dbd82b688678ee49cae.bundle",
"ship-kitbash-wallbrace_assets_all_95bdbf039f3665a1aa28fbc7911d6221.bundle",
"ship-skins_assets_archetype_atlascompany_blackcastle_9916a7446b92345986d61b84f52dbd94.bundle",
"ship-skins_assets_archetype_atlascompany_delhi_c3cc2228a720ab825efb7d692df07e7c.bundle",
"ship-skins_assets_archetype_atlascompany_expedient_91f3cc9510d7a480ce3ce8010d438d3d.bundle",
"ship-skins_assets_archetype_atlascompany_ferrosrojo_72fee61cfc8f498cd4cc29726b0f3886.bundle",
"ship-skins_assets_archetype_atlascompany_hanzo_b34b72566ddf4ecbff4347b24acbc501.bundle",
"ship-skins_assets_archetype_atlascompany_imperial_3aff3b30d4a708458085b9be3884fbf5.bundle",
"ship-skins_assets_archetype_atlascompany_scog_3b024decf52e883efcc0682ec5172b04.bundle",
"ship-skins_assets_archetype_atlascompany_tiber_7a276e8845046b82455f898b4654fa45.bundle",
"ship-skins_assets_archetype_geckocompany_ferrosrojo_cd52c253d2ae6b7a525f44a42b5a687c.bundle",
"ship-skins_assets_archetype_geckocompany_hanzo_c3dd3972429ecf1392e0c662e5c9fd69.bundle",
"ship-skins_assets_archetype_geckocompany_helix_b3b0f2424bed8256fe428dd08cee5dd2.bundle",
"ship-skins_assets_archetype_geckocompany_hudson_84f64a05b71c0fd63fa8a3f682a3cf3c.bundle",
"ship-skins_assets_archetype_geckocompany_imperial_528127da9b8dbb87d30c040b3c960d9a.bundle",
"ship-skins_assets_archetype_geckocompany_lynxrail_1e3911f36421459b92f186abaf8757a1.bundle",
"ship-skins_assets_archetype_geckocompany_lynxsalvage_982033aae9105f33657af8860ec14b3d.bundle",
"ship-skins_assets_archetype_geckocompany_tiber_15e99213d29d95e025f4efabfa895e22.bundle",
"ship-skins_assets_archetype_geckoghostship_3ceede0bcbd3ae2e9badafc6e78c217d.bundle",
"ship-skins_assets_archetype_gecko_dbdff3d2a3e559361f5de23f3bba0945.bundle",
"ship-skins_assets_archetype_jaavelincompany_aquarius_5cc8d53753be1400cfffcec6ce25984f.bundle",
"ship-skins_assets_archetype_jaavelincompany_directways_32c37463d3e4419980db0011f6c9cf1b.bundle",
"ship-skins_assets_archetype_jaavelincompany_ferrosrojo_cc4f5e0e1e200ffc0e4e3c48ae2be169.bundle",
"ship-skins_assets_archetype_jaavelincompany_hanzo_e5bbdef357550d1eab5e66f2135605d0.bundle",
"ship-skins_assets_archetype_jaavelincompany_helix_12681ca3ab2c017989f9cc0e005385c7.bundle",
"ship-skins_assets_archetype_jaavelincompany_hudson_72592b132030978e1ccdb9f5df6c359b.bundle",
"ship-skins_assets_archetype_jaavelincompany_lynxrail_4fa32b0889fb1d11e052c597c0d0c437.bundle",
"ship-skins_assets_archetype_jaavelincompany_tiber_f3e85bbb1e5cadd348c419d11c284a0f.bundle",
"ship-skins_assets_archetype_jaavelinghostship_69a2e6efdf863ce00cf4754e7b89617e.bundle",
"ship-skins_assets_archetype_mackerelcompany_aquarius_5fb2fccfd3ff988ef05fef6a9ffbe930.bundle",
"ship-skins_assets_archetype_mackerelcompany_directways_6baf4e06ba31d89bba9db1ba3d8b5081.bundle",
"ship-skins_assets_archetype_mackerelcompany_expedient_7ecffe37ab3c3dacea04c3a3ef34bfab.bundle",
"ship-skins_assets_archetype_mackerelcompany_ferrosrojo_55be818c33eb4ce9f83e561aa9dfb88b.bundle",
"ship-skins_assets_archetype_mackerelcompany_hudson_92315ebec7da7e7abf61dadab34cc220.bundle",
"ship-skins_assets_archetype_mackerelcompany_imperial_e4cc00371a6bd01a961751f52a964bd1.bundle",
"ship-skins_assets_archetype_mackerelcompany_lynxrail_b64c2dea073b3ce6491c286570ab8d1b.bundle",
"ship-skins_assets_archetype_mackerelcompany_lynxsalvage_f9c148dc5d16ca1f49846f9e47c74561.bundle",
"ship-skins_assets_archetype_mackerelcompany_samsara_19eaf34c1f3af7727b47f22639eb4fa2.bundle",
"ship-skins_assets_archetype_mackerelcompany_tiber_1fdf652f09bd70a3bff9fe370a544c5d.bundle",
"ship-skins_assets_archetype_mackerelghostship_400283cc62d247f6f84ae8a5c9d6ce8a.bundle",
"ship-weeklyship_assets_all_2b1358283adc787b2d1ae5d246f0e11f.bundle",
"stickers_assets_all_299d4483bb9e83e2357ee099b3a21668.bundle",
"structureparts_assets_all_0f44db73da994cebb24e8b7f185a0fb7.bundle",
"tooltips_assets_all_aebb3606654044dd3d6dc79c454e6c50.bundle",
"tutorialbase_assets_all_12bc5692489231d16c6d5a5f4a537f29.bundle",
"ui-base_assets_all_1fa71a9770650baf15ddbe50561d4ccb.bundle",
"ui-fonts_assets_all_c9a62604ac1cf5cff2bd811dce25ed98.bundle",
"ui-frontend_assets_all_928d207bbd50cae4f2a4307d98c1c47c.bundle",
"ui-onboarding_assets_all_2e43ee6215d0512706fb3577c4fb2b62.bundle",
"ui-sticker_assets_all_6a5deb352a7b9da4bdf10dd84435571b.bundle",
"ui-tp_assets_all_e53a7c7c063529b05c64ec4e05037d06.bundle",
"vfx_assets_all_c692fb56334ed7cb88d603164776d8d9.bundle"
    };
}

#endif