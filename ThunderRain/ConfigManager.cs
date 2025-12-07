using BepInEx;
using BepInEx.Configuration;

using RiskOfOptions;
using RiskOfOptions.Options;

namespace ThunderRain {

	internal static class ConfigManager {

		internal static ConfigFile OptionsConfig { get; set; }

		#region PiShock Info
		internal static ConfigEntry<string> PiShockUsername { get; set; }
		internal static ConfigEntry<string> PiShockApiKey { get; set; }
		internal static ConfigEntry<string> PiShockShareCodes { get; set; }
		#endregion

		#region Activated By
		internal static ConfigEntry<bool> VibrationsFromDealingDamage { get; set; }
		internal static ConfigEntry<bool> VibrationsFromReceivingDamage { get; set; }
		internal static ConfigEntry<bool> ShocksFromDealingDamage { get; set; }
		internal static ConfigEntry<bool> ShocksFromReceivingDamage { get; set; }
		internal static ConfigEntry<bool> VibrationsFromMinionsDealingDamage { get; set; }
		internal static ConfigEntry<bool> VibrationsFromMinionsReceivingDamage { get; set; }
		internal static ConfigEntry<bool> ShocksFromMinionsDealingDamage { get; set; }
		internal static ConfigEntry<bool> ShocksFromMinionsReceivingDamage { get; set; }
		internal static ConfigEntry<bool> ShockOnDeath { get; set; }
		#endregion

		#region Operation Values
		internal static ConfigEntry<int> BaseVibrationDurationSeconds { get; set; }
		internal static ConfigEntry<int> BaseShockDurationSeconds { get; set; }

		internal static ConfigEntry<int> DealingDamageBaseVibrationIntensity { get; set; }
		internal static ConfigEntry<int> DealingDamageBaseShockIntensity { get; set; }

		internal static ConfigEntry<int> ReceivingDamageBaseVibrationIntensity { get; set; }
		internal static ConfigEntry<int> ReceivingDamageBaseShockIntensity { get; set; }

		internal static ConfigEntry<int> MaximumVibrationIntensity { get; set; }
		internal static ConfigEntry<int> MaximumShockIntensity { get; set; }
		internal static ConfigEntry<int> MaximumVibrationDuration { get; set; }
		internal static ConfigEntry<int> MaximumShockDuration { get; set; }

		// internal static ConfigEntry<int> MinionsDealingDamageBaseVibrationIntensity { get; set; }
		// internal static ConfigEntry<int> MinionsDealingDamageBaseShockIntensity { get; set; }
		// internal static ConfigEntry<int> MinionsReceivingDamageBaseVibrationIntensity { get; set; }
		// internal static ConfigEntry<int> MinionsReceivingDamageBaseShockIntensity { get; set; }

		internal static ConfigEntry<int> ShockOnDeathIntensity { get; set; }
		internal static ConfigEntry<int> ShockOnDeathDuration { get; set; }
		#endregion

		#region Operation Behavior
		internal static ConfigEntry<int> TimeSpanSeconds;
		internal static ConfigEntry<DeviceManager.ShockerSelection> ShockerSelection;
		internal static ConfigEntry<bool> AllowExcessDamage { get; set; }
		#endregion

