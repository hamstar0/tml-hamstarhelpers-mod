using HamstarHelpers.DotNetHelpers.DataStructures;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.NPCHelpers {
	public class NPCBannerHelpers {
		private IDictionary<int, int> NpcTypesToBannerItemTypes;
		private ISet<int> BannerItemTypes;
		private IDictionary<int, ISet<int>> BannerItemTypesToNpcTypes;



		////////////////

		internal void InitializeBanners() {
			this.BannerItemTypesToNpcTypes = new Dictionary<int, ISet<int>>();
			this.NpcTypesToBannerItemTypes = NPCBannerHelpers.GetNpcToBannerItemTypes();

			foreach( var kv in this.NpcTypesToBannerItemTypes ) {
				if( !this.BannerItemTypesToNpcTypes.ContainsKey(kv.Value) ) {
					this.BannerItemTypesToNpcTypes[kv.Value] = new HashSet<int>();
				}
				this.BannerItemTypesToNpcTypes[kv.Value].Add( kv.Key );
			}

			this.BannerItemTypes = new HashSet<int>( this.BannerItemTypesToNpcTypes.Keys );
		}


		////////////////

		public static IDictionary<int, int> GetNpcToBannerItemTypes() {
			IDictionary<int, int> npc_types_to_banner_item_types = new Dictionary<int, int>();

			for( int npc_type = 0; npc_type < Main.npcTexture.Length; npc_type++ ) {
				int banner_type = Item.NPCtoBanner( npc_type );
				if( banner_type == 0 ) { continue; }

				int banner_item_type = Item.BannerToItem( banner_type );
				if( banner_item_type >= Main.itemTexture.Length || banner_item_type <= 0 ) { continue; }

				try {
					Item item = new Item();
					item.SetDefaults( banner_item_type );
				} catch( Exception _ ) {
					ErrorLogger.Log( "Could not find banner of item id " + banner_item_type + " for npc id " + npc_type );
					continue;
				}

				npc_types_to_banner_item_types[npc_type] = banner_item_type;
			}

			return npc_types_to_banner_item_types;
		}

		////////////////

		public static ReadOnlySet<int> GetBannerItemTypes() {
			var npc_banner_helpers = HamstarHelpersMod.Instance.NPCBannerHelpers;

			return new ReadOnlySet<int>( npc_banner_helpers.BannerItemTypes );
		}

		public static int GetBannerItemTypeOfNpcType( int npc_type ) {
			var npc_banner_helpers = HamstarHelpersMod.Instance.NPCBannerHelpers;

			if( !npc_banner_helpers.NpcTypesToBannerItemTypes.ContainsKey(npc_type) ) { return -1; }
			return npc_banner_helpers.NpcTypesToBannerItemTypes[ npc_type ];
		}

		public static ReadOnlySet<int> GetNpcTypesOfBannerItemType( int item_type ) {
			var npc_banner_helpers = HamstarHelpersMod.Instance.NPCBannerHelpers;

			if( !npc_banner_helpers.BannerItemTypesToNpcTypes.ContainsKey( item_type ) ) { return new ReadOnlySet<int>( new HashSet<int>() ); }
			return new ReadOnlySet<int>( npc_banner_helpers.BannerItemTypesToNpcTypes[ item_type ] );
		}
	}
}
