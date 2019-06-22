using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.Dusts {
	/** <summary>Assorted static "helper" functions pertaining to dusts.</summary> */
	public static class DustHelpers {
		public static bool IsActive( int who ) {
			return who != 6000 && Main.dust[who].active && Main.dust[who].type != 0;
		}


		public static IList<Dust> GetActive() {
			var list = new List<Dust>();

			for( int i = 0; i < Main.dust.Length; i++ ) {
				Dust dust = Main.dust[i];
				if( dust != null && dust.active && dust.type != 0 ) {
					list.Add( dust );
				}
			}
			return list;
		}
	}
}
