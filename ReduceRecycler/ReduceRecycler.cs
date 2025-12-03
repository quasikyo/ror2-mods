using System;
using System.Collections;

using UnityEngine;

using BepInEx;
using MonoMod.Cil;
using RoR2;
using R2API.Utils;

using static On.RoR2.EquipmentSlot;

namespace ReduceRecycler {
	[BepInDependency("com.rune580.riskofoptions")]
	[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
	[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
	public class ReduceRecycler : BaseUnityPlugin {

		private const string finalStageSceneName = "moon2";
		private const string voidFieldsStageSceneName = "arena";

		/// <summary>
		///  When to reset the cooldown.
		/// </summary>
		internal enum CooldownReset {
			/// <summary>
			/// Resets after use.
			/// </summary>
			AfterUse,
			/// <summary>
			/// Resets on button press.
			/// </summary>
			OnDemand
		}

		public void Awake() {
			Log.Init(Logger);
			Log.Info($"Performing setup for {nameof(ReduceRecycler)}");
			ConfigManager.Init();

			FireRecycle += ResetAfterUse;
			// IL hooks seem to be a one-way street; Got errors when I tried removing them
			IL.RoR2.EquipmentSlot.MyFixedUpdate += EquipmentActivationAllowedIfRecycler;
			IL.RoR2.EquipmentSlot.ExecuteIfReady += ResetOnDemand;
		}

		/// <summary>
		/// Modify call to CharacterBody::isEquipmentActivationAllowed
		/// to always return true if current equipment is Recycler.
		/// Otherwise, use existing logic.
		/// </summary>
		private void EquipmentActivationAllowedIfRecycler(ILContext context)  {
			ILCursor cursor = new ILCursor(context);
			cursor.GotoNext(
				x => x.MatchLdarg(0),
				x => x.MatchCallOrCallvirt<EquipmentSlot>("get_characterBody"),
				x => x.MatchCallOrCallvirt<CharacterBody>("get_isEquipmentActivationAllowed")
			);
			cursor.Index += 2;
			cursor.Remove();
			cursor.EmitDelegate<Func<CharacterBody, bool>>((CharacterBody body) => {
				VehicleSeat vehicle = body.currentVehicle;
				bool originalValue = !vehicle || vehicle.isEquipmentActivationAllowed;
				return IsRecycler(body.equipmentSlot) || originalValue;
			});
		}

		/// <summary>
		/// Reset cooldown on button press by hijacking check to EquipmentSlot::stock.
		/// </summary>
		private void ResetOnDemand(ILContext context) {
			ILCursor cursor = new ILCursor(context);
			cursor.GotoNext(x => x.MatchCallOrCallvirt<EquipmentSlot>("get_stock"));
			cursor.Remove();
			cursor.EmitDelegate<Func<EquipmentSlot, int>>((EquipmentSlot slot) => {
				if (ConfigManager.CooldownReset.Value == CooldownReset.OnDemand) {
					Log.Debug("Resetting before use");
					ResetRecyclerCooldown(slot);
				}
				return slot.stock;
			});
		}

		/// <summary>
		/// Resets equipment cooldown after rerolling an item with the Recycler.
		/// </summary>
		/// <returns>It the Recycler rerolled an item.</returns>
		private bool ResetAfterUse(orig_FireRecycle original, EquipmentSlot slot) {
			bool didRecyclerReroll = original(slot);
			Log.Debug($"didRecyclerReroll: {didRecyclerReroll}");
			if (ConfigManager.CooldownReset.Value == CooldownReset.AfterUse && didRecyclerReroll) {
				// Game uses a subcooldown timer before updating certain game state
				Log.Debug("Resetting after use");
				StartCoroutine(
					ExecuteWithDelay(
						() => ResetRecyclerCooldown(slot),
						TimeSpan.FromSeconds(0.5)
					)
				);
			}
			return didRecyclerReroll;
		}

		/// <summary>
		/// Resets cooldown of <paramref name="slot"/> and adds the time to the run timer.
		/// The critieria are:
		/// - Out of charges
		/// - On cooldown
		/// - Is teleporter finished (applies only if corresponding config option is enabled)
		/// </summary>
		/// <param name="slot"><c>EquipmentSlot</c> of the Recycler.</param>
		private void ResetRecyclerCooldown(EquipmentSlot slot) {
			if (!ConfigManager.IsModEnabled.Value) {
				Log.Debug("Skipping reset as mod is disabled");
				return;
			}

			if (!IsRecycler(slot)) {
				Log.Debug("Skipping reset as equipment is not the Recycler");
				return;
			}

			float cooldownSeconds = slot.cooldownTimer;
			bool isInfinity = float.IsInfinity(cooldownSeconds);
			if (isInfinity) {
				Log.Debug("Skipping reset as cooldownTimer is infinity");
				return;
			}
			float stopwatchCurrentSeconds = Run.instance.GetRunStopwatch();

			bool isTeleporterPresent = StageHasTeleporter(Stage.instance.sceneDef);
			bool isTeleporterFinished = isTeleporterPresent && TeleporterInteraction.instance.isCharged;
			bool isTeleporterRelevant = ConfigManager.EnableOnlyAfterTeleporter.Value;
			// discrete math, relevancy implies a need for a charged TP
			bool allowCooldown = !isTeleporterRelevant || isTeleporterFinished;

			bool isOutOfCharges = GetEquipmentCharges(slot) == 0;
			bool doRemoveCooldown = isOutOfCharges && allowCooldown;

			if (!doRemoveCooldown) {
				Log.Debug("Skipping reset due to failing criteria evaluation");
				return;
			}

			slot.characterBody.inventory.DeductActiveEquipmentCooldown(cooldownSeconds);
			if (!Run.instance.isRunStopwatchPaused) {
				Run.instance.SetRunStopwatch(stopwatchCurrentSeconds + cooldownSeconds);
			}
		}

		#region Helper Functions
		// https://risk-of-thunder.github.io/R2Wiki/Mod-Creation/Developer-Reference/Scene-Names
		private bool StageHasTeleporter(SceneDef scene) {
			// TODO: don't determine this by name but by whether a TP has been placed on the stage
			bool isStage = scene.sceneType == SceneType.Stage;
			bool isCommencement = scene.baseSceneName.Equals(finalStageSceneName);
			bool isVoidFields = scene.baseSceneName.Equals(voidFieldsStageSceneName);
			return isStage && !isCommencement && !isVoidFields;
		}

		private bool IsRecycler(EquipmentSlot slot) {
			EquipmentIndex index = slot?.equipmentIndex ?? EquipmentIndex.None;
			EquipmentDef equipment = EquipmentCatalog.GetEquipmentDef(index);
			return equipment == RoR2Content.Equipment.Recycle;
		}

		private int GetEquipmentCharges(EquipmentSlot equipmentSlot) {
			Inventory inventory = equipmentSlot.characterBody.inventory;
			uint slot = inventory.activeEquipmentSlot;
			EquipmentState state = inventory.GetEquipment(slot);
			return state.charges;
		}

		private IEnumerator ExecuteWithDelay(Action action, TimeSpan delay) {
			yield return new WaitForSeconds((float) delay.TotalSeconds);
			action();
		}
		#endregion
	}
}
