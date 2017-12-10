using HamstarHelpers.DotNetHelpers.DataStructures;
using System.Collections.Generic;
using Terraria;

namespace HamstarHelpers.NPCHelpers {
	public static class NPCBannerHelpers {
		private static IDictionary<int, int> NpcTypesToBannerItemTypes;
		private static ISet<int> BannerItemTypes;
		private static IDictionary<int, ISet<int>> BannerItemTypesToNpcTypes;



		////////////////

		internal static void InitializeBanners() {
			NPCBannerHelpers.NpcTypesToBannerItemTypes = NPCBannerHelpers.GetNpcToBannerItemTypes();
			NPCBannerHelpers.BannerItemTypesToNpcTypes = new Dictionary<int, ISet<int>>();

			foreach( var kv in NPCBannerHelpers.NpcTypesToBannerItemTypes ) {
				if( !NPCBannerHelpers.BannerItemTypesToNpcTypes.ContainsKey(kv.Value) ) {
					NPCBannerHelpers.BannerItemTypesToNpcTypes[kv.Value] = new HashSet<int>();
				}
				NPCBannerHelpers.BannerItemTypesToNpcTypes[kv.Value].Add( kv.Key );
			}

			NPCBannerHelpers.BannerItemTypes = new HashSet<int>( NPCBannerHelpers.BannerItemTypesToNpcTypes.Keys );
		}


		////////////////

		public static IDictionary<int, int> GetNpcToBannerItemTypes() {
			IDictionary<int, int> npc_types_to_banner_item_types = new Dictionary<int, int>();

			for( int npc_type = 0; npc_type < Main.npcTexture.Length; npc_type++ ) {
				int banner_type = Item.NPCtoBanner( npc_type );
				if( banner_type == 0 ) { continue; }

				int banner_item_type = Item.BannerToItem( banner_type );
				if( banner_item_type >= Main.itemTexture.Length || banner_item_type <= 0 ) { continue; }

				npc_types_to_banner_item_types[npc_type] = banner_item_type;
			}

			return npc_types_to_banner_item_types;
		}

		////////////////

		public static ReadOnlySet<int> GetBannerItemTypes() {
			return new ReadOnlySet<int>( NPCBannerHelpers.BannerItemTypes );
		}

		public static int GetBannerItemTypeOfNpcType( int npc_type ) {
			if( !NPCBannerHelpers.NpcTypesToBannerItemTypes.ContainsKey(npc_type) ) { return -1; }
			return NPCBannerHelpers.NpcTypesToBannerItemTypes[ npc_type ];
		}

		public static ReadOnlySet<int> GetNpcTypesOfBannerItemType( int item_type ) {
			if( !NPCBannerHelpers.BannerItemTypesToNpcTypes.ContainsKey( item_type ) ) { return new ReadOnlySet<int>( new HashSet<int>() ); }
			return new ReadOnlySet<int>( NPCBannerHelpers.BannerItemTypesToNpcTypes[ item_type ] );
		}
	}
}
