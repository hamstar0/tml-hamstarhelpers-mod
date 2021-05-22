using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.ModLoader.Config;


namespace HamstarHelpers.Libraries.NPCs {
	/// <summary>
	/// Assorted static "helper" functions pertaining to NPC identification.
	/// </summary>
	public partial class NPCIdentityLibraries {
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
	}
}
