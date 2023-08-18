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

			ConfigManager.SetupConfig();

			VibrationInfoProvider vibrationInfoProvider = VibrationInfoProvider.From(ConfigManager.VibrationBehavior.Value);
			DeviceManager = new DeviceManager(vibrationInfoProvider, Logger);
			DeviceManager.ConnectDevices();
			StartCoroutine(DeviceManager.PollVibrations());

			GlobalEventManager.onClientDamageNotified += VibrateDevicesOnDamage;
		}

		private void VibrateDevicesOnDamage(DamageDealtMessage damageMessage) {
			Logger.LogDebug($"Victim: {damageMessage.victim} - Attacker: {damageMessage.attacker}");
			if (damageMessage.attacker == null || damageMessage.victim == null) { return; }

			LocalPlayerController = LocalUserManager.GetFirstLocalUser().cachedMasterController;
			GameObject playerBody = LocalPlayerController.master.GetBodyObject();
			bool didPlayerReceiveDamage = playerBody == damageMessage.victim;
			bool didPlayerDealDamage = playerBody == damageMessage.attacker;

			float percentageOfHealthDamaged = damageMessage.damage / damageMessage.victim.GetComponent<HealthComponent>().fullCombinedHealth;
			if (!ConfigManager.AccountForExcessDamage.Value) percentageOfHealthDamaged = Math.Min(percentageOfHealthDamaged, 1);
			VibrationInfo vibrationInfo = new VibrationInfo(TimeSpan.FromSeconds(ConfigManager.BaseVibrationDurationSeconds.Value * percentageOfHealthDamaged));
			
			if (didPlayerDealDamage && ConfigManager.IsRewardingEnabled.Value) {
				vibrationInfo.Intensity = ConfigManager.VibrationIntensityMultiplierOnDamageDealt.Value * percentageOfHealthDamaged;
				DeviceManager.SendVibrationInfo(vibrationInfo);
			}
			if (didPlayerReceiveDamage && ConfigManager.IsPunishingEnabled.Value) {
				vibrationInfo.Intensity = ConfigManager.VibrationIntensityMultiplierOnDamageReceived.Value * percentageOfHealthDamaged;
				DeviceManager.SendVibrationInfo(vibrationInfo);
			}
		}
	}
}
