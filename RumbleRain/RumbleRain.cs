using System;

using UnityEngine;

using BepInEx;
using RoR2;
using R2API.Utils;

namespace RumbleRain {
	[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
	[NetworkCompatibility(CompatibilityLevel.NoNeedForSync)]
	public class RumbleRain : BaseUnityPlugin {

		public const string PluginGUID = PluginAuthor + "." + PluginName;
		public const string PluginAuthor = "quasikyo";
		public const string PluginName = "RumbleRain";
		public const string PluginVersion = "1.0.0";

		private PlayerCharacterMasterController LocalPlayerController { get; set; }
		private DeviceManager DeviceManager { get; set; }

		public void Awake() {
			Logger.LogInfo("Performing setup for RumbleRain");

			VibrationInfoProvider vibrationInfoProvider = VibrationInfoProvider.From(ConfigManager.VibrationBehavior.Value);
			DeviceManager = new DeviceManager(vibrationInfoProvider, Logger);
			DeviceManager.ConnectDevices();

			Run.onRunStartGlobal += (Run _) => { StartCoroutine(DeviceManager.PollVibrations()); };
			Run.onRunDestroyGlobal += (Run _) => {
				StopAllCoroutines();
				DeviceManager.CleanUp();
			};
			GlobalEventManager.onClientDamageNotified += VibrateDevicesOnDamage;
		}

		private void VibrateDevicesOnDamage(DamageDealtMessage damageMessage) {
			Logger.LogDebug($"Victim: {damageMessage.victim} | Attacker: {damageMessage.attacker}");
			if (damageMessage.victim == null) { return; }

			LocalPlayerController = LocalUserManager.GetFirstLocalUser().cachedMasterController;
			GameObject playerBody = LocalPlayerController.master.GetBodyObject();
			bool didPlayerReceiveDamage = playerBody == damageMessage.victim;
			bool didPlayerDealDamage = playerBody == damageMessage.attacker;

			float victimMaxHealth = damageMessage.victim.GetComponent<HealthComponent>().fullCombinedHealth;
			float percentageOfHealthDamaged = damageMessage.damage / victimMaxHealth;
			if (!ConfigManager.AllowExcessDamage.Value) {
				percentageOfHealthDamaged = Math.Min(percentageOfHealthDamaged, 1);
			}

			double baseSeconds = ConfigManager.BaseVibrationDurationSeconds.Value;
			VibrationInfo vibrationInfo = new VibrationInfo(TimeSpan.FromSeconds(baseSeconds * percentageOfHealthDamaged));

			if (didPlayerDealDamage && ConfigManager.IsRewardingEnabled.Value) {
				vibrationInfo.Intensity = ConfigManager.DamageDealtBaseVibrationIntensity.Value * percentageOfHealthDamaged;
				DeviceManager.SendVibrationInfo(vibrationInfo);
			}
			if (didPlayerReceiveDamage && ConfigManager.IsPunishingEnabled.Value) {
				vibrationInfo.Intensity = ConfigManager.DamageReceivedBaseVibrationIntensity.Value * percentageOfHealthDamaged;
				DeviceManager.SendVibrationInfo(vibrationInfo);
			}
		}
	}
}
