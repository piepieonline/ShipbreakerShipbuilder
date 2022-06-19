using UnityEngine;
using UnityEditor;
using BBI.Unity.Game;

public class Create_EntityBlueprintAsset : MonoBehaviour
{
    // [MenuItem("Assets/Create/Scriptable Objects/EntityBlueprintAsset")]
    public static void CreateMyAsset(System.Type t)
    {
        ScriptableObject asset = ScriptableObject.CreateInstance(t);

        string folderPath = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
            if (folderPath.Contains("."))
                folderPath = folderPath.Remove(folderPath.LastIndexOf('/'));
                
        AssetDatabase.CreateAsset(asset, $"{folderPath}/{t.Name}.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

[MenuItem("Assets/Create/Scriptable Objects/CuttingTargetableAsset")] public static void CreateCuttingTargetableAsset() { CreateMyAsset(typeof(BBI.Unity.Game.Data.CuttingTargetableAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/AchievementAsset")] public static void CreateAchievementAsset() { CreateMyAsset(typeof(BBI.Unity.Game.AchievementAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/AirPressureAsset")] public static void CreateAirPressureAsset() { CreateMyAsset(typeof(BBI.Unity.Game.AirPressureAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/AirPressureOxygenMutatorAsset")] public static void CreateAirPressureOxygenMutatorAsset() { CreateMyAsset(typeof(BBI.Unity.Game.AirPressureOxygenMutatorAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/AirPressureStateAsset")] public static void CreateAirPressureStateAsset() { CreateMyAsset(typeof(BBI.Unity.Game.AirPressureStateAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/AlignedUvMapperAsset")] public static void CreateAlignedUvMapperAsset() { CreateMyAsset(typeof(BBI.Unity.Game.AlignedUvMapperAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/AsphyxiationMutatorAsset")] public static void CreateAsphyxiationMutatorAsset() { CreateMyAsset(typeof(BBI.Unity.Game.AsphyxiationMutatorAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/AssetSaveKeyConfigAsset")] public static void CreateAssetSaveKeyConfigAsset() { CreateMyAsset(typeof(BBI.Unity.Game.AssetSaveKeyConfigAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/AudioClusterSettingsAsset")] public static void CreateAudioClusterSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.AudioClusterSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/AudioCollisionAsset")] public static void CreateAudioCollisionAsset() { CreateMyAsset(typeof(BBI.Unity.Game.AudioCollisionAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/AudioDebugInfoAsset")] public static void CreateAudioDebugInfoAsset() { CreateMyAsset(typeof(BBI.Unity.Game.AudioDebugInfoAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/AudioDebugSettingsAsset")] public static void CreateAudioDebugSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.AudioDebugSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/AudioMaterialAsset")] public static void CreateAudioMaterialAsset() { CreateMyAsset(typeof(BBI.Unity.Game.AudioMaterialAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/AudioMotionTrackerSettingsAsset")] public static void CreateAudioMotionTrackerSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.AudioMotionTrackerSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/AudioProximityAsset")] public static void CreateAudioProximityAsset() { CreateMyAsset(typeof(BBI.Unity.Game.AudioProximityAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/AudioServiceSettingsAsset")] public static void CreateAudioServiceSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.AudioServiceSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/BackButtonSettingsAsset")] public static void CreateBackButtonSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.BackButtonSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/BargeSettingsAsset")] public static void CreateBargeSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.BargeSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/BreachForceSettingsAsset")] public static void CreateBreachForceSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.BreachForceSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/BreakableJointComponentAsset")] public static void CreateBreakableJointComponentAsset() { CreateMyAsset(typeof(BBI.Unity.Game.BreakableJointComponentAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/BreakableSettingsAsset")] public static void CreateBreakableSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.BreakableSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/BuffableAttributeAsset")] public static void CreateBuffableAttributeAsset() { CreateMyAsset(typeof(BBI.Unity.Game.BuffableAttributeAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/CameraAsset")] public static void CreateCameraAsset() { CreateMyAsset(typeof(BBI.Unity.Game.CameraAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/CareerLevelSettingsAsset")] public static void CreateCareerLevelSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.CareerLevelSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/CategoryAsset")] public static void CreateCategoryAsset() { CreateMyAsset(typeof(BBI.Unity.Game.CategoryAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/CertificationLevelSettingsAsset")] public static void CreateCertificationLevelSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.CertificationLevelSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/CertificationSettingsAsset")] public static void CreateCertificationSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.CertificationSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ColorMapperBase")] public static void CreateColorMapperBase() { CreateMyAsset(typeof(BBI.Unity.Game.ColorMapperBase)); }
[MenuItem("Assets/Create/Scriptable Objects/CompanyInfoAsset")] public static void CreateCompanyInfoAsset() { CreateMyAsset(typeof(BBI.Unity.Game.CompanyInfoAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/CompanyShipNameAsset")] public static void CreateCompanyShipNameAsset() { CreateMyAsset(typeof(BBI.Unity.Game.CompanyShipNameAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ConditionThresholdAsset")] public static void CreateConditionThresholdAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ConditionThresholdAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ControlRebindSettingsAsset")] public static void CreateControlRebindSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ControlRebindSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ControlSpriteAsset")] public static void CreateControlSpriteAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ControlSpriteAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ControlSpriteListAsset")] public static void CreateControlSpriteListAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ControlSpriteListAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ControlSriteSettingsAsset")] public static void CreateControlSriteSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ControlSriteSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/CrushingSettingsAsset")] public static void CreateCrushingSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.CrushingSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/CurrencyAsset")] public static void CreateCurrencyAsset() { CreateMyAsset(typeof(BBI.Unity.Game.CurrencyAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/CurveAsset")] public static void CreateCurveAsset() { CreateMyAsset(typeof(BBI.Unity.Game.CurveAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/CutExecutionAsset")] public static void CreateCutExecutionAsset() { CreateMyAsset(typeof(BBI.Unity.Game.CutExecutionAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/CutGradeColorData")] public static void CreateCutGradeColorData() { CreateMyAsset(typeof(BBI.Unity.Game.CutGradeColorData)); }
[MenuItem("Assets/Create/Scriptable Objects/CutLineAsset")] public static void CreateCutLineAsset() { CreateMyAsset(typeof(BBI.Unity.Game.CutLineAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/CutterAsset")] public static void CreateCutterAsset() { CreateMyAsset(typeof(BBI.Unity.Game.CutterAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/CuttingToolAsset")] public static void CreateCuttingToolAsset() { CreateMyAsset(typeof(BBI.Unity.Game.CuttingToolAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/DamageAsset")] public static void CreateDamageAsset() { CreateMyAsset(typeof(BBI.Unity.Game.DamageAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/DamageDebugSettingsAsset")] public static void CreateDamageDebugSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.DamageDebugSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/DamageTypeAsset")] public static void CreateDamageTypeAsset() { CreateMyAsset(typeof(BBI.Unity.Game.DamageTypeAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/DebtInterestAsset")] public static void CreateDebtInterestAsset() { CreateMyAsset(typeof(BBI.Unity.Game.DebtInterestAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/DebtMilestoneAsset")] public static void CreateDebtMilestoneAsset() { CreateMyAsset(typeof(BBI.Unity.Game.DebtMilestoneAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/DemoChargeAsset")] public static void CreateDemoChargeAsset() { CreateMyAsset(typeof(BBI.Unity.Game.DemoChargeAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/DemoChargeControllerAsset")] public static void CreateDemoChargeControllerAsset() { CreateMyAsset(typeof(BBI.Unity.Game.DemoChargeControllerAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/DifficultyModeAsset")] public static void CreateDifficultyModeAsset() { CreateMyAsset(typeof(BBI.Unity.Game.DifficultyModeAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/DifficultyModeSettingsAsset")] public static void CreateDifficultyModeSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.DifficultyModeSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/DispositionAsset")] public static void CreateDispositionAsset() { CreateMyAsset(typeof(BBI.Unity.Game.DispositionAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/DissolveSettingsAsset")] public static void CreateDissolveSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.DissolveSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/DoorAsset")] public static void CreateDoorAsset() { CreateMyAsset(typeof(BBI.Unity.Game.DoorAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/DoorGateAsset")] public static void CreateDoorGateAsset() { CreateMyAsset(typeof(BBI.Unity.Game.DoorGateAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/DustViewAsset")] public static void CreateDustViewAsset() { CreateMyAsset(typeof(BBI.Unity.Game.DustViewAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/DynamicLightColorAsset")] public static void CreateDynamicLightColorAsset() { CreateMyAsset(typeof(BBI.Unity.Game.DynamicLightColorAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/DynamicLightFlickerAsset")] public static void CreateDynamicLightFlickerAsset() { CreateMyAsset(typeof(BBI.Unity.Game.DynamicLightFlickerAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/DynamicObjectives")] public static void CreateDynamicObjectives() { CreateMyAsset(typeof(BBI.Unity.Game.DynamicObjectives)); }
[MenuItem("Assets/Create/Scriptable Objects/DynamicRoomContainerAsset")] public static void CreateDynamicRoomContainerAsset() { CreateMyAsset(typeof(BBI.Unity.Game.DynamicRoomContainerAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ElectricalHauntingComponentAsset")] public static void CreateElectricalHauntingComponentAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ElectricalHauntingComponentAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ElementalAssetBase")] public static void CreateElementalAssetBase() { CreateMyAsset(typeof(BBI.Unity.Game.ElementalAssetBase)); }
[MenuItem("Assets/Create/Scriptable Objects/ElementalSettingsAsset")] public static void CreateElementalSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ElementalSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/EntityBlueprintAsset")] public static void CreateEntityBlueprintAsset() { CreateMyAsset(typeof(BBI.Unity.Game.EntityBlueprintAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/EntityComponentDataAssetBase")] public static void CreateEntityComponentDataAssetBase() { CreateMyAsset(typeof(BBI.Unity.Game.EntityComponentDataAssetBase)); }
[MenuItem("Assets/Create/Scriptable Objects/EquipmentInfoSettingsAsset")] public static void CreateEquipmentInfoSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.EquipmentInfoSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/FXCollisionAsset")] public static void CreateFXCollisionAsset() { CreateMyAsset(typeof(BBI.Unity.Game.FXCollisionAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/FXMaterialAsset")] public static void CreateFXMaterialAsset() { CreateMyAsset(typeof(BBI.Unity.Game.FXMaterialAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/FogViewAsset")] public static void CreateFogViewAsset() { CreateMyAsset(typeof(BBI.Unity.Game.FogViewAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ForceOverTimeAsset")] public static void CreateForceOverTimeAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ForceOverTimeAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/GeneratedNoiseAsset")] public static void CreateGeneratedNoiseAsset() { CreateMyAsset(typeof(BBI.Unity.Game.GeneratedNoiseAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/GoalObjectiveAsset")] public static void CreateGoalObjectiveAsset() { CreateMyAsset(typeof(BBI.Unity.Game.GoalObjectiveAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/GrabAsset")] public static void CreateGrabAsset() { CreateMyAsset(typeof(BBI.Unity.Game.GrabAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/GrappleLineSettingsAsset")] public static void CreateGrappleLineSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.GrappleLineSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/GrappleVFXInstanceAsset")] public static void CreateGrappleVFXInstanceAsset() { CreateMyAsset(typeof(BBI.Unity.Game.GrappleVFXInstanceAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/GrapplingHookAsset")] public static void CreateGrapplingHookAsset() { CreateMyAsset(typeof(BBI.Unity.Game.GrapplingHookAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/GrapplingRopeAsset")] public static void CreateGrapplingRopeAsset() { CreateMyAsset(typeof(BBI.Unity.Game.GrapplingRopeAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/HabAfterShiftAsset")] public static void CreateHabAfterShiftAsset() { CreateMyAsset(typeof(BBI.Unity.Game.HabAfterShiftAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/HabSettingsAsset")] public static void CreateHabSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.HabSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/HardPointTypeAsset")] public static void CreateHardPointTypeAsset() { CreateMyAsset(typeof(BBI.Unity.Game.HardPointTypeAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/HauntingCauserComponentAsset")] public static void CreateHauntingCauserComponentAsset() { CreateMyAsset(typeof(BBI.Unity.Game.HauntingCauserComponentAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/HauntingSettingsAssset")] public static void CreateHauntingSettingsAssset() { CreateMyAsset(typeof(BBI.Unity.Game.HauntingSettingsAssset)); }
[MenuItem("Assets/Create/Scriptable Objects/HauntingThresholdComponentAsset")] public static void CreateHauntingThresholdComponentAsset() { CreateMyAsset(typeof(BBI.Unity.Game.HauntingThresholdComponentAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/HierarchySettingsAsset")] public static void CreateHierarchySettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.HierarchySettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/HighlightComponentAsset")] public static void CreateHighlightComponentAsset() { CreateMyAsset(typeof(BBI.Unity.Game.HighlightComponentAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/IRigidbodyAsset")] public static void CreateIRigidbodyAsset() { CreateMyAsset(typeof(BBI.Unity.Game.IRigidbodyAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ImpactDamageRangeAsset")] public static void CreateImpactDamageRangeAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ImpactDamageRangeAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/IndustrialActionShipAsset")] public static void CreateIndustrialActionShipAsset() { CreateMyAsset(typeof(BBI.Unity.Game.IndustrialActionShipAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/InteractableObjectAsset")] public static void CreateInteractableObjectAsset() { CreateMyAsset(typeof(BBI.Unity.Game.InteractableObjectAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/InteractionTypeAsset")] public static void CreateInteractionTypeAsset() { CreateMyAsset(typeof(BBI.Unity.Game.InteractionTypeAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/InventoryItemAsset")] public static void CreateInventoryItemAsset() { CreateMyAsset(typeof(BBI.Unity.Game.InventoryItemAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/InventoryTypeAsset")] public static void CreateInventoryTypeAsset() { CreateMyAsset(typeof(BBI.Unity.Game.InventoryTypeAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ItemQualityTypeAsset")] public static void CreateItemQualityTypeAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ItemQualityTypeAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/JointPairingAsset")] public static void CreateJointPairingAsset() { CreateMyAsset(typeof(BBI.Unity.Game.JointPairingAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/JointSetupAsset")] public static void CreateJointSetupAsset() { CreateMyAsset(typeof(BBI.Unity.Game.JointSetupAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/JointSetupInfoAsset")] public static void CreateJointSetupInfoAsset() { CreateMyAsset(typeof(BBI.Unity.Game.JointSetupInfoAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/JointabilityAsset")] public static void CreateJointabilityAsset() { CreateMyAsset(typeof(BBI.Unity.Game.JointabilityAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/LODGroupDataAsset")] public static void CreateLODGroupDataAsset() { CreateMyAsset(typeof(BBI.Unity.Game.LODGroupDataAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/LanguageSettingsAsset")] public static void CreateLanguageSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.LanguageSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/LevelAsset")] public static void CreateLevelAsset() { CreateMyAsset(typeof(BBI.Unity.Game.LevelAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/LevelListAsset")] public static void CreateLevelListAsset() { CreateMyAsset(typeof(BBI.Unity.Game.LevelListAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/LynxEditorToolbarAsset")] public static void CreateLynxEditorToolbarAsset() { CreateMyAsset(typeof(BBI.Unity.Game.LynxEditorToolbarAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/MachineEmissiveSettingsAsset")] public static void CreateMachineEmissiveSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.MachineEmissiveSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/MachinePartAsset")] public static void CreateMachinePartAsset() { CreateMyAsset(typeof(BBI.Unity.Game.MachinePartAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/MainSettingsAsset")] public static void CreateMainSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.MainSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/MandatoryConnectionAsset")] public static void CreateMandatoryConnectionAsset() { CreateMyAsset(typeof(BBI.Unity.Game.Data.Assets.BreakableJoints.MandatoryConnectionAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ModuleConstructionAsset")] public static void CreateModuleConstructionAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ModuleConstructionAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ModuleEntryContainer")] public static void CreateModuleEntryContainer() { CreateMyAsset(typeof(BBI.Unity.Game.ModuleEntryContainer)); }
[MenuItem("Assets/Create/Scriptable Objects/ModuleEntryDefinition")] public static void CreateModuleEntryDefinition() { CreateMyAsset(typeof(BBI.Unity.Game.ModuleEntryDefinition)); }
[MenuItem("Assets/Create/Scriptable Objects/ModuleListAsset")] public static void CreateModuleListAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ModuleListAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ModulePropertyAsset")] public static void CreateModulePropertyAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ModulePropertyAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ModuleSkinAsset")] public static void CreateModuleSkinAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ModuleSkinAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ModuleSkinListAsset")] public static void CreateModuleSkinListAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ModuleSkinListAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ModuleTypeAsset")] public static void CreateModuleTypeAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ModuleTypeAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ModuleVitalityListAsset")] public static void CreateModuleVitalityListAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ModuleVitalityListAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/NPCSalvageTriggerComponent")] public static void CreateNPCSalvageTriggerComponent() { CreateMyAsset(typeof(BBI.Unity.Game.NPCSalvageTriggerComponent)); }
[MenuItem("Assets/Create/Scriptable Objects/NarrativeCategoryAsset")] public static void CreateNarrativeCategoryAsset() { CreateMyAsset(typeof(BBI.Unity.Game.NarrativeCategoryAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/NarrativeContentAsset")] public static void CreateNarrativeContentAsset() { CreateMyAsset(typeof(BBI.Unity.Game.NarrativeContentAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/NarrativeEntryAsset")] public static void CreateNarrativeEntryAsset() { CreateMyAsset(typeof(BBI.Unity.Game.NarrativeEntryAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/NarrativeGeneratedAssetListAsset")] public static void CreateNarrativeGeneratedAssetListAsset() { CreateMyAsset(typeof(BBI.Unity.Game.NarrativeGeneratedAssetListAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/NarrativeMessageAsset")] public static void CreateNarrativeMessageAsset() { CreateMyAsset(typeof(BBI.Unity.Game.NarrativeMessageAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/NarrativeMessageRewardAsset")] public static void CreateNarrativeMessageRewardAsset() { CreateMyAsset(typeof(BBI.Unity.Game.NarrativeMessageRewardAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/NarrativeRandomizableSceneAsset")] public static void CreateNarrativeRandomizableSceneAsset() { CreateMyAsset(typeof(BBI.Unity.Game.NarrativeRandomizableSceneAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/NarrativeSettingsAsset")] public static void CreateNarrativeSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.NarrativeSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ObjectInfoAsset")] public static void CreateObjectInfoAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ObjectInfoAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ObjectiveEntry")] public static void CreateObjectiveEntry() { CreateMyAsset(typeof(BBI.Unity.Game.ObjectiveEntry)); }
[MenuItem("Assets/Create/Scriptable Objects/ObjectiveGroupEntry")] public static void CreateObjectiveGroupEntry() { CreateMyAsset(typeof(BBI.Unity.Game.ObjectiveGroupEntry)); }
[MenuItem("Assets/Create/Scriptable Objects/OutlineSettingsAsset")] public static void CreateOutlineSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.OutlineSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/PATConditionAsset")] public static void CreatePATConditionAsset() { CreateMyAsset(typeof(BBI.Unity.Game.PATConditionAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/PanelSealingAsset")] public static void CreatePanelSealingAsset() { CreateMyAsset(typeof(BBI.Unity.Game.PanelSealingAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/PlayerActionTrackerAsset")] public static void CreatePlayerActionTrackerAsset() { CreateMyAsset(typeof(BBI.Unity.Game.PlayerActionTrackerAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/PlayerEntityAsset")] public static void CreatePlayerEntityAsset() { CreateMyAsset(typeof(BBI.Unity.Game.PlayerEntityAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/PlayerFXAsset")] public static void CreatePlayerFXAsset() { CreateMyAsset(typeof(BBI.Unity.Game.PlayerFXAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/PlayerSettingsAsset")] public static void CreatePlayerSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.PlayerSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/PolymorphicAsset")] public static void CreatePolymorphicAsset() { CreateMyAsset(typeof(BBI.Unity.Game.PolymorphicAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/PoolObjectiveAsset")] public static void CreatePoolObjectiveAsset() { CreateMyAsset(typeof(BBI.Unity.Game.PoolObjectiveAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/PortraitDistortionCameraShakeSettings")] public static void CreatePortraitDistortionCameraShakeSettings() { CreateMyAsset(typeof(BBI.Unity.Game.PortraitDistortionCameraShakeSettings)); }
[MenuItem("Assets/Create/Scriptable Objects/PortraitDistortionSettings")] public static void CreatePortraitDistortionSettings() { CreateMyAsset(typeof(BBI.Unity.Game.PortraitDistortionSettings)); }
[MenuItem("Assets/Create/Scriptable Objects/PreloadListAsset")] public static void CreatePreloadListAsset() { CreateMyAsset(typeof(BBI.Unity.Game.PreloadListAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/PressurizationHauntingComponentAsset")] public static void CreatePressurizationHauntingComponentAsset() { CreateMyAsset(typeof(BBI.Unity.Game.PressurizationHauntingComponentAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/PropertyContainerAsset")] public static void CreatePropertyContainerAsset() { CreateMyAsset(typeof(BBI.Unity.Game.PropertyContainerAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/PushOffAsset")] public static void CreatePushOffAsset() { CreateMyAsset(typeof(BBI.Unity.Game.PushOffAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/RaycastSettingsAsset")] public static void CreateRaycastSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.RaycastSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/RentalCostsAsset")] public static void CreateRentalCostsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.RentalCostsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/RewardTiersAsset")] public static void CreateRewardTiersAsset() { CreateMyAsset(typeof(BBI.Unity.Game.RewardTiersAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/RewardTypeWeightsAsset")] public static void CreateRewardTypeWeightsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.RewardTypeWeightsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/RewardWeightsAsset")] public static void CreateRewardWeightsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.RewardWeightsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/RoomDataAsset")] public static void CreateRoomDataAsset() { CreateMyAsset(typeof(BBI.Unity.Game.RoomDataAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/RoomInfoMarkerAsset")] public static void CreateRoomInfoMarkerAsset() { CreateMyAsset(typeof(BBI.Unity.Game.RoomInfoMarkerAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/RoomSettingsAsset")] public static void CreateRoomSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.RoomSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/RoomTrackerAsset")] public static void CreateRoomTrackerAsset() { CreateMyAsset(typeof(BBI.Unity.Game.RoomTrackerAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/RoomTypeAsset")] public static void CreateRoomTypeAsset() { CreateMyAsset(typeof(BBI.Unity.Game.RoomTypeAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/SalvageBlockerBargeAsset")] public static void CreateSalvageBlockerBargeAsset() { CreateMyAsset(typeof(BBI.Unity.Game.SalvageBlockerBargeAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/SalvageBlockerProcessorAsset")] public static void CreateSalvageBlockerProcessorAsset() { CreateMyAsset(typeof(BBI.Unity.Game.SalvageBlockerProcessorAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/SalvageSettingsAsset")] public static void CreateSalvageSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.SalvageSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/SalvageableComponentAsset")] public static void CreateSalvageableComponentAsset() { CreateMyAsset(typeof(BBI.Unity.Game.SalvageableComponentAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/SaveLoadSettingsAsset")] public static void CreateSaveLoadSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.SaveLoadSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ScalpelAsset")] public static void CreateScalpelAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ScalpelAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ScanModeAsset")] public static void CreateScanModeAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ScanModeAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ScannerAsset")] public static void CreateScannerAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ScannerAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ScannerComponentAsset")] public static void CreateScannerComponentAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ScannerComponentAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ScorchGlowAsset")] public static void CreateScorchGlowAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ScorchGlowAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ScriptableConditionAsset")] public static void CreateScriptableConditionAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ScriptableConditionAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/SelectedMarkerSettings")] public static void CreateSelectedMarkerSettings() { CreateMyAsset(typeof(BBI.Unity.Game.SelectedMarkerSettings)); }
[MenuItem("Assets/Create/Scriptable Objects/SessionSettingsAsset")] public static void CreateSessionSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.SessionSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ShaderGroupAsset")] public static void CreateShaderGroupAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ShaderGroupAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ShaderPropertyAsset")] public static void CreateShaderPropertyAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ShaderPropertyAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ShatterableComponentAsset")] public static void CreateShatterableComponentAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ShatterableComponentAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ShiftCycleCostAsset")] public static void CreateShiftCycleCostAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ShiftCycleCostAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ShipArchetypeAsset")] public static void CreateShipArchetypeAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ShipArchetypeAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ShipClassAsset")] public static void CreateShipClassAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ShipClassAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ShipRewardsAsset")] public static void CreateShipRewardsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ShipRewardsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/SolidColorMapperAsset")] public static void CreateSolidColorMapperAsset() { CreateMyAsset(typeof(BBI.Unity.Game.SolidColorMapperAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/SpaceTruckAsset")] public static void CreateSpaceTruckAsset() { CreateMyAsset(typeof(BBI.Unity.Game.SpaceTruckAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/SpaceTruckLevelAsset")] public static void CreateSpaceTruckLevelAsset() { CreateMyAsset(typeof(BBI.Unity.Game.SpaceTruckLevelAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/SpaceTruckSectionAsset")] public static void CreateSpaceTruckSectionAsset() { CreateMyAsset(typeof(BBI.Unity.Game.SpaceTruckSectionAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/SpaceTruckSettingsAsset")] public static void CreateSpaceTruckSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.SpaceTruckSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/SpaceTruckStatAsset")] public static void CreateSpaceTruckStatAsset() { CreateMyAsset(typeof(BBI.Unity.Game.SpaceTruckStatAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/SpeechSettingsAsset")] public static void CreateSpeechSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.SpeechSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/StatusEffectAsset")] public static void CreateStatusEffectAsset() { CreateMyAsset(typeof(BBI.Unity.Game.StatusEffectAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/StickerAsset")] public static void CreateStickerAsset() { CreateMyAsset(typeof(BBI.Unity.Game.StickerAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/StickerCollectionAsset")] public static void CreateStickerCollectionAsset() { CreateMyAsset(typeof(BBI.Unity.Game.StickerCollectionAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/StickerSettingsAsset")] public static void CreateStickerSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.StickerSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/StickerSizeAsset")] public static void CreateStickerSizeAsset() { CreateMyAsset(typeof(BBI.Unity.Game.StickerSizeAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/StickerSlotAsset")] public static void CreateStickerSlotAsset() { CreateMyAsset(typeof(BBI.Unity.Game.StickerSlotAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/StructurePartAsset")] public static void CreateStructurePartAsset() { CreateMyAsset(typeof(BBI.Unity.Game.StructurePartAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/SuitInformationAsset")] public static void CreateSuitInformationAsset() { CreateMyAsset(typeof(BBI.Unity.Game.SuitInformationAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/TalkingHeadCharacterAsset")] public static void CreateTalkingHeadCharacterAsset() { CreateMyAsset(typeof(BBI.Unity.Game.TalkingHeadCharacterAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/TalkingHeadsSettings")] public static void CreateTalkingHeadsSettings() { CreateMyAsset(typeof(BBI.Unity.Game.TalkingHeadsSettings)); }
[MenuItem("Assets/Create/Scriptable Objects/TallyObjectiveAsset")] public static void CreateTallyObjectiveAsset() { CreateMyAsset(typeof(BBI.Unity.Game.TallyObjectiveAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/TargetUvMapperAsset")] public static void CreateTargetUvMapperAsset() { CreateMyAsset(typeof(BBI.Unity.Game.TargetUvMapperAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/TethersAsset")] public static void CreateTethersAsset() { CreateMyAsset(typeof(BBI.Unity.Game.TethersAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ThrowAsset")] public static void CreateThrowAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ThrowAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ThrustersAsset")] public static void CreateThrustersAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ThrustersAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ToolDegradationSettingsAsset")] public static void CreateToolDegradationSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ToolDegradationSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ToolDurabilityAsset")] public static void CreateToolDurabilityAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ToolDurabilityAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/ToolTipAsset")] public static void CreateToolTipAsset() { CreateMyAsset(typeof(BBI.Unity.Game.ToolTipAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/TriggerableCollisionAsset")] public static void CreateTriggerableCollisionAsset() { CreateMyAsset(typeof(BBI.Unity.Game.TriggerableCollisionAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/TriggerableSpeechAsset")] public static void CreateTriggerableSpeechAsset() { CreateMyAsset(typeof(BBI.Unity.Game.TriggerableSpeechAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/TutorialSettingsAsset")] public static void CreateTutorialSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.TutorialSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/TypeAsset")] public static void CreateTypeAsset() { CreateMyAsset(typeof(BBI.Unity.Game.TypeAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/UIMarkerAsset")] public static void CreateUIMarkerAsset() { CreateMyAsset(typeof(BBI.Unity.Game.UIMarkerAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/UIMarkerObjectOfInterestAirPressureImbalanceAsset")] public static void CreateUIMarkerObjectOfInterestAirPressureImbalanceAsset() { CreateMyAsset(typeof(BBI.Unity.Game.UIMarkerObjectOfInterestAirPressureImbalanceAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/UnityLogSettingsAsset")] public static void CreateUnityLogSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.UnityLogSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/UpgradeListAsset")] public static void CreateUpgradeListAsset() { CreateMyAsset(typeof(BBI.Unity.Game.UpgradeListAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/UpgradeSettingsAsset")] public static void CreateUpgradeSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.UpgradeSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/UpgradeTreeAsset")] public static void CreateUpgradeTreeAsset() { CreateMyAsset(typeof(BBI.Unity.Game.UpgradeTreeAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/UvMapperBase")] public static void CreateUvMapperBase() { CreateMyAsset(typeof(BBI.Unity.Game.UvMapperBase)); }
[MenuItem("Assets/Create/Scriptable Objects/VaporizationAsset")] public static void CreateVaporizationAsset() { CreateMyAsset(typeof(BBI.Unity.Game.VaporizationAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/VaporizationSettingsAsset")] public static void CreateVaporizationSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.VaporizationSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/VaporizeOnGrappleChargePushAsset")] public static void CreateVaporizeOnGrappleChargePushAsset() { CreateMyAsset(typeof(BBI.Unity.Game.VaporizeOnGrappleChargePushAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/VaporizeOnIntegrityEmptyAsset")] public static void CreateVaporizeOnIntegrityEmptyAsset() { CreateMyAsset(typeof(BBI.Unity.Game.VaporizeOnIntegrityEmptyAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/VendingMachineItemAsset")] public static void CreateVendingMachineItemAsset() { CreateMyAsset(typeof(BBI.Unity.Game.VendingMachineItemAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/VitalityAsset")] public static void CreateVitalityAsset() { CreateMyAsset(typeof(BBI.Unity.Game.VitalityAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/VitalityControllerAsset")] public static void CreateVitalityControllerAsset() { CreateMyAsset(typeof(BBI.Unity.Game.VitalityControllerAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/VitalitySettingsAsset")] public static void CreateVitalitySettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.VitalitySettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/VitalityTypeAsset")] public static void CreateVitalityTypeAsset() { CreateMyAsset(typeof(BBI.Unity.VitalityTypeAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/WeeklyShipAsset")] public static void CreateWeeklyShipAsset() { CreateMyAsset(typeof(BBI.Unity.Game.WeeklyShipAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/WeeklyShipListAsset")] public static void CreateWeeklyShipListAsset() { CreateMyAsset(typeof(BBI.Unity.Game.WeeklyShipListAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/WeeklyShipObjectiveGroupListAsset")] public static void CreateWeeklyShipObjectiveGroupListAsset() { CreateMyAsset(typeof(BBI.Unity.Game.WeeklyShipObjectiveGroupListAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/WorkOrderSettingsAsset")] public static void CreateWorkOrderSettingsAsset() { CreateMyAsset(typeof(BBI.Unity.Game.WorkOrderSettingsAsset)); }
[MenuItem("Assets/Create/Scriptable Objects/WorldUvMapperAsset")] public static void CreateWorldUvMapperAsset() { CreateMyAsset(typeof(BBI.Unity.Game.WorldUvMapperAsset)); }

}
