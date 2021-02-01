using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Services.Hooks.LoadHooks;


namespace HamstarHelpers.Services.Mods.BossChecklist {
	/// <summary>
	/// Provides a snapshot of boss information from the BossChecklist mod (must be enabled).
	/// </summary>
	public partial class BossChecklistService : ILoadable {
		/// <summary></summary>
		public static readonly Version MinimumBossChecklistVersion = new Version( 1, 1 );

		////

		/// <summary></summary>
		public static IReadOnlyDictionary<string, BossInfo> BossInfoTable { get; private set;  }



		////////////////

		private IDictionary<string, BossInfo> _BossInfoTable = new Dictionary<string, BossInfo>();



		////////////////

		void ILoadable.OnModsLoad() {
			BossChecklistService.BossInfoTable = new ReadOnlyDictionary<string, BossInfo>(
				this._BossInfoTable
			);
		}

		void ILoadable.OnPostModsLoad() {
			LoadHooks.AddPostModLoadHook( this.OnPostAddRecipes );
		}

		void ILoadable.OnModsUnload() {
			BossChecklistService.BossInfoTable = null;
		}

		////

		internal void OnPostAddRecipes() {
			Mod bcMod = ModLoader.GetMod( "BossChecklist" );
			Version bcMinVers = BossChecklistService.MinimumBossChecklistVersion;
			if( bcMod == null || bcMod.Version < bcMinVers ) {
				return;
			}

			var rawBossInfoTable = bcMod.Call(
				"GetBossInfoDictionary",
				ModHelpersMod.Instance,
				bcMinVers.ToString()
			) as Dictionary<string, Dictionary<string, object>>;

			if( rawBossInfoTable == null ) {
				return;
			}

			this._BossInfoTable = rawBossInfoTable.ToDictionary( boss => boss.Key, boss => {
				Dictionary<string, object> bossInfo = boss.Value;

				return new BossInfo() {
					UniqueKey = bossInfo.GetOrDefault( "key" ) as string ?? "",
					ModName = bossInfo.GetOrDefault( "modSource" ) as string ?? "",
					BossClassName = bossInfo.GetOrDefault( "internalName" ) as string ?? "",
					BossDisplayName = bossInfo.GetOrDefault( "displayName" ) as string ?? "",
					ProgressionValue = Convert.ToSingle( bossInfo.GetOrDefault( "progression" ) ?? 0f ),
					IsDowned = bossInfo.GetOrDefault( "downed" ) as Func<bool> ?? ( () => false ),
					IsBoss = Convert.ToBoolean( bossInfo.GetOrDefault( "isBoss" ) ?? false ),
					IsMiniboss = Convert.ToBoolean( bossInfo.GetOrDefault( "isMiniboss" ) ?? false ),
					IsEvent = Convert.ToBoolean( bossInfo.GetOrDefault( "isEvent" ) ?? false ),
					SegmentNpcIDs = bossInfo.GetOrDefault( "npcIDs" ) as List<int> ?? new List<int>(),
					SpawnItemTypes = bossInfo.GetOrDefault( "spawnItem" ) as List<int> ?? new List<int>(),
					LootItemTypes = bossInfo.GetOrDefault( "loot" ) as List<int> ?? new List<int>(),
					CollectiblesLootItemTypes = bossInfo.GetOrDefault( "collection" ) as List<int> ?? new List<int>(),
				};
			} );

			BossChecklistService.BossInfoTable = new ReadOnlyDictionary<string, BossInfo>(
				this._BossInfoTable
			);
		}
	}
}
