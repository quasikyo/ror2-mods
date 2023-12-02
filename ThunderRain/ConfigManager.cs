using BepInEx;
using BepInEx.Configuration;

using RiskOfOptions;
using RiskOfOptions.Options;

namespace ThunderRain {

	internal static class ConfigManager {

		internal static ConfigFile OptionsConfig { get; set; }

		internal static ConfigEntry<string> PiShockUsername { get; set; }
		internal static ConfigEntry<string> PiShockApiKey { get; set; }
		internal static ConfigEntry<string> PiShockShareCodes { get; set; }

		static ConfigManager() {
			OptionsConfig = new ConfigFile($"{Paths.ConfigPath}\\ThunderRain.cfg", true);
			ModSettingsManager.SetModDescription("Shocks PiShocks in response to in-game damage events.");

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
				"Share code generated on PiShock control. Separate each code with a comma."
			);
			ModSettingsManager.AddOption(new StringInputFieldOption(PiShockShareCodes));
		}
	}
}
