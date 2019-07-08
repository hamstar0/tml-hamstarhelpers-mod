using HamstarHelpers.Components.DataStructures;
using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.NPCs {
	/// <summary>
	/// Assorted static "helper" functions pertaining to NPC-dropped banners.
	/// </summary>
	public partial class NPCBannerHelpers {
		/// <summary>
		/// Gets table of npc types to their respective banner item types.
		/// </summary>
		/// <returns></returns>
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

		/// <summary>
		/// Gets all banner item types.
		/// </summary>
		/// <returns></returns>
		public static ReadOnlySet<int> GetBannerItemTypes() {
			var npcBannerHelpers = ModHelpersMod.Instance.NPCBannerHelpers;

			return new ReadOnlySet<int>( npcBannerHelpers.BannerItemTypes );
		}

		/// <summary>
		/// Gets the banner item type of a given NPC type.
		/// </summary>
		/// <param name="npcType"></param>
		/// <returns></returns>
		public static int GetBannerItemTypeOfNpcType( int npcType ) {
			var npcBannerHelpers = ModHelpersMod.Instance.NPCBannerHelpers;

			if( !npcBannerHelpers.NpcTypesToBannerItemTypes.ContainsKey(npcType) ) { return -1; }
			return npcBannerHelpers.NpcTypesToBannerItemTypes[ npcType ];
		}

		/// <summary>
		/// Gets all NPC types of a given banner item type.
		/// </summary>
		/// <param name="itemType"></param>
		/// <returns></returns>
		public static ReadOnlySet<int> GetNpcTypesOfBannerItemType( int itemType ) {
			var npcBannerHelpers = ModHelpersMod.Instance.NPCBannerHelpers;

			if( !npcBannerHelpers.BannerItemTypesToNpcTypes.ContainsKey( itemType ) ) { return new ReadOnlySet<int>( new HashSet<int>() ); }
			return new ReadOnlySet<int>( npcBannerHelpers.BannerItemTypesToNpcTypes[ itemType ] );
		}
	}
}
