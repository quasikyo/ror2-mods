using System;

using UnityEngine;

using BepInEx;
using RoR2;
using R2API.Utils;

using static RumbleRain.VibrationInfoProvider;

namespace RumbleRain {
	[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
	[BepInDependency("com.rune580.riskofoptions")]
	[NetworkCompatibility(CompatibilityLevel.NoNeedForSync)]
	public class RumbleRain : BaseUnityPlugin {

		public const string PluginGUID = PluginAuthor + "." + PluginName;
		public const string PluginAuthor = "quasikyo";
		public const string PluginName = "RumbleRain";
		public const string PluginVersion = "0.2.2";

		internal static DeviceManager DeviceManager { get; private set; }

		public void Awake() {
			Logger.LogInfo("Performing setup for RumbleRain");

			VibrationInfoProvider vibrationInfoProvider = From(ConfigManager.VibrationBehavior.Value);
			DeviceManager = new DeviceManager(vibrationInfoProvider, Logger);
			DeviceManager.ConnectDevices();

			Run.onRunStartGlobal += (Run _) => { StartCoroutine(DeviceManager.PollVibrations()); };
			Run.onRunDestroyGlobal += (Run _) => {
				StopAllCoroutines();
				DeviceManager.CleanUp();
			};
			GlobalEventManager.onClientDamageNotified += VibrateDevicesOnDamage;
		}

		public void Update() {
			if (ConfigManager.ToggleVibrationKeybind.Value.IsDown()) {
				DeviceManager.ToggleConnectedDevices();
			}
		}

		private void VibrateDevicesOnDamage(DamageDealtMessage damageMessage) {
			if (damageMessage.victim == null) { return; }

			PlayerCharacterMasterController playerController = LocalUserManager.GetFirstLocalUser().cachedMasterController;
			GameObject playerBody = playerController.master.GetBodyObject();
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
