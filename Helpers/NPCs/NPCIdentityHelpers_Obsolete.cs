using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.ModLoader.Config;


namespace HamstarHelpers.Helpers.NPCs {
	/// <summary>
	/// Assorted static "helper" functions pertaining to NPC identification.
	/// </summary>
	public partial class NPCIdentityHelpers {
		/// <summary>
		/// Gets an NPC type from a given unique key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		[Obsolete( "use NPCID.TypeFromUniqueKey(string)" )]
		public static int TypeFromUniqueKey( string key ) {
			string[] parts = key.Split( new char[] { ' ' }, 2 );

			if( parts.Length != 2 ) {
				return 0;
			}
			return NPCIdentityHelpers.TypeFromUniqueKey( parts[0], parts[1] );
		}

		/// <summary>
		/// Gets an NPC type from a given unique key.
		/// </summary>
		/// <param name="mod"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		[Obsolete( "use NPCID.TypeFromUniqueKey(string, string)" )]
		public static int TypeFromUniqueKey( string mod, string name ) {
			if( mod == "Terraria" ) {
				if( !NPCID.Search.ContainsName( name ) ) {
					return 0;
				}
				return NPCID.Search.GetId( name );
			}
			return ModLoader.GetMod( mod )?.NPCType( name ) ?? 0;
		}

		// TODO: GetVanillaSnapshotHash
	}
}