		static ConfigManager() {
			OptionsConfig = new ConfigFile($"{Paths.ConfigPath}\\ThunderRain.cfg", true);
			ModSettingsManager.SetModDescription("Shocks PiShocks in response to in-game damage events.");

			#region PiShock Info
			PiShockUsername = OptionsConfig.Bind(
				"PiShock Info",
				"PiShock Username",
				"",
				"Your PiShock username."
			);
			ModSettingsManager.AddOption(new StringInputFieldOption(PiShockUsername));

			PiShockApiKey = OptionsConfig.Bind(
				"PiShock Info",
				"PiShock API Key",
				"",
				"Generated at https://pishock.com/#/account."
			);
			ModSettingsManager.AddOption(new StringInputFieldOption(PiShockApiKey));

			PiShockShareCodes = OptionsConfig.Bind(
				"PiShock Info",
				"PiShock Share Code",
				"",
				"Share code generated on PiShock control panel. Separate each code with a comma."
			);
			ModSettingsManager.AddOption(new StringInputFieldOption(PiShockShareCodes));
			#endregion

			#region Activated By
			VibrationsFromDealingDamage = OptionsConfig.Bind(
				"Activated By",
				"Vibrations from Dealing Damage",
				true,
				"Generate vibrations when dealing damage."
			);
			ModSettingsManager.AddOption(new CheckBoxOption(VibrationsFromDealingDamage));

			VibrationsFromReceivingDamage = OptionsConfig.Bind(
				"Activated By",
				"Vibrations from Receiving Damage",
				true,
				"Generate vibrations when receiving damage."
			);
			ModSettingsManager.AddOption(new CheckBoxOption(VibrationsFromReceivingDamage));

			ShocksFromDealingDamage = OptionsConfig.Bind(
				"Activated By",
				"Shocks from Dealing Damage",
				false,
				"Generate shocks when dealing damage."
			);
			ModSettingsManager.AddOption(new CheckBoxOption(ShocksFromDealingDamage));

			ShocksFromReceivingDamage = OptionsConfig.Bind(
				"Activated By",
				"Shocks from Receiving Damage",
				false,
				"Generate shocks when receiving damage."
			);
			ModSettingsManager.AddOption(new CheckBoxOption(ShocksFromReceivingDamage));

			VibrationsFromMinionsDealingDamage = OptionsConfig.Bind(
				"Activated By",
				"Vibrations from Minions Dealing Damage",
				false,
				"Generate vibrations when your minions (drones, turrets, etc.) deal damage."
			);
			ModSettingsManager.AddOption(new CheckBoxOption(VibrationsFromMinionsDealingDamage));

			VibrationsFromMinionsReceivingDamage = OptionsConfig.Bind(
				"Activated By",
				"Vibrations from Minions Receiving Damage",
				false,
				"Generate vibrations when your minions (drones, turrets, etc.) receive damage."
			);
			ModSettingsManager.AddOption(new CheckBoxOption(VibrationsFromMinionsReceivingDamage));

			ShocksFromMinionsDealingDamage = OptionsConfig.Bind(
				"Activated By",
				"Shocks from Minions Dealing Damage",
				false,
				"Generate shocks when your minions (drones, turrets, etc.) deal damage."
			);
			ModSettingsManager.AddOption(new CheckBoxOption(ShocksFromMinionsDealingDamage));

			ShocksFromMinionsReceivingDamage = OptionsConfig.Bind(
				"Activated By",
				"Shocks from Minions Receiving Damage",
				false,
				"Generate shocks when your minions (drones, turrets, etc.) receive damage."
			);
			ModSettingsManager.AddOption(new CheckBoxOption(ShocksFromMinionsReceivingDamage));

			ShockOnDeath = OptionsConfig.Bind(
				"Activated By",
				"Shock on Death",
				false,
				"Get shocked on death."
			);
			ModSettingsManager.AddOption(new CheckBoxOption(ShockOnDeath));
			#endregion

			#region Operation Values
			BaseVibrationDurationSeconds = OptionsConfig.Bind(
				"Operation Values",
				"Base Vibration Duration in Seconds",
				3,
				new ConfigDescription(
					"The base vibration duration in seconds to add when damage is dealt or received. Multiplied by (damageDealt / maxHealthOfDamagedEntity).",
					new AcceptableValueRange<int>(0, PiShockValues.MaxApiDurationSeconds)
				)
			);
			ModSettingsManager.AddOption(new IntSliderOption(BaseVibrationDurationSeconds));

			BaseShockDurationSeconds = OptionsConfig.Bind(
				"Operation Values",
				"Base Shock Duration in Seconds",
				1,
				new ConfigDescription(
					"The base shock duration in seconds to add when damage is dealt or received. Multiplied by (damageDealt / maxHealthOfDamagedEntity).",
					new AcceptableValueRange<int>(0, PiShockValues.MaxApiDurationSeconds)
				)
			);
			ModSettingsManager.AddOption(new IntSliderOption(BaseShockDurationSeconds));

			DealingDamageBaseVibrationIntensity = OptionsConfig.Bind(
				"Operation Values",
				"Base Vibration Intensity on Damage Dealt",
				50,
				new ConfigDescription(
					"The base vibration intensity for vibrations generated by dealing damage. Multiplied by (damageDealt / maxHealthOfDamagedEntity).",
					new AcceptableValueRange<int>(0, PiShockValues.MaxApiIntensity)
				)
			);
			ModSettingsManager.AddOption(new IntSliderOption(DealingDamageBaseVibrationIntensity));

			DealingDamageBaseShockIntensity = OptionsConfig.Bind(
				"Operation Values",
				"Base Shock Intensity on Damage Dealt",
				1,
				new ConfigDescription(
					"The base shock intensity for shocks generated by dealing damage. Multiplied by (damageDealt / maxHealthOfDamagedEntity).",
					new AcceptableValueRange<int>(0, PiShockValues.MaxApiIntensity)
				)
			);
			ModSettingsManager.AddOption(new IntSliderOption(DealingDamageBaseShockIntensity));

			ReceivingDamageBaseVibrationIntensity = OptionsConfig.Bind(
				"Operation Values",
				"Base Vibration Intensity on Damage Received",
				50,
				new ConfigDescription(
					"The base vibration intensity for vibrations generated by receiving damage. Multiplied by (damageDealt / maxHealthOfDamagedEntity).",
					new AcceptableValueRange<int>(0, PiShockValues.MaxApiIntensity)
				)
			);
			ModSettingsManager.AddOption(new IntSliderOption(ReceivingDamageBaseVibrationIntensity));

			ReceivingDamageBaseShockIntensity = OptionsConfig.Bind(
				"Operation Values",
				"Base Shock Intensity on Damage Received",
				1,
				new ConfigDescription(
					"The base shock intensity for shocks generated by receiving damage. Multiplied by (damageDealt / maxHealthOfDamagedEntity).",
					new AcceptableValueRange<int>(0, PiShockValues.MaxApiIntensity)
				)
			);
			ModSettingsManager.AddOption(new IntSliderOption(ReceivingDamageBaseShockIntensity));

			MaximumVibrationIntensity = OptionsConfig.Bind(
				"Operation Values",
				"Maximum Vibration Intensity",
				PiShockValues.MaxApiIntensity,
				new ConfigDescription(
					"The maximum vibration intensity for a single operation.",
					new AcceptableValueRange<int>(0, PiShockValues.MaxApiIntensity)
				)
			);
			ModSettingsManager.AddOption(new IntSliderOption(MaximumVibrationIntensity));

			MaximumShockIntensity = OptionsConfig.Bind(
				"Operation Values",
				"Maximum Shock Intensity",
				PiShockValues.MaxApiIntensity,
				new ConfigDescription(
					"The maximum shock intensity for a single operation.",
					new AcceptableValueRange<int>(0, PiShockValues.MaxApiIntensity)
				)
			);
			ModSettingsManager.AddOption(new IntSliderOption(MaximumShockIntensity));

			MaximumVibrationDuration = OptionsConfig.Bind(
				"Operation Values",
				"Maximum Vibration Duration Seconds",
				PiShockValues.MaxApiDurationSeconds,
				new ConfigDescription(
					"The maximum shock duration for a single operation in seconds.",
					new AcceptableValueRange<int>(0, PiShockValues.MaxApiDurationSeconds)
				)
			);
			ModSettingsManager.AddOption(new IntSliderOption(MaximumVibrationDuration));

			MaximumShockDuration = OptionsConfig.Bind(
				"Operation Values",
				"Maximum Shock Duration Seconds",
				PiShockValues.MaxApiDurationSeconds,
				new ConfigDescription(
					"The maximum shock duration for a single operation in seconds.",
					new AcceptableValueRange<int>(0, PiShockValues.MaxApiDurationSeconds)
				)
			);
			ModSettingsManager.AddOption(new IntSliderOption(MaximumShockDuration));

			ShockOnDeathIntensity = OptionsConfig.Bind(
				"Operation Values",
				"Shock on Death Intensity",
				0,
				new ConfigDescription(
					"Intensity of shock to receive on death independent of other calculations. Still subject to configured maximum.",
					new AcceptableValueRange<int>(0, PiShockValues.MaxApiIntensity)
				)
			);
			ModSettingsManager.AddOption(new IntSliderOption(ShockOnDeathIntensity));

			ShockOnDeathDuration = OptionsConfig.Bind(
				"Operation Values",
				"Shock on Death Duration Seconds",
				0,
				new ConfigDescription(
					"Duration of shock to receive on death independent of other calculations. Still subject to configured maximum.",
					new AcceptableValueRange<int>(0, PiShockValues.MaxApiDurationSeconds)
				)
			);
			ModSettingsManager.AddOption(new IntSliderOption(ShockOnDeathDuration));
			#endregion

			#region Operation Behavior
			TimeSpanSeconds = OptionsConfig.Bind(
				"Operation Behavior",
				"Time Span",
				3,
				new ConfigDescription(
					"Sums up values over this many seconds.",
					new AcceptableValueRange<int>(1, 100)
				)
			);
			ModSettingsManager.AddOption(new IntSliderOption(TimeSpanSeconds));

			ShockerSelection = OptionsConfig.Bind(
				"Operation Behavior",
				"Shocker Selection",
				DeviceManager.ShockerSelection.Random,
				"Controls which shockers are selected when commands are sent."
			);
			ModSettingsManager.AddOption(new ChoiceOption(ShockerSelection));

			AllowExcessDamage = OptionsConfig.Bind(
				"Operation Behavior",
				"Allow For Excess Damage",
				false,
				"Allow for excess damage dealt over an entity's max combined health to affect operation intensity."
			);
			ModSettingsManager.AddOption(new CheckBoxOption(AllowExcessDamage));
			#endregion
		}
	}
}
