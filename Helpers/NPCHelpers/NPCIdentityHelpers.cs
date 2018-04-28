using HamstarHelpers.DotNetHelpers.DataStructures;
using HamstarHelpers.Helpers.DotNetHelpers.DataStructures;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.NPCHelpers {
	public partial class NPCIdentityHelpers {
		public static string GetUniqueId( NPC npc ) {
			string id = npc.TypeName;

			if( npc.HasGivenName ) { id = npc.GivenName + " " + id; }
			if( npc.modNPC != null ) { id = npc.modNPC.mod.Name + " " + id; }

			if( id != "" ) { return id; }
			return "" + npc.type;
		}
		
		// TODO: GetVanillaSnapshotHash



		////////////////

		public static ReadOnlyDictionaryOfSets<string, int> NamesToIds {
			get {
				return HamstarHelpersMod.Instance.NPCIdentityHelpers._NamesToIds;
			}
		}



		////////////////
		
		private ReadOnlyDictionaryOfSets<string, int> _NamesToIds = null;


		////////////////

		internal void PopulateNames() {
			var dict = new Dictionary<string, ISet<int>>();

			for( int i = 1; i < NPCLoader.NPCCount; i++ ) {
				string name = Lang.GetNPCNameValue( i );
				
				if( dict.ContainsKey(name) ) {
					dict[name].Add( i );
				} else {
					dict[name] = new HashSet<int>() { i };
				}
			}

			this._NamesToIds = new ReadOnlyDictionaryOfSets<string, int>( dict );
		}


		////////////////

		[System.Obsolete( "use NPCHelpers.LooselyAssessThreat", true )]
		public static float LooselyAssessThreat( NPC npc ) {
			return NPCHelpers.LooselyAssessThreat( npc );
		}
	}
}
