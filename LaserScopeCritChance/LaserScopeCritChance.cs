using RoR2;
using BepInEx;
using R2API.Utils;

using static R2API.RecalculateStatsAPI;

namespace LaserScopeCritChance {

	[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
	[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
	public class LaserScopeCritChance : BaseUnityPlugin {

		public const string PluginGUID = PluginAuthor + "." + PluginName;
		public const string PluginAuthor = "quasikyo";
		public const string PluginName = "LaserScopeCritChance";
		public const string PluginVersion = "1.0.1";

		private const string laserScopeInternalName = "CritDamage";
		private ItemIndex LaserScopeItemIndex { get; set; }

		public void Awake() {
			Log.Init(Logger);

			Run.onRunStartGlobal += (Run _) => {
				LaserScopeItemIndex = ItemCatalog.FindItemIndex(laserScopeInternalName);
				GetStatCoefficients += AddLaserScopeCritChance;
			};
			Run.onRunDestroyGlobal += (Run _) => {
				GetStatCoefficients -= AddLaserScopeCritChance;
			};
		}

		private void AddLaserScopeCritChance(CharacterBody body, StatHookEventArgs args) {
			int laserScopeCount = body?.inventory?.GetItemCount(LaserScopeItemIndex) ?? 0;
			Log.Debug($"laserScopeCount: {laserScopeCount}");
			if (laserScopeCount <= 0) { return; }

			args.critAdd += 5;
		}
	}
}
