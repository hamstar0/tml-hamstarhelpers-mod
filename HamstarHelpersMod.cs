using HamstarHelpers.MiscHelpers;
using HamstarHelpers.NPCHelpers;
using HamstarHelpers.Utilities.Messages;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
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
			ISet<int> banners = new HashSet<int>();
			ISet<int> musicboxes = new HashSet<int>();

			for( int npc_type = 0; npc_type < Main.npcTexture.Length; npc_type++ ) {
				int banner = Item.NPCtoBanner( npc_type );
				if( banner == 0 ) { continue; }

				banners.Add( Item.BannerToItem( banner ) );
			}

			musicboxes.Add( ItemID.MusicBoxAltOverworldDay );
			musicboxes.Add( ItemID.MusicBoxAltUnderground );
			musicboxes.Add( ItemID.MusicBoxBoss1 );
			musicboxes.Add( ItemID.MusicBoxBoss2 );
			musicboxes.Add( ItemID.MusicBoxBoss3 );
			musicboxes.Add( ItemID.MusicBoxBoss4 );
			musicboxes.Add( ItemID.MusicBoxBoss5 );
			musicboxes.Add( ItemID.MusicBoxCorruption );
			musicboxes.Add( ItemID.MusicBoxCrimson );
			musicboxes.Add( ItemID.MusicBoxDD2 );
			musicboxes.Add( ItemID.MusicBoxDesert );
			musicboxes.Add( ItemID.MusicBoxDungeon );
			musicboxes.Add( ItemID.MusicBoxEclipse );
			musicboxes.Add( ItemID.MusicBoxEerie );
			musicboxes.Add( ItemID.MusicBoxFrostMoon );
			musicboxes.Add( ItemID.MusicBoxGoblins );
			musicboxes.Add( ItemID.MusicBoxHell );
			musicboxes.Add( ItemID.MusicBoxIce );
			musicboxes.Add( ItemID.MusicBoxJungle );
			musicboxes.Add( ItemID.MusicBoxLunarBoss );
			musicboxes.Add( ItemID.MusicBoxMartians );
			musicboxes.Add( ItemID.MusicBoxMushrooms );
			musicboxes.Add( ItemID.MusicBoxNight );
			musicboxes.Add( ItemID.MusicBoxOcean );
			musicboxes.Add( ItemID.MusicBoxOverworldDay );
			musicboxes.Add( ItemID.MusicBoxPirates );
			musicboxes.Add( ItemID.MusicBoxPlantera );
			musicboxes.Add( ItemID.MusicBoxPumpkinMoon );
			musicboxes.Add( ItemID.MusicBoxRain );
			musicboxes.Add( ItemID.MusicBoxSandstorm );
			musicboxes.Add( ItemID.MusicBoxSnow );
			musicboxes.Add( ItemID.MusicBoxSpace );
			musicboxes.Add( ItemID.MusicBoxTemple );
			musicboxes.Add( ItemID.MusicBoxTheHallow );
			musicboxes.Add( ItemID.MusicBoxTitle );
			musicboxes.Add( ItemID.MusicBoxTowers );
			musicboxes.Add( ItemID.MusicBoxUnderground );
			musicboxes.Add( ItemID.MusicBoxUndergroundCorruption );
			musicboxes.Add( ItemID.MusicBoxUndergroundCrimson );
			musicboxes.Add( ItemID.MusicBoxUndergroundHallow );

			RecipeGroup banner_group = new RecipeGroup( () => Lang.misc[37] + " Mob Banner", banners.ToArray() );
			RecipeGroup musicbox_group = new RecipeGroup( () => Lang.misc[37] + " Recorded Music Box", musicboxes.ToArray() );

			RecipeGroup.RegisterGroup( "HamstarsHelpers:Banners", banner_group );
			RecipeGroup.RegisterGroup( "HamstarsHelpers:RecordedMusicBoxes", musicbox_group );
		}
	}
}
