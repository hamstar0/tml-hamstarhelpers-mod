using HamstarHelpers.ItemHelpers;
using HamstarHelpers.NPCHelpers;
using HamstarHelpers.Utilities.Messages;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
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

			DebugHelpers.DebugHelpers.PrintToBatch( sb );
			DebugHelpers.DebugHelpers.Once = false;
			DebugHelpers.DebugHelpers.OnceInAWhile--;
		}

		/*public override bool HijackSendData( int who_am_i, int msg_type, int remote_client, int ignore_client, NetworkText text, int number, float number2, float number3, float number4, int number5, int number6, int number7 ) {
			var modplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>();
			if( !modplayer.HasEnteredWorld ) {
				return false;
			}

			if( NPCSpawnInfoHelpers.IsSimulatingSpawns && msg_type == 23 ) {
				if( number >= 0 && number <= Main.npc.Length ) {
					NPC npc = Main.npc[number];

					if( npc != null && npc.active ) {
						NPCSpawnInfoHelpers.AddSpawn( npc.type );

						Main.npc[number] = new NPC();
						npc.active = false;
					}
				}
				return true;
			}
			return false;
		}*/


		public override void AddRecipeGroups() {
			IDictionary<int, int> banners = new Dictionary<int, int>();

			for( int npc_type = 0; npc_type < Main.npcTexture.Length; npc_type++ ) {
				int banner = Item.NPCtoBanner( npc_type );
				if( banner == 0 ) { continue; }

				banners[npc_type] = Item.BannerToItem( banner );
			}

			// Initialize banners
			NPCBannerHelpers.InitializeBanners( banners );

			string any = Lang.misc[37].ToString();
			RecipeGroup evil_boss_drops_grp = new RecipeGroup( () => any+" Evil Biome Boss Chunk", new int[] { ItemID.ShadowScale, ItemID.TissueSample } );
			RecipeGroup mirror_grp = new RecipeGroup( () => any+" Mirrors", new int[] { ItemID.MagicMirror, ItemID.IceMirror } );
			RecipeGroup banner_grp = new RecipeGroup( () => any+" Mob Banner", NPCBannerHelpers.GetBannerItemTypes().ToArray() );
			RecipeGroup musicbox_grp = new RecipeGroup( () => any+" Recorded Music Box", ItemMusicBoxHelpers.GetMusicBoxes().ToArray() );

			RecipeGroup.RegisterGroup( "HamstarHelpers:EvilBiomeBossDrops", evil_boss_drops_grp );
			RecipeGroup.RegisterGroup( "HamstarHelpers:MagicMirrors", mirror_grp );
			RecipeGroup.RegisterGroup( "HamstarHelpers:NpcBanners", banner_grp );
			RecipeGroup.RegisterGroup( "HamstarHelpers:RecordedMusicBoxes", musicbox_grp );
		}
	}
}
