using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Services.Hooks.LoadHooks;


namespace HamstarHelpers.Services.Mods.BossChecklist {
	/// <summary>
	/// Provides a snapshot of boss information from the BossChecklist mod (must be enabled).
	/// </summary>
	public class BossChecklistService : ILoadable {
		// Boss Checklist might add new features, so a version is passed into GetBossInfo.
		// If a new version of the GetBossInfo Call is implemented, find this class in the Boss Checklist Github once again and
		// replace this version with the new version:
		// https://github.com/JavidPack/BossChecklist/blob/master/BossChecklistIntegrationExample.cs

		/// <summary></summary>
		public class BossInfo {
			/// <summary>
			/// Equal to ModName BossClassName
			/// </summary>
			public string UniqueKey { get; internal set; } = ""; // equal to "modSource internalName"
			/// <summary></summary>
			public string ModName { get; internal set; } = "";
			/// <summary></summary>
			public string BossClassName { get; internal set; } = "";
			/// <summary></summary>
			public string BossDisplayName { get; internal set; } = "";
			/// <summary></summary>
			public float ProgressionValue { get; internal set; } = 0f; // See https://github.com/JavidPack/BossChecklist/blob/master/BossTracker.cs#L13 for vanilla boss values
			/// <summary></summary>
			public Func<bool> IsDowned { get; internal set; } = () => false;
			/// <summary></summary>
			public bool IsBoss { get; internal set; } = false;
			/// <summary></summary>
			public bool IsMiniboss { get; internal set; } = false;
			/// <summary></summary>
			public bool IsEvent { get; internal set; } = false;
			/// <summary></summary>
			public List<int> SegmentNpcIDs { get; internal set; } = new List<int>(); // Does not include minions, only npcids that count towards the NPC still being alive.
			/// <summary></summary>
			public List<int> SpawnItemTypes { get; internal set; } = new List<int>();
			/// <summary></summary>
			public List<int> LootItemTypes { get; internal set; } = new List<int>();
			/// <summary></summary>
			public List<int> CollectiblesLootItemTypes { get; internal set; } = new List<int>();
		}



		////////////////

		/// <summary></summary>
		public static readonly Version MinimumBossChecklistVersion = new Version( 1, 1 );

		////

		/// <summary></summary>
		public static IReadOnlyDictionary<string, BossInfo> BossInfoTable { get; private set;  }



		////////////////

		private IDictionary<string, BossInfo> _BossInfoTable = new Dictionary<string, BossInfo>();



		////////////////

		void ILoadable.OnModsLoad() {
			BossChecklistService.BossInfoTable = new ReadOnlyDictionary<string, BossInfo>( this._BossInfoTable );
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

			var rawBossInfoTable = bcMod.Call( "GetBossInfoDictionary", ModHelpersMod.Instance, bcMinVers.ToString() )
				as Dictionary<string, Dictionary<string, object>>;
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
		}
	}
}
