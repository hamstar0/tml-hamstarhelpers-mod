using System;
using Terraria.ModLoader;


namespace HamstarHelpers.Utilities.Web {
	[System.Obsolete( "use ModVersionGet.ModVersionGet", true )]
	public class ModVersionGet {
		public static Version GetLatestKnownVersion( Mod mod, out bool found ) {
			return WebHelpers.ModVersionGet.GetLatestKnownVersion( mod, out found );
		}
	}
}
