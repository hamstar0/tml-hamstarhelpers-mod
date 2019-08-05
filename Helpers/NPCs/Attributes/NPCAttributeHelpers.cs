using System;
using Terraria;


namespace HamstarHelpers.Helpers.NPCs.Attributes {
	/// <summary>
	/// Assorted static "helper" functions pertaining to gameplay attributes of NPCs.
	/// </summary>
	public partial class NPCAttributeHelpers {
		/// <summary>
		/// Gets an NPC's qualified (human readable) name.
		/// </summary>
		/// <param name="npc"></param>
		/// <returns></returns>
		public static string GetQualifiedName( NPC npc ) {
			return Lang.GetNPCNameValue( npc.netID );
		}

		/// <summary>
		/// Gets an NPC's qualified (human readable) name.
		/// </summary>
		/// <param name="netid"></param>
		/// <returns></returns>
		public static string GetQualifiedName( int netid ) {    //npcType?
			return Lang.GetNPCNameValue( netid );
		}
	}
}
