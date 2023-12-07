using System;
using System.Collections;
using BepInEx;
using R2API.Utils;
using RoR2;
using UnityEngine;

namespace ThunderRain {

	[BepInDependency("com.rune580.riskofoptions")]
	[NetworkCompatibility(CompatibilityLevel.NoNeedForSync)]
	[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
	public class ThunderRain : BaseUnityPlugin {

		public const string PluginGUID = PluginAuthor + "." + PluginName;
		public const string PluginAuthor = "quasikyo";
		public const string PluginName = "ThunderRain";
		public const string PluginVersion = "1.0.1";

		private static ValuePool Buffer { get; set; }
		private static DeviceManager DeviceManager { get; set; }

		public void Awake() {
			Log.Init(Logger);
			Log.Info($"Performing setup for {nameof(ThunderRain)}");
			Buffer = new ValuePool();
			DeviceManager = new DeviceManager(nameof(ThunderRain));

			GlobalEventManager.onClientDamageNotified += OperateDevicesOnDamage;
		}

		private IEnumerator ReadBuffer() {
			Log.Debug($"Starting buffer");
			Buffer.SetActive();
			yield return new WaitForSeconds(ConfigManager.TimeSpanSeconds.Value);
			DeviceManager.ProcessValuePool(Buffer);
			Log.Debug($"Clearing buffer");
			Buffer.Reset();
		}

		private void OperateDevicesOnDamage(DamageDealtMessage damageMessage) {
			Log.Debug($"Victim: {damageMessage.victim?.ToString() ?? "reduced to atoms"}");
			Log.Debug($"Attacker: {damageMessage.attacker?.ToString() ?? "reduced to atoms"}");

			CharacterMaster playerMaster = LocalUserManager.GetFirstLocalUser().cachedMaster;
			CharacterBody player = playerMaster.GetBody();
			CharacterBody victim = damageMessage.victim?.GetComponent<CharacterBody>();
			CharacterBody attacker = damageMessage.attacker?.GetComponent<CharacterBody>();

			if (victim == null) { return; }

			if (Buffer.Status == ValuePool.PoolStatus.Empty) {
				StartCoroutine(ReadBuffer());
			}

			float victimMaxHealth = victim.healthComponent.fullCombinedHealth;
			float percentageOfMaxHealthDamaged = damageMessage.damage / victimMaxHealth;
			if (!ConfigManager.AllowExcessDamage.Value) {
				percentageOfMaxHealthDamaged = Math.Min(percentageOfMaxHealthDamaged, 1);
			}

			bool didPlayerDealDamage = player == attacker;
			bool didPlayerReceiveDamage = player == victim;
			bool didPlayerMinionDealDamage = attacker?.master?.minionOwnership.ownerMaster == playerMaster;
			bool didPlayerMinionReceiveDamage = victim.master?.minionOwnership.ownerMaster == playerMaster;

			Log.Debug($"{attacker} dealt {damageMessage.damage} ({percentageOfMaxHealthDamaged * 100}%) to {victim} max {victimMaxHealth}.");
			Log.Debug($"Was victim local player? {didPlayerReceiveDamage}");
			Log.Debug($"Was attacker local player? {didPlayerDealDamage}");

			if ((didPlayerDealDamage && ConfigManager.VibrationsFromDealingDamage.Value) || (didPlayerMinionDealDamage && ConfigManager.VibrationsFromMinionsDealingDamage.Value)) {
				Buffer.VibrationValues.Duration += TimeSpan.FromSeconds(ConfigManager.BaseVibrationDurationSeconds.Value * percentageOfMaxHealthDamaged);
				Buffer.VibrationValues.Intensity += ConfigManager.DealingDamageBaseVibrationIntensity.Value * percentageOfMaxHealthDamaged;
			}

			if ((didPlayerDealDamage && ConfigManager.ShocksFromDealingDamage.Value) || (didPlayerMinionDealDamage && ConfigManager.ShocksFromMinionsDealingDamage.Value)) {
				Buffer.ShockValues.Duration += TimeSpan.FromSeconds(ConfigManager.BaseShockDurationSeconds.Value * percentageOfMaxHealthDamaged);
				Buffer.ShockValues.Intensity += ConfigManager.DealingDamageBaseShockIntensity.Value * percentageOfMaxHealthDamaged;
			}

			if ((didPlayerReceiveDamage && ConfigManager.VibrationsFromReceivingDamage.Value) || (didPlayerMinionReceiveDamage && ConfigManager.VibrationsFromMinionsReceivingDamage.Value)) {
				Buffer.VibrationValues.Duration += TimeSpan.FromSeconds(ConfigManager.BaseVibrationDurationSeconds.Value * percentageOfMaxHealthDamaged);
				Buffer.VibrationValues.Intensity += ConfigManager.ReceivingDamageBaseVibrationIntensity.Value * percentageOfMaxHealthDamaged;
			}

			if ((didPlayerReceiveDamage && ConfigManager.ShocksFromReceivingDamage.Value) || (didPlayerMinionReceiveDamage && ConfigManager.ShocksFromMinionsReceivingDamage.Value)) {
				Buffer.ShockValues.Duration += TimeSpan.FromSeconds(ConfigManager.BaseShockDurationSeconds.Value * percentageOfMaxHealthDamaged);
				Buffer.ShockValues.Intensity += ConfigManager.ReceivingDamageBaseShockIntensity.Value * percentageOfMaxHealthDamaged;
			}
		}
	}
}
