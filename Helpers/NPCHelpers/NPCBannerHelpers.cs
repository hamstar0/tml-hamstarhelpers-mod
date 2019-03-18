using HamstarHelpers.Components.DataStructures;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.NPCHelpers {
	public partial class NPCBannerHelpers {
		public static IDictionary<int, int> GetNpcToBannerItemTypes() {
			IDictionary<int, int> npcTypesToBannerItemTypes = new Dictionary<int, int>();

			for( int npcType = 0; npcType < Main.npcTexture.Length; npcType++ ) {
				int bannerType = Item.NPCtoBanner( npcType );
				if( bannerType == 0 ) { continue; }

				int bannerItemType = Item.BannerToItem( bannerType );
				if( bannerItemType >= Main.itemTexture.Length || bannerItemType <= 0 ) { continue; }

				try {
					Item item = new Item();
					item.SetDefaults( bannerItemType );
				} catch( Exception ) {
					LogHelpers.Log( "Could not find banner of item id " + bannerItemType + " for npc id " + npcType );
					continue;
				}

				npcTypesToBannerItemTypes[npcType] = bannerItemType;
			}

			return npcTypesToBannerItemTypes;
		}

		////////////////

		public static ReadOnlySet<int> GetBannerItemTypes() {
			var npcBannerHelpers = ModHelpersMod.Instance.NPCBannerHelpers;

			return new ReadOnlySet<int>( npcBannerHelpers.BannerItemTypes );
		}

		public static int GetBannerItemTypeOfNpcType( int npcType ) {
			var npcBannerHelpers = ModHelpersMod.Instance.NPCBannerHelpers;

			if( !npcBannerHelpers.NpcTypesToBannerItemTypes.ContainsKey(npcType) ) { return -1; }
			return npcBannerHelpers.NpcTypesToBannerItemTypes[ npcType ];
		}

		public static ReadOnlySet<int> GetNpcTypesOfBannerItemType( int itemType ) {
			var npcBannerHelpers = ModHelpersMod.Instance.NPCBannerHelpers;

			if( !npcBannerHelpers.BannerItemTypesToNpcTypes.ContainsKey( itemType ) ) { return new ReadOnlySet<int>( new HashSet<int>() ); }
			return new ReadOnlySet<int>( npcBannerHelpers.BannerItemTypesToNpcTypes[ itemType ] );
		}
	}
}
