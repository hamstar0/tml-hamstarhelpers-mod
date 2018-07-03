using HamstarHelpers.DotNetHelpers;
using System;
using Terraria;


namespace HamstarHelpers.WorldHelpers {
	public partial class WorldHelpers {
		[Obsolete( "Recommend using WorldHelpers.GetUniqueIdWithSeed()", false)]
		public static string GetUniqueId() {
			return WorldHelpers.GetUniqueId( false );
		}

		[Obsolete( "Recommend using WorldHelpers.GetUniqueIdWithSeed()", false )]
		public static string GetUniqueId( bool as_file_name ) {
			if( as_file_name ) {
				return FileHelpers.SanitizePath( Main.worldName ) + "@" + Main.worldID;
			} else {
				return FileHelpers.SanitizePath( Main.worldName ) + ":" + Main.worldID;
			}
		}
	}
}
