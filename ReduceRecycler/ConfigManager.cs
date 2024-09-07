using BepInEx;
using BepInEx.Configuration;

using RiskOfOptions;
using RiskOfOptions.Options;

namespace ReduceRecycler {
	internal static class ConfigManager {
		private static ConfigFile OptionsConfig { get; set; }

		internal static ConfigEntry<bool> IsModEnabled { get; set; }
		internal static ConfigEntry<bool> EnableOnlyAfterTeleporter { get; set; }
		internal static ConfigEntry<ReduceRecycler.CooldownReset> CooldownReset { get; set; }

		// static ConfigManager() {
		internal static void Init() {
			OptionsConfig = new ConfigFile(Paths.ConfigPath + "\\ReduceRecycler.cfg", true);
			ModSettingsManager.SetModDescription("Customize the behavior of the recycler equipment to shorten the time between uses.");

			IsModEnabled = OptionsConfig.Bind(
				"Behavior",
				"Enable Mod",
				true,
				"Dynamically enable or disable the mod. If this is unchecked, the mod will not function until re-enabled."
			);
			ModSettingsManager.AddOption(new CheckBoxOption(IsModEnabled));

			EnableOnlyAfterTeleporter = OptionsConfig.Bind(
				"Behavior",
				"Enable Only After Teleporter",
				false,
				"Enable no-cooldown only after the teleporter for the stage has been completed. For stage without a teleporter (Hidden Realms, Void Fields, Commencement), this just makes the Recycler behave as normal."
			);
			ModSettingsManager.AddOption(new CheckBoxOption(EnableOnlyAfterTeleporter));

			CooldownReset = OptionsConfig.Bind(
				"Behavior",
				"Cooldown Reset",
				ReduceRecycler.CooldownReset.AfterUse,
				"When to reset the cooldown of Recycler. AfterUse will reset immediately after use. OnDemand will reset on button press."
			);
			ModSettingsManager.AddOption(new ChoiceOption(CooldownReset));
		}

	}
}
