﻿using System;

using BepInEx;
using BepInEx.Configuration;

using RiskOfOptions;
using RiskOfOptions.Options;
using RiskOfOptions.OptionConfigs;

using static RumbleRain.VibrationInfoProvider;

namespace RumbleRain {
	internal static class ConfigManager {
		internal static ConfigFile VibrationConfigFile { get; set; }

		internal static ConfigEntry<string> ServerUri { get; set; }
		internal static ConfigEntry<float> PollingRateSeconds { get; set; }

		internal static ConfigEntry<bool> IsRewardingEnabled { get; set; }
		internal static ConfigEntry<bool> IsPunishingEnabled { get; set; }

		internal static ConfigEntry<float> DamageDealtBaseVibrationIntensity { get; set; }
		internal static ConfigEntry<float> DamageReceivedBaseVibrationIntensity { get; set; }
		internal static ConfigEntry<float> MaximumVibrationIntensity { get; set; }
		internal static ConfigEntry<float> BaseVibrationDurationSeconds { get; set; }
		internal static ConfigEntry<float> MaximumVibrationDurationSeconds { get; set; }

		internal static ConfigEntry<VibrationBehavior> VibrationBehavior { get; set; }
		internal static ConfigEntry<bool> AllowExcessDamage { get; set; }

		static ConfigManager() {
			VibrationConfigFile = new ConfigFile(Paths.ConfigPath + "\\RumbleRain.cfg", true);
			ModSettingsManager.SetModDescription("Vibrate BPio-capable devices when receiving and/or dealing damage.");

			ServerUri = VibrationConfigFile.Bind(
				"Devices",
				"Server Uri",
				"ws://localhost:12345",
				"URI of the Intiface server."
			);
			ModSettingsManager.AddOption(new StringInputFieldOption(ServerUri, true));

			PollingRateSeconds = VibrationConfigFile.Bind(
				"Devices",
				"Polling Rate Seconds",
				0.2f,
				"How often to update vibrations."
			);
			ModSettingsManager.AddOption(new StepSliderOption(
					PollingRateSeconds,
					new StepSliderConfig() { min = 0.1f, max = 3f, increment = 0.1f }
				)
			);

			IsRewardingEnabled = VibrationConfigFile.Bind(
				"Activated By",
				"Dealing Damage",
				false,
				"Receive vibrations when dealing damage."
			);
			ModSettingsManager.AddOption(new CheckBoxOption(IsRewardingEnabled));

			IsPunishingEnabled = VibrationConfigFile.Bind(
				"Activated By",
				"Receiving Damage",
				true,
				"Receive vibrations when receiving damage."
			);
			ModSettingsManager.AddOption(new CheckBoxOption(IsPunishingEnabled));

			DamageDealtBaseVibrationIntensity = VibrationConfigFile.Bind(
				"Vibration Values",
				"Damage Dealt Base Vibration Intensity",
				0.05f,
				"Base vibration intensity percentage for vibrations generated by dealing damage. Multiplied by (damageDealt / maxHealthOfDamagedEntity)."
			);
			ModSettingsManager.AddOption(new StepSliderOption(
					DamageDealtBaseVibrationIntensity,
					new StepSliderConfig() { min = 0f, max = 1f, increment = 0.01f }
				)
			);

			DamageReceivedBaseVibrationIntensity = VibrationConfigFile.Bind(
				"Vibration Values",
				"Damage Received Base Vibration Intensity",
				1.0f,
				"Base vibration intensity percentage for vibrations generated by receiving damage. Multiplied by (damageDealt / maxHealthOfDamagedEntity)."
			);
			ModSettingsManager.AddOption(new StepSliderOption(
					DamageReceivedBaseVibrationIntensity,
					new StepSliderConfig() { min = 0f, max = 1f, increment = 0.01f }
				)
			);

			MaximumVibrationIntensity = VibrationConfigFile.Bind(
				"Vibration Values",
				"Maximum Vibration Intensity",
				1.0f,
				"Max percentage of total vibration intensity expressed as a decimal (0.1 for 10%, 1 for 100%)."
			);
			ModSettingsManager.AddOption(new StepSliderOption(
					MaximumVibrationIntensity,
					new StepSliderConfig() { min = 0f, max = 1f, increment = 0.01f }
				)
			);

			MaximumVibrationDurationSeconds = VibrationConfigFile.Bind(
				"Vibration Values",
				"Maximum Vibration Duration Seconds",
				20.0f,
				"The longest amount of seconds vibrations will last for assuming no new vibrations are generated."
			);
			ModSettingsManager.AddOption(new StepSliderOption(
					MaximumVibrationDurationSeconds,
					new StepSliderConfig() { min = 0f, max = (float)TimeSpan.FromMinutes(5).TotalSeconds, increment = 0.5f }
				)
			);

			BaseVibrationDurationSeconds = VibrationConfigFile.Bind(
				"Vibration Values",
				"Base Vibration Duration Seconds",
				5.0f,
				"The base vibration duration in seconds to add when damage is dealt or received. Multiplied by (damageDealt / maxHealthOfDamagedEntity)."
			);
			ModSettingsManager.AddOption(new StepSliderOption(
					BaseVibrationDurationSeconds,
					new StepSliderConfig() { min = 0f, increment = 0.5f }
				)
			);

			VibrationBehavior = VibrationConfigFile.Bind(
				"Vibration Behavior",
				"Vibration Behavior",
				VibrationInfoProvider.VibrationBehavior.AdditiveWithLinearDecay,
				"How vibrations should handle new information and evolve over time."
			);
			ModSettingsManager.AddOption(new ChoiceOption(VibrationBehavior, true));

			AllowExcessDamage = VibrationConfigFile.Bind(
				"Vibration Behavior",
				"Allow For Excess Damage",
				false,
				"Allow for excess damage dealt over an entity's max combined health to affect vibrations."
			);
			ModSettingsManager.AddOption(new CheckBoxOption(AllowExcessDamage));
		}
	}
}
