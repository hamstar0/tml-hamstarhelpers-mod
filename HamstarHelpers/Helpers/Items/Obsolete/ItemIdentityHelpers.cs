using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace HamstarHelpers.Helpers.Items {
	/// <summary>
	/// Assorted static "helper" functions pertaining to item identification.
	/// </summary>
	public partial class ItemIdentityHelpers {
		/// @private
		[Obsolete( "use ItemDefinition's ctor", true )]
		public static ItemDefinition GetItemDefinition( string uniqueKey ) {
			string[] segs = uniqueKey.Split( new char[] { ' ' }, 2 );
			return new ItemDefinition( segs[0], segs[1] );
		}
	}
}
