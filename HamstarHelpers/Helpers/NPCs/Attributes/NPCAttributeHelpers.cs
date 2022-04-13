using HamstarHelpers.Classes.DataStructures;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.NPCs.Attributes {
	/// <summary>
	/// Assorted static "helper" functions pertaining to gameplay attributes of NPCs.
	/// </summary>
	public partial class NPCAttributeHelpers {
		/// <summary>
		/// Table of NPC ids by qualified names.
		/// </summary>
		public static ReadOnlyDictionaryOfSets<string, int> DisplayNamesToIds =>
			ModHelpersMod.Instance.NPCAttributeHelpers._DisplayNamesToIds;



		////////////////

		/// <summary>
		/// Gets an NPC's qualified (human readable) name.
		/// </summary>
		/// <param name="npc"></param>
		/// <returns></returns>
		public static string GetQualifiedName( NPC npc ) {
			try {
				return Lang.GetNPCNameValue( npc.netID );
			} catch {
				return "";
			}
		}

		/// <summary>
		/// Gets an NPC's qualified (human readable) name.
		/// </summary>
		/// <param name="netid"></param>
		/// <returns></returns>
		public static string GetQualifiedName( int netid ) {    //npcType?
			try {
				return Lang.GetNPCNameValue( netid );
			} catch {
				return "";
			}
		}
	}
}
