using System.Collections.Generic;


namespace HamstarHelpers.NPCHelpers {
	public static class NPCBannerHelpers {
		internal static IDictionary<int, int> NpcTypesToBannerItemTypes;
		internal static IDictionary<int, ISet<int>> BannerItemTypesToNpcTypes;

		internal static void InitializeBanners( IDictionary<int, int> npcs_to_banners ) {
			NPCBannerHelpers.NpcTypesToBannerItemTypes = npcs_to_banners;
			NPCBannerHelpers.BannerItemTypesToNpcTypes = new Dictionary<int, ISet<int>>();

			foreach( var kv in npcs_to_banners ) {
				if( !NPCBannerHelpers.BannerItemTypesToNpcTypes.ContainsKey(kv.Value) ) {
					NPCBannerHelpers.BannerItemTypesToNpcTypes[kv.Value] = new HashSet<int>();
				}
				NPCBannerHelpers.BannerItemTypesToNpcTypes[kv.Value].Add( kv.Key );
			}
		}


		public static IEnumerable<int> GetBannerItemTypes() {
			return NPCBannerHelpers.BannerItemTypesToNpcTypes.Keys;
		}

		public static int GetBannerItemTypeOfNpcType( int npc_type ) {
			if( !NPCBannerHelpers.NpcTypesToBannerItemTypes.ContainsKey(npc_type) ) { return -1; }
			return NPCBannerHelpers.NpcTypesToBannerItemTypes[ npc_type ];
		}

		public static ISet<int> GetNpcTypesOfBannerItemType( int item_type ) {
			if( !NPCBannerHelpers.BannerItemTypesToNpcTypes.ContainsKey( item_type ) ) { return new HashSet<int>(); }
			return NPCBannerHelpers.BannerItemTypesToNpcTypes[ item_type ];
		}
	}
}
