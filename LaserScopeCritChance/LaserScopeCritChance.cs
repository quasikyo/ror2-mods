using RoR2;
using BepInEx;
using R2API.Utils;

using static R2API.RecalculateStatsAPI;

namespace LaserScopeCritChance {

	[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
	[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
	public class LaserScopeCritChance : BaseUnityPlugin {

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
