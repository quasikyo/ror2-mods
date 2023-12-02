using BepInEx;
using R2API.Utils;

namespace ThunderRain {

	[BepInDependency("com.rune580.riskofoptions")]
	[NetworkCompatibility(CompatibilityLevel.NoNeedForSync)]
	[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
	public class ThunderRain : BaseUnityPlugin {

		public const string PluginGUID = PluginAuthor + "." + PluginName;
		public const string PluginAuthor = "quasikyo";
		public const string PluginName = "ThunderRain";
		public const string PluginVersion = "1.0.0";

		// private PiShockUser UserInfo { get; set; }
		// private PiShockApiClient PiShock { get; set; }

		public void Awake() {
			Log.Init(Logger);
			Log.Info($"Performing setup for {nameof(ThunderRain)}");

			new DeviceManager(nameof(ThunderRain));
			// UserInfo = new PiShockUser {
			// 	Username = ConfigManager.PiShockUsername.Value,
			// 	ApiKey = ConfigManager.PiShockApiKey.Value,
			// };
			// PiShock = new PiShockApiClient {
			// 	DisplayName = nameof(ThunderRain)
			// };
			// Log.Info(UserInfo);
			// Log.Info(PiShock);
        }
	}
}
