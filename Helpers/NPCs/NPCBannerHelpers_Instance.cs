using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Helpers.NPCs {
	/// @private
	public partial class NPCBannerHelpers {
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
	}
}
