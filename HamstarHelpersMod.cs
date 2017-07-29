using HamstarHelpers.MiscHelpers;
using HamstarHelpers.NPCHelpers;
using HamstarHelpers.Utilities.Messages;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;


namespace HamstarHelpers {
	public class HamstarHelpersMod : Mod {
		public HamstarHelpersMod() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}

		public override void PostDrawInterface( SpriteBatch sb ) {
			PlayerMessage.DrawPlayerLabels( sb );
			SimpleMessage.DrawMessage( sb );

			DebugHelpers.PrintToBatch( sb );
			DebugHelpers.Once = false;
			DebugHelpers.OnceInAWhile--;
		}
		
		public override bool HijackSendData( int who_am_i, int msg_type, int remote_client, int ignore_client, NetworkText text, int number, float number2, float number3, float number4, int number5, int number6, int number7 ) {
			if( Main.netMode == 2 ) {	// Server
				if( NPCSpawnInfoHelpers.IsGaugingSpawnRates && msg_type == 23 ) {
					if( number >= 0 && number <= Main.npc.Length ) {
						NPC npc = Main.npc[number];

						if( npc != null && npc.active ) {
							NPCSpawnInfoHelpers.AddSpawn( npc.type );
							npc.active = false;
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}
