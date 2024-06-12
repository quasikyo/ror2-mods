using System.Collections;

using UnityEngine;

using BepInEx;
using RoR2;
using R2API.Utils;

namespace ReduceRecycler {
	[BepInDependency("com.rune580.riskofoptions")]
	[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
	[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
	public class ReduceRecycler : BaseUnityPlugin {

		public const string PluginGUID = PluginAuthor + "." + PluginName;
		public const string PluginAuthor = "quasikyo";
		public const string PluginName = "ReduceRecycler";
		public const string PluginVersion = "1.2.0";

		private const string finalStageSceneName = "moon2";
		private const string voidFieldsStageSceneName = "arena";

		public void Awake() {
			Log.Init(Logger);
			Log.Info($"Performing setup for {nameof(ReduceRecycler)}");
			ConfigManager.Init();

			On.RoR2.EquipmentSlot.FireRecycle += (On.RoR2.EquipmentSlot.orig_FireRecycle orig, EquipmentSlot self) => {
				bool didRecyclerTransmute = orig(self);
				if (didRecyclerTransmute) {
					// Game state doesn't update quick enough OR until after this hook is done running (weird)
					StartCoroutine(ResetRecyclerCooldown(self));
				}
				return didRecyclerTransmute;
			};
		}

		private IEnumerator ResetRecyclerCooldown(EquipmentSlot equipment) {
			yield return new WaitForSeconds(0.5f);
			float cooldownSeconds = equipment.cooldownTimer;
			float stopwatchCurrentSeconds = Run.instance.GetRunStopwatch();

			bool isInfinity = float.IsInfinity(cooldownSeconds);
			if (isInfinity) yield break;

			bool isTeleporterPresent = StageHasTeleporter(Stage.instance.sceneDef);
			bool isTeleporterFinished = isTeleporterPresent && TeleporterInteraction.instance.isCharged;
			bool isTeleporterRelevant = ConfigManager.EnableOnlyAfterTeleporter.Value;
			// discrete math, relevancy implies a need for a charged TP
			bool allowCooldown = !isTeleporterRelevant || isTeleporterFinished;

			bool isOutOfCharges = GetEquipmentCharges(equipment) == 0;
			bool doRemoveCooldown = isOutOfCharges && allowCooldown;

			if (doRemoveCooldown) {
				equipment.characterBody.inventory.DeductActiveEquipmentCooldown(cooldownSeconds);
				if (!Run.instance.isRunStopwatchPaused) {
					Run.instance.SetRunStopwatch(stopwatchCurrentSeconds + cooldownSeconds);
				}
			}
		}

		// https://risk-of-thunder.github.io/R2Wiki/Mod-Creation/Developer-Reference/Scene-Names
		private bool StageHasTeleporter(SceneDef scene) {
			// TODO: don't determine this by name but by whether a TP has been placed on the stage
			bool isStage = scene.sceneType == SceneType.Stage;
			bool isCommencement = scene.baseSceneName.Equals(finalStageSceneName);
			bool isVoidFields = scene.baseSceneName.Equals(voidFieldsStageSceneName);
			return isStage && !isCommencement && !isVoidFields;
		}

		private int GetEquipmentCharges(EquipmentSlot equipmentSlot) {
			Inventory inventory = equipmentSlot.characterBody.inventory;
			uint slot = inventory.activeEquipmentSlot;
			EquipmentState state = inventory.GetEquipment(slot);
			return state.charges;
		}
	}
}
