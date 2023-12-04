using System;
using BepInEx;
using R2API.Utils;
using RoR2;

namespace ThunderRain {

	[BepInDependency("com.rune580.riskofoptions")]
	[NetworkCompatibility(CompatibilityLevel.NoNeedForSync)]
	[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
	public class ThunderRain : BaseUnityPlugin {

		public const string PluginGUID = PluginAuthor + "." + PluginName;
		public const string PluginAuthor = "quasikyo";
		public const string PluginName = "ThunderRain";
		public const string PluginVersion = "1.0.0";

		private static DeviceManager DeviceManager { get; set; }

		public void Awake() {
			Log.Init(Logger);
			Log.Info($"Performing setup for {nameof(ThunderRain)}");
			DeviceManager = new DeviceManager(nameof(ThunderRain));

			GlobalEventManager.onClientDamageNotified += OperateDevicesOnDamage;
        }

		private void OperateDevicesOnDamage(DamageDealtMessage damageMessage) {
			Log.Debug($"Victim: {damageMessage.victim?.ToString() ?? "reduced to atoms"}");
			Log.Debug($"Attacker: {damageMessage.attacker?.ToString() ?? "reduced to atoms"}");
			if (damageMessage.victim == null) { return; }

			CharacterMaster playerMaster = LocalUserManager.GetFirstLocalUser().cachedMaster;
			CharacterBody player = playerMaster.GetBody();
			CharacterBody victim = damageMessage.victim.GetComponent<CharacterBody>();
			CharacterBody attacker = damageMessage.attacker?.GetComponent<CharacterBody>();

			float victimMaxHealth = victim.healthComponent.fullCombinedHealth;
			float percentageOfMaxHealthDamaged = damageMessage.damage / victimMaxHealth;
			if (!ConfigManager.AllowExcessDamage.Value) {
				percentageOfMaxHealthDamaged = Math.Min(percentageOfMaxHealthDamaged, 1);
			}

			bool didPlayerDealDamage = player == attacker;
			bool didPlayerReceiveDamage = player == victim;
			bool didPlayerMinionDealDamage = attacker?.master.minionOwnership.ownerMaster == playerMaster;
			bool didPlayerMinionReceiveDamage = victim.master.minionOwnership.ownerMaster == playerMaster;

			Log.Debug($"{attacker} dealt {damageMessage.damage} ({percentageOfMaxHealthDamaged * 100}%) to {victim} max {victimMaxHealth}.");
			Log.Debug($"Was victim local player? {didPlayerReceiveDamage}");
			Log.Debug($"Was attacker local player? {didPlayerDealDamage}");

			PiShockValues values = new PiShockValues();
			if ((didPlayerDealDamage && ConfigManager.VibrationsFromDealingDamage.Value) || (didPlayerMinionDealDamage && ConfigManager.VibrationsFromMinionsDealingDamage.Value)) {
				values.Duration = TimeSpan.FromSeconds(ConfigManager.BaseVibrationDurationSeconds.Value * percentageOfMaxHealthDamaged);
				values.Intensity = (int) (ConfigManager.DealingDamageBaseVibrationIntensity.Value * percentageOfMaxHealthDamaged);
				DeviceManager.Operate(PiShockOperation.Vibrate, values);
			}

			if ((didPlayerDealDamage && ConfigManager.ShocksFromDealingDamage.Value) || (didPlayerMinionDealDamage && ConfigManager.ShocksFromMinionsDealingDamage.Value)) {
				values.Duration = TimeSpan.FromSeconds(ConfigManager.BaseShockDurationSeconds.Value * percentageOfMaxHealthDamaged);
				values.Intensity = (int) (ConfigManager.DealingDamageBaseShockIntensity.Value * percentageOfMaxHealthDamaged);
				DeviceManager.Operate(PiShockOperation.Shock, values);
			}

			if ((didPlayerReceiveDamage && ConfigManager.VibrationsFromReceivingDamage.Value) || (didPlayerMinionReceiveDamage && ConfigManager.VibrationsFromMinionsReceivingDamage.Value)) {
				values.Duration = TimeSpan.FromSeconds(ConfigManager.BaseVibrationDurationSeconds.Value * percentageOfMaxHealthDamaged);
				values.Intensity = (int) (ConfigManager.ReceivingDamageBaseVibrationIntensity.Value * percentageOfMaxHealthDamaged);
				DeviceManager.Operate(PiShockOperation.Vibrate, values);
			}

			if ((didPlayerReceiveDamage && ConfigManager.ShocksFromReceivingDamage.Value) || (didPlayerMinionReceiveDamage && ConfigManager.ShocksFromMinionsReceivingDamage.Value)) {
				values.Duration = TimeSpan.FromSeconds(ConfigManager.BaseShockDurationSeconds.Value * percentageOfMaxHealthDamaged);
				values.Intensity = (int) (ConfigManager.ReceivingDamageBaseShockIntensity.Value * percentageOfMaxHealthDamaged);
				DeviceManager.Operate(PiShockOperation.Shock, values);
			}
		}
	}
}
