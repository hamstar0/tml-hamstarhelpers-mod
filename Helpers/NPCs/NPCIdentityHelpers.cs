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
		/// Gets a (human readable) unique key (as segments) from a given NPC type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static Tuple<string, string> GetUniqueKeySegs( int type ) {
			if( type < -65 || type >= NPCLoader.NPCCount ) {
				throw new ArgumentOutOfRangeException( "Invalid type: " + type );
			}
			if( type < NPCID.Count ) {
				return Tuple.Create( "Terraria", NPCID.Search.GetName( type ) );
			}
			
			var modNPC = NPCLoader.GetNPC( type );
			return Tuple.Create( modNPC.mod.Name, modNPC.Name );
		}


		////

		/// <summary>
		/// </summary>
		/// <param name="uniqueKey"></param>
		/// <returns></returns>
		public static NPCDefinition GetNPCDefinition( string uniqueKey ) {
			string[] segs = uniqueKey.Split( new char[] { ' ' }, 2 );
			return new NPCDefinition( segs[0], segs[1] );
		}


		////

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
