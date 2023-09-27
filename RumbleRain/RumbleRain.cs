using System;

using BepInEx;
using RoR2;
using R2API.Utils;

using static RumbleRain.VibrationInfoProvider;

namespace RumbleRain {
	[BepInDependency("com.rune580.riskofoptions")]
	[NetworkCompatibility(CompatibilityLevel.NoNeedForSync)]
	[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
	public class RumbleRain : BaseUnityPlugin {

		public const string PluginGUID = PluginAuthor + "." + PluginName;
		public const string PluginAuthor = "quasikyo";
		public const string PluginName = "RumbleRain";
		public const string PluginVersion = "0.3.1";

		internal static DeviceManager DeviceManager { get; private set; }

		public void Awake() {
			Log.Init(Logger);
			Log.Info($"Performing setup for ${nameof(RumbleRain)}");

			DeviceManager = new DeviceManager(From(ConfigManager.VibrationBehavior.Value), nameof(RumbleRain));
			DeviceManager.ConnectDevices();

			Run.onRunStartGlobal += (Run _) => {
				StartCoroutine(DeviceManager.PollVibrations());
			};
			Run.onRunDestroyGlobal += (Run _) => {
				StopAllCoroutines();
				DeviceManager.CleanUp();
			};

            On.RoR2.PauseManager.CCTogglePause += (a, b) => { };
			GlobalEventManager.onClientDamageNotified += VibrateDevicesOnDamage;
		}

		public void Update() {
			if (ConfigManager.ToggleVibrationKeybind.Value.IsDown()) {
				Log.Debug($"Toggle hotkey {ConfigManager.ToggleVibrationKeybind.Value} pressed");
				DeviceManager.ToggleConnectedDevices();
			}
		}

		private void VibrateDevicesOnDamage(DamageDealtMessage damageMessage) {
			Log.Debug($"Victim: {damageMessage.victim?.ToString() ?? "reduced to atoms"}");
			Log.Debug($"Attacker: {damageMessage.attacker?.ToString() ?? "reduced to atoms"}");
			if (damageMessage.victim == null) { return; }

			PlayerCharacterMasterController playerController = LocalUserManager.GetFirstLocalUser().cachedMasterController;
			CharacterBody player = playerController.master.GetBody();
			CharacterBody victim = damageMessage.victim.GetComponent<CharacterBody>();
			CharacterBody attacker = damageMessage.attacker?.GetComponent<CharacterBody>();

			bool didPlayerReceiveDamage = player == victim;
			bool didPlayerDealDamage = player == attacker;
			float victimMaxHealth = victim.healthComponent.fullCombinedHealth;
			float percentageOfMaxHealthDamaged = damageMessage.damage / victimMaxHealth;
			if (!ConfigManager.AllowExcessDamage.Value) {
				percentageOfMaxHealthDamaged = Math.Min(percentageOfMaxHealthDamaged, 1);
			}

			Log.Debug($"{attacker} dealt {damageMessage.damage} ({percentageOfMaxHealthDamaged * 100}%) to {victim} max {victimMaxHealth}.");
			Log.Debug($"Was victim local player? {didPlayerReceiveDamage}");
			Log.Debug($"Was attacker local player? {didPlayerDealDamage}");

			double baseSeconds = ConfigManager.BaseVibrationDurationSeconds.Value;
			VibrationInfo vibrationInfo = new VibrationInfo(TimeSpan.FromSeconds(baseSeconds * percentageOfMaxHealthDamaged));
			if (didPlayerDealDamage && ConfigManager.IsRewardingEnabled.Value) {
				vibrationInfo.Intensity = ConfigManager.DamageDealtBaseVibrationIntensity.Value * percentageOfMaxHealthDamaged;
				DeviceManager.SendVibrationInfo(vibrationInfo);
			}
			if (didPlayerReceiveDamage && ConfigManager.IsPunishingEnabled.Value) {
				vibrationInfo.Intensity = ConfigManager.DamageReceivedBaseVibrationIntensity.Value * percentageOfMaxHealthDamaged;
				DeviceManager.SendVibrationInfo(vibrationInfo);
			}
		}
	}
}
