using HamstarHelpers.Components.DataStructures;
using Terraria;


namespace HamstarHelpers.Helpers.NPCs {
	public partial class NPCIdentityHelpers {
		public static string GetProperUniqueId( int npcType ) {
			var npc = new NPC();
			npc.SetDefaults( npcType );

			return NPCIdentityHelpers.GetProperUniqueId( npc );
		}

		public static string GetProperUniqueId( NPC npc ) {
			if( npc.modNPC == null ) {
				return "Terraria." + npc.type;
			}
			return npc.modNPC.mod.Name + "." + npc.modNPC.Name;
		}

		////

		/*public static string GetUniqueId( NPC npc ) {
			string id = npc.TypeName;

			if( npc.HasGivenName ) { id = npc.GivenName + " " + id; }
			if( npc.modNPC != null ) { id = npc.modNPC.mod.Name + " " + id; }

			if( id != "" ) { return id; }
			return "" + npc.type;
		}*/


		public static string GetQualifiedName( NPC npc ) {
			return Lang.GetNPCNameValue( npc.netID );
		}

		public static string GetQualifiedName( int netid ) {    //npcType?
			return Lang.GetNPCNameValue( netid );
		}

		// TODO: GetVanillaSnapshotHash



		////////////////

		public static ReadOnlyDictionaryOfSets<string, int> NamesToIds {
			get {
				return ModHelpersMod.Instance.NPCIdentityHelpers._NamesToIds;
			}
		}
	}
}
