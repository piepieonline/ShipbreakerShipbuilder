using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine.AddressableAssets;
using BBI.Unity.Game;

public class MyPreview : ObjectPreview
{
    // string folderPath;

    public override bool HasPreviewGUI()
    {
        return true;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        if(GUILayout.Button($"Copy {target.name} GUID")) {
            if(AssetDatabase.TryGetGUIDAndLocalFileIdentifier(target, out var guid, out long _))
                GUIUtility.systemCopyBuffer = guid;
        }

        if(GUILayout.Button($"Save {target.name}")) {
            //add everything the button would do.
            var newobj = ScriptableObject.CreateInstance(target.GetType());
            EditorUtility.CopySerialized(target, newobj);

            // folderPath = folderPath == null || folderPath == "" ? "Assets" : folderPath;

            MethodInfo getActiveFolderPath = typeof(ProjectWindowUtil).GetMethod(
                "GetActiveFolderPath",
                BindingFlags.Static | BindingFlags.NonPublic);
            
            string folderPath = (string) getActiveFolderPath.Invoke(null, null);


            Debug.Log(folderPath);
            AssetDatabase.CreateAsset(newobj, $"{folderPath}/{target.name}.asset");
        }
    }
}

[CustomPreview(typeof(BBI.Unity.VitalityTypeAsset))] public class PreviewVitalityTypeAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.AchievementAsset))] public class PreviewAchievementAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.IndustrialActionShipAsset))] public class PreviewIndustrialActionShipAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ObjectiveGroupEntry))] public class PreviewObjectiveGroupEntry : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.CategoryAsset))] public class PreviewCategoryAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.DebtInterestAsset))] public class PreviewDebtInterestAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.DebtMilestoneAsset))] public class PreviewDebtMilestoneAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.DifficultyModeAsset))] public class PreviewDifficultyModeAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.CompanyInfoAsset))] public class PreviewCompanyInfoAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.CompanyShipNameAsset))] public class PreviewCompanyShipNameAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.NarrativeMessageAsset))] public class PreviewNarrativeMessageAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.NarrativeMessageRewardAsset))] public class PreviewNarrativeMessageRewardAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.PlayerActionTrackerAsset))] public class PreviewPlayerActionTrackerAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.NarrativeCategoryAsset))] public class PreviewNarrativeCategoryAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.NarrativeContentAsset))] public class PreviewNarrativeContentAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.NarrativeEntryAsset))] public class PreviewNarrativeEntryAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ShiftCycleCostAsset))] public class PreviewShiftCycleCostAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.RentalCostsAsset))] public class PreviewRentalCostsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ShipClassAsset))] public class PreviewShipClassAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.BuffableAttributeAsset))] public class PreviewBuffableAttributeAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.CurveAsset))] public class PreviewCurveAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.EntityComponentDataAssetBase))] public class PreviewEntityComponentDataAssetBase : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.DamageTypeAsset))] public class PreviewDamageTypeAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.PropertyContainerAsset))] public class PreviewPropertyContainerAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ModuleConstructionAsset))] public class PreviewModuleConstructionAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ObjectInfoAsset))] public class PreviewObjectInfoAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ModulePropertyAsset))] public class PreviewModulePropertyAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ObjectiveEntry))] public class PreviewObjectiveEntry : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.RoomTypeAsset))] public class PreviewRoomTypeAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.CurrencyAsset))] public class PreviewCurrencyAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.TypeAsset))] public class PreviewTypeAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.SpaceTruckAsset))] public class PreviewSpaceTruckAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.SpaceTruckLevelAsset))] public class PreviewSpaceTruckLevelAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.SpaceTruckSectionAsset))] public class PreviewSpaceTruckSectionAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.SpaceTruckStatAsset))] public class PreviewSpaceTruckStatAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.NarrativeRandomizableSceneAsset))] public class PreviewNarrativeRandomizableSceneAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.NPCSalvageTriggerComponent))] public class PreviewNPCSalvageTriggerComponent : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.PATConditionAsset))] public class PreviewPATConditionAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.HabAfterShiftAsset))] public class PreviewHabAfterShiftAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.PortraitDistortionSettings))] public class PreviewPortraitDistortionSettings : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.PortraitDistortionCameraShakeSettings))] public class PreviewPortraitDistortionCameraShakeSettings : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.DemoChargeControllerAsset))] public class PreviewDemoChargeControllerAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.CutGradeColorData))] public class PreviewCutGradeColorData : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.CuttingToolAsset))] public class PreviewCuttingToolAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ScalpelAsset))] public class PreviewScalpelAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ControlSpriteAsset))] public class PreviewControlSpriteAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ControlSpriteListAsset))] public class PreviewControlSpriteListAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.CrushingSettingsAsset))] public class PreviewCrushingSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.JointPairingAsset))] public class PreviewJointPairingAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.JointSetupInfoAsset))] public class PreviewJointSetupInfoAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.JointabilityAsset))] public class PreviewJointabilityAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ConditionThresholdAsset))] public class PreviewConditionThresholdAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.LevelListAsset))] public class PreviewLevelListAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.NarrativeGeneratedAssetListAsset))] public class PreviewNarrativeGeneratedAssetListAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.RewardTiersAsset))] public class PreviewRewardTiersAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.RewardTypeWeightsAsset))] public class PreviewRewardTypeWeightsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.RewardWeightsAsset))] public class PreviewRewardWeightsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ShipRewardsAsset))] public class PreviewShipRewardsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.TalkingHeadCharacterAsset))] public class PreviewTalkingHeadCharacterAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.TalkingHeadsSettings))] public class PreviewTalkingHeadsSettings : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.WeeklyShipAsset))] public class PreviewWeeklyShipAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.WeeklyShipObjectiveGroupListAsset))] public class PreviewWeeklyShipObjectiveGroupListAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.WeeklyShipListAsset))] public class PreviewWeeklyShipListAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ShipArchetypeAsset))] public class PreviewShipArchetypeAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.AssetSaveKeyConfigAsset))] public class PreviewAssetSaveKeyConfigAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ToolTipAsset))] public class PreviewToolTipAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.BreakableJointComponentAsset))] public class PreviewBreakableJointComponentAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.CutExecutionAsset))] public class PreviewCutExecutionAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.CutLineAsset))] public class PreviewCutLineAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.CutterAsset))] public class PreviewCutterAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.EntityBlueprintAsset))] public class PreviewEntityBlueprintAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ImpactDamageRangeAsset))] public class PreviewImpactDamageRangeAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.InventoryTypeAsset))] public class PreviewInventoryTypeAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ItemQualityTypeAsset))] public class PreviewItemQualityTypeAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.InventoryItemAsset))] public class PreviewInventoryItemAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.LynxEditorToolbarAsset))] public class PreviewLynxEditorToolbarAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.InteractableObjectAsset))] public class PreviewInteractableObjectAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.InteractionTypeAsset))] public class PreviewInteractionTypeAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ScriptableConditionAsset))] public class PreviewScriptableConditionAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ShaderGroupAsset))] public class PreviewShaderGroupAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.TriggerableSpeechAsset))] public class PreviewTriggerableSpeechAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.AudioCollisionAsset))] public class PreviewAudioCollisionAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.AudioMaterialAsset))] public class PreviewAudioMaterialAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.FXMaterialAsset))] public class PreviewFXMaterialAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.FXCollisionAsset))] public class PreviewFXCollisionAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.DamageAsset))] public class PreviewDamageAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.PlayerFXAsset))] public class PreviewPlayerFXAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ShaderPropertyAsset))] public class PreviewShaderPropertyAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ShatterableComponentAsset))] public class PreviewShatterableComponentAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.IRigidbodyAsset))] public class PreviewIRigidbodyAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.VitalityControllerAsset))] public class PreviewVitalityControllerAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.VitalityAsset))] public class PreviewVitalityAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.HardPointTypeAsset))] public class PreviewHardPointTypeAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ModuleVitalityListAsset))] public class PreviewModuleVitalityListAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ModuleListAsset))] public class PreviewModuleListAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.JointSetupAsset))] public class PreviewJointSetupAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ForceOverTimeAsset))] public class PreviewForceOverTimeAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.GoalObjectiveAsset))] public class PreviewGoalObjectiveAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.GrabAsset))] public class PreviewGrabAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.GrapplingHookAsset))] public class PreviewGrapplingHookAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.GrapplingRopeAsset))] public class PreviewGrapplingRopeAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.HighlightComponentAsset))] public class PreviewHighlightComponentAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.PoolObjectiveAsset))] public class PreviewPoolObjectiveAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.PushOffAsset))] public class PreviewPushOffAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.AudioDebugInfoAsset))] public class PreviewAudioDebugInfoAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ScannerAsset))] public class PreviewScannerAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ScannerComponentAsset))] public class PreviewScannerComponentAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ScanModeAsset))] public class PreviewScanModeAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.TallyObjectiveAsset))] public class PreviewTallyObjectiveAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.TethersAsset))] public class PreviewTethersAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ThrowAsset))] public class PreviewThrowAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ThrustersAsset))] public class PreviewThrustersAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.TriggerableCollisionAsset))] public class PreviewTriggerableCollisionAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.AudioProximityAsset))] public class PreviewAudioProximityAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.VaporizationAsset))] public class PreviewVaporizationAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.VendingMachineItemAsset))] public class PreviewVendingMachineItemAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ModuleSkinListAsset))] public class PreviewModuleSkinListAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ModuleSkinAsset))] public class PreviewModuleSkinAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ModuleTypeAsset))] public class PreviewModuleTypeAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.StructurePartAsset))] public class PreviewStructurePartAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.DynamicLightColorAsset))] public class PreviewDynamicLightColorAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.DynamicLightFlickerAsset))] public class PreviewDynamicLightFlickerAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.GeneratedNoiseAsset))] public class PreviewGeneratedNoiseAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.DynamicRoomContainerAsset))] public class PreviewDynamicRoomContainerAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.UpgradeListAsset))] public class PreviewUpgradeListAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.UpgradeTreeAsset))] public class PreviewUpgradeTreeAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.StickerSlotAsset))] public class PreviewStickerSlotAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.StickerAsset))] public class PreviewStickerAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.StickerCollectionAsset))] public class PreviewStickerCollectionAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.StickerSizeAsset))] public class PreviewStickerSizeAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.GrappleLineSettingsAsset))] public class PreviewGrappleLineSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.GrappleVFXInstanceAsset))] public class PreviewGrappleVFXInstanceAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.SelectedMarkerSettings))] public class PreviewSelectedMarkerSettings : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.DynamicObjectives))] public class PreviewDynamicObjectives : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.RoomDataAsset))] public class PreviewRoomDataAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ColorMapperBase))] public class PreviewColorMapperBase : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.UvMapperBase))] public class PreviewUvMapperBase : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.PreloadListAsset))] public class PreviewPreloadListAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.LevelAsset))] public class PreviewLevelAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.PolymorphicAsset))] public class PreviewPolymorphicAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.StatusEffectAsset))] public class PreviewStatusEffectAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.Data.CuttingTargetableAsset))] public class CuttingTargetableAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.MainSettingsAsset))] public class PreviewMainSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.AudioClusterSettingsAsset))] public class PreviewAudioClusterSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.AudioDebugSettingsAsset))] public class PreviewAudioDebugSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.AudioMotionTrackerSettingsAsset))] public class PreviewAudioMotionTrackerSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.AudioServiceSettingsAsset))] public class PreviewAudioServiceSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.BackButtonSettingsAsset))] public class PreviewBackButtonSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.BargeSettingsAsset))] public class PreviewBargeSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.BreakableSettingsAsset))] public class PreviewBreakableSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.CareerLevelSettingsAsset))] public class PreviewCareerLevelSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.CertificationSettingsAsset))] public class PreviewCertificationSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.CertificationLevelSettingsAsset))] public class PreviewCertificationLevelSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ControlRebindSettingsAsset))] public class PreviewControlRebindSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ControlSriteSettingsAsset))] public class PreviewControlSriteSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.DamageDebugSettingsAsset))] public class PreviewDamageDebugSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.DifficultyModeSettingsAsset))] public class PreviewDifficultyModeSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.DissolveSettingsAsset))] public class PreviewDissolveSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ElementalSettingsAsset))] public class PreviewElementalSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.HabSettingsAsset))] public class PreviewHabSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.HauntingSettingsAssset))] public class PreviewHauntingSettingsAssset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.HierarchySettingsAsset))] public class PreviewHierarchySettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.LanguageSettingsAsset))] public class PreviewLanguageSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.MachineEmissiveSettingsAsset))] public class PreviewMachineEmissiveSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.NarrativeSettingsAsset))] public class PreviewNarrativeSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.OutlineSettingsAsset))] public class PreviewOutlineSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.PlayerSettingsAsset))] public class PreviewPlayerSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.RaycastSettingsAsset))] public class PreviewRaycastSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.RoomSettingsAsset))] public class PreviewRoomSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.SalvageSettingsAsset))] public class PreviewSalvageSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.SaveLoadSettingsAsset))] public class PreviewSaveLoadSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.SessionSettingsAsset))] public class PreviewSessionSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.SpaceTruckSettingsAsset))] public class PreviewSpaceTruckSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.SpeechSettingsAsset))] public class PreviewSpeechSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.StickerSettingsAsset))] public class PreviewStickerSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ToolDegradationSettingsAsset))] public class PreviewToolDegradationSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.TutorialSettingsAsset))] public class PreviewTutorialSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.UnityLogSettingsAsset))] public class PreviewUnityLogSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.UpgradeSettingsAsset))] public class PreviewUpgradeSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.EquipmentInfoSettingsAsset))] public class PreviewEquipmentInfoSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.VaporizationSettingsAsset))] public class PreviewVaporizationSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.VitalitySettingsAsset))] public class PreviewVitalitySettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.WorkOrderSettingsAsset))] public class PreviewWorkOrderSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.AlignedUvMapperAsset))] public class PreviewAlignedUvMapperAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.TargetUvMapperAsset))] public class PreviewTargetUvMapperAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.WorldUvMapperAsset))] public class PreviewWorldUvMapperAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.SolidColorMapperAsset))] public class PreviewSolidColorMapperAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.DemoChargeAsset))] public class PreviewDemoChargeAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ToolDurabilityAsset))] public class PreviewToolDurabilityAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.BreachForceSettingsAsset))] public class PreviewBreachForceSettingsAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.CameraAsset))] public class PreviewCameraAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.DispositionAsset))] public class PreviewDispositionAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.DustViewAsset))] public class PreviewDustViewAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ElectricalHauntingComponentAsset))] public class PreviewElectricalHauntingComponentAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.FogViewAsset))] public class PreviewFogViewAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.HauntingCauserComponentAsset))] public class PreviewHauntingCauserComponentAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.HauntingThresholdComponentAsset))] public class PreviewHauntingThresholdComponentAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.PressurizationHauntingComponentAsset))] public class PreviewPressurizationHauntingComponentAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.RoomInfoMarkerAsset))] public class PreviewRoomInfoMarkerAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.UIMarkerAsset))] public class PreviewUIMarkerAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.UIMarkerObjectOfInterestAirPressureImbalanceAsset))] public class PreviewUIMarkerObjectOfInterestAirPressureImbalanceAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.VaporizeOnGrappleChargePushAsset))] public class PreviewVaporizeOnGrappleChargePushAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.VaporizeOnIntegrityEmptyAsset))] public class PreviewVaporizeOnIntegrityEmptyAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ElementalAssetBase))] public class PreviewElementalAssetBase : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.DoorGateAsset))] public class PreviewDoorGateAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.LODGroupDataAsset))] public class PreviewLODGroupDataAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.SalvageBlockerBargeAsset))] public class PreviewSalvageBlockerBargeAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.SalvageBlockerProcessorAsset))] public class PreviewSalvageBlockerProcessorAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.AirPressureAsset))] public class PreviewAirPressureAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.AirPressureOxygenMutatorAsset))] public class PreviewAirPressureOxygenMutatorAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.AirPressureStateAsset))] public class PreviewAirPressureStateAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.AsphyxiationMutatorAsset))] public class PreviewAsphyxiationMutatorAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.PlayerEntityAsset))] public class PreviewPlayerEntityAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.RoomTrackerAsset))] public class PreviewRoomTrackerAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.SuitInformationAsset))] public class PreviewSuitInformationAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.DoorAsset))] public class PreviewDoorAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.MachinePartAsset))] public class PreviewMachinePartAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.PanelSealingAsset))] public class PreviewPanelSealingAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ScorchGlowAsset))] public class PreviewScorchGlowAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.SalvageableComponentAsset))] public class PreviewSalvageableComponentAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.Data.Assets.BreakableJoints.MandatoryConnectionAsset))] public class PreviewMandatoryConnectionAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ModuleEntryContainer))] public class PreviewModuleEntryContainerAsset : MyPreview {}
[CustomPreview(typeof(BBI.Unity.Game.ModuleEntryDefinition))] public class PreviewModuleEntryDefinitionAsset : MyPreview {}