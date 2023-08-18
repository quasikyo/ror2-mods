﻿using BepInEx;
using BepInEx.Configuration;
using RoR2;

using static RumbleRain.VibrationInfoProvider;

namespace RumbleRain {
	internal static class ConfigManager {
		internal static ConfigFile VibrationConfigFile { get; set; }

		internal static ConfigEntry<string> ServerUri { get; set; }
		internal static ConfigEntry<float> PollingRateSeconds { get; set; }

		internal static ConfigEntry<bool> IsRewardingEnabled { get; set; }
		internal static ConfigEntry<bool> IsPunishingEnabled { get; set; }

		internal static ConfigEntry<double> VibrationIntensityMultiplierOnDamageReceived { get; set; }
		internal static ConfigEntry<double> VibrationIntensityMultiplierOnDamageDealt { get; set; }
		internal static ConfigEntry<double> MaximumVibrationIntensity { get; set; }
		internal static ConfigEntry<double> BaseVibrationDurationSeconds { get; set; }
		internal static ConfigEntry<double> MaximumVibrationDurationSeconds { get; set; }

		internal static ConfigEntry<VibrationBehavior> VibrationBehavior { get; set; }
		internal static ConfigEntry<bool> AccountForExcessDamage { get; set; }

        [ConCommand(commandName = "rumblerain_config_reload", flags = ConVarFlags.None, helpText = "Reloads the config file for RumbleRain.")]
		internal static void ReloadConfig(ConCommandArgs _) {
			VibrationConfigFile.Reload();
		}

		internal static void SetupConfig() {
			VibrationConfigFile = new ConfigFile(Paths.ConfigPath + "\\RumbleRain.cfg", true);

			ServerUri = VibrationConfigFile.Bind(
				"Devices",
				"Server Uri",
				"ws://localhost:12345",
				"URI of the Intiface server."
			);
			PollingRateSeconds = VibrationConfigFile.Bind(
				"Devices",
				"Polling Rate Seconds",
				0.2f,
				"How often to update vibrations."
			);

			IsRewardingEnabled = VibrationConfigFile.Bind(
				"Activated By",
				"Dealing Damage",
				false,
				"Receive vibrations when dealing damage."
			);
			IsPunishingEnabled = VibrationConfigFile.Bind(
				"Activated By",
				"Receiving Damage",
				true,
				"Receive vibrations when receiving damage."
			);

			VibrationIntensityMultiplierOnDamageDealt = VibrationConfigFile.Bind(
				"Vibration Values",
				"Vibration Intensity Multiplier On Damage Dealt",
				0.05,
				"Multiplier for vibrations generated by dealing damage."
			);
			VibrationIntensityMultiplierOnDamageReceived = VibrationConfigFile.Bind(
				"Vibration Values",
				"Vibration Intensity Multiplier On Damage Received",
				1.0,
				"Multiplier for vibrations generated by receiving damage."
			);
			MaximumVibrationIntensity = VibrationConfigFile.Bind(
				"Vibration Values",
				"Maximum Vibration Intensity",
				1.0,
				"Max percentage of total vibration intensity expressed as a decimal (0.1 for 10%, 1 for 100%)."
			);
			BaseVibrationDurationSeconds = VibrationConfigFile.Bind(
				"Vibration Values",
				"Base Vibration Duration Seconds",
				5.0,
				"The base vibration duration in seconds to add when damage is dealt or received. Multiplied by (damageDealt / maxHealthOfDamagedEntity)."
			);
			MaximumVibrationDurationSeconds = VibrationConfigFile.Bind(
				"Vibration Values",
				"Maximum Vibration Duration Seconds",
				30.0,
				"The longest amount of seconds vibrations will last for assuming no new vibrations are generated."
			);

			VibrationBehavior = VibrationConfigFile.Bind(
				"Vibration Behavior",
				"Vibration Behavior",
				VibrationInfoProvider.VibrationBehavior.AdditiveWithLinearDecay,
				"How vibrations should handle new information and evolve over time."
			);
			AccountForExcessDamage = VibrationConfigFile.Bind(
				"Vibration Behavior",
				"Account For Excess Damage",
				false,
				"Account for excess damage dealt over an entity's max combined health."
			);
		}
	}
}
