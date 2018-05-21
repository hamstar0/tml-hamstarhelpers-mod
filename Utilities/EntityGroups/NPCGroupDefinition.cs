using Newtonsoft.Json;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Utilities.EntityGroups {
	public class NPCGroupDefinition : EntityGroupDefinition<NPC> {
		public static IList<NPC> GetNpcGroup( string query_json ) {
			var def = JsonConvert.DeserializeObject<NPCGroupDefinition>( query_json );
			return def.GetGroup();
		}

		
		////////////////

		private readonly NPC[] MyPool = null;

		public override NPC[] GetPool() {
			if( this.MyPool == null ) {
				for( int i = 0; i < Main.npcTexture.Length; i++ ) {
					this.MyPool[i] = new NPC();
					this.MyPool[i].SetDefaults( i );
				}
			}
			return this.MyPool;
		}
	}
}
