using System;
using System.Collections.Generic;
using HamstarHelpers.Classes.Loadable;


namespace HamstarHelpers.Services.Mods.BossChecklist {
	/// <summary>
	/// Provides a snapshot of boss information from the BossChecklist mod (must be enabled).
	/// </summary>
	public partial class BossChecklistService : ILoadable {
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
	}
}
