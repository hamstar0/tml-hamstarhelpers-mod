using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.NPCs {
	/// <summary>
	/// Assorted static "helper" functions pertaining to finding world NPCs.
	/// </summary>
	public class NPCFinderHelpers {
		private static IDictionary<int, int> AnyWhoOfType = new Dictionary<int, int>();



		////////////////

		/// <summary>
		/// Finds first active, world NPC of the given type.
		/// </summary>
		/// <param name="npcType"></param>
		/// <returns></returns>
		public static NPC FindFirstNpcByType( int npcType ) {
			NPC npc = null;

			if( NPCFinderHelpers.AnyWhoOfType.Keys.Contains( npcType ) ) {
				npc = Main.npc[NPCFinderHelpers.AnyWhoOfType[npcType]];

				if( npc != null && npc.active && npc.type == npcType ) {
					return npc;
				} else {
					NPCFinderHelpers.AnyWhoOfType.Remove( npcType );
				}
			}

			for( int i = 0; i < Main.npc.Length; i++ ) {
				npc = Main.npc[i];

				if( npc != null && npc.active && npc.type == npcType ) {
					NPCFinderHelpers.AnyWhoOfType[npcType] = i;
					break;
				}
			}

			return npc;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="position"></param>
		/// <param name="minWorldDistance"></param>
		/// <param name="maxWorldDistance"></param>
		/// <param name="isFriendly"></param>
		/// <returns></returns>
		public static IList<int> FindNPCsNearby( Vector2 position, int minWorldDistance, int maxWorldDistance, bool? isFriendly = null ) {
			var npcsWhos = new List<int>();
			int max = Main.npc.Length;
			int minSqr = minWorldDistance * minWorldDistance;
			int maxSqr = maxWorldDistance * maxWorldDistance;

			for( int i=0; i<max; i++ ) {
				NPC npc = Main.npc[i];
				if( npc?.active != true ) {
					continue;
				}
				if( isFriendly.HasValue && npc.friendly != isFriendly.Value ) {
					continue;
				}

				float distSqr = ( npc.position - position ).LengthSquared();
				if( distSqr < minSqr || distSqr >= maxSqr ) {
					continue;
				}

				npcsWhos.Add( i );
			}

			return npcsWhos;
		}
	}
}
