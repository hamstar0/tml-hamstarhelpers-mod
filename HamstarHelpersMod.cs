using HamstarHelpers.ItemHelpers;
using HamstarHelpers.NetProtocol;
using HamstarHelpers.NPCHelpers;
using HamstarHelpers.TmlHelpers;
using HamstarHelpers.Utilities.Messages;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers {
	class HamstarHelpersMod : Mod {
		public bool HasRecipesBeenAdded { get; private set; }
		public bool HasSetupContent { get; private set; }
		public bool HasCurrentPlayerEnteredWorld { get; internal set; }


		////////////////

		public HamstarHelpersMod() {
			this.HasRecipesBeenAdded = false;
			this.HasSetupContent = false;
			this.HasCurrentPlayerEnteredWorld = false;

			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}

		public override void Load() {
			AltNPCInfo.DataInitialize();
			AltProjectileInfo.DataInitialize();

			/*var dict = new SortedDictionary<float, NPC>();
			for( int i = 0; i < Main.npcTexture.Length; i++ ) {
				NPC npc = new NPC();
				npc.SetDefaults( i );
				dict[NPCIdentityHelpers.LooselyAssessThreat( npc )] = npc;
			}
			foreach( var kv in dict ) {
				int digits = (int)Math.Ceiling( Math.Log10( kv.Value.type ) );
				string gap = new string( ' ', 6 - digits );
				ErrorLogger.Log( kv.Value.type + gap + " - " + kv.Key.ToString( "N2" ) + " = " + kv.Value.TypeName + "'s threat" );
			}*/
		}

		////////////////

		public override void PostAddRecipes() {
			this.HasRecipesBeenAdded = true;
		}

		public override void PostSetupContent() {
			this.HasSetupContent = true;
		}

		public override void PreSaveAndQuit() {
			var modworld = this.GetModWorld<MyModWorld>();

			this.HasCurrentPlayerEnteredWorld = false;
			modworld.HasCorrectID = false;
		}

		////////////////

		public override void PostDrawInterface( SpriteBatch sb ) {
			var modworld = this.GetModWorld<MyModWorld>();

			PlayerMessage.DrawPlayerLabels( sb );
			SimpleMessage.DrawMessage( sb );

			DebugHelpers.DebugHelpers.PrintToBatch( sb );
			DebugHelpers.DebugHelpers.Once = false;
			DebugHelpers.DebugHelpers.OnceInAWhile--;

			if( modworld.Logic != null ) {
				modworld.Logic.ReadyClient = true;  // Ugh!
			}
		}

		////////////////

		public override void HandlePacket( BinaryReader reader, int player_who ) {
			try {
				if( Main.netMode == 1 ) {
					ClientPacketHandlers.RoutePacket( this, reader );
				} else if( Main.netMode == 2 ) {
					ServerPacketHandlers.RoutePacket( this, reader, player_who );
				}
			} catch( Exception e ) {
				DebugHelpers.DebugHelpers.Log( "(Hamstar's Helpers) HandlePacket - " + e.ToString() );
			}
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

		////////////////
		
		public override void AddRecipeGroups() {
			IDictionary<int, int> npc_types_to_banner_item_types = new Dictionary<int, int>();

			for( int npc_type = 0; npc_type < Main.npcTexture.Length; npc_type++ ) {
				int banner_type = Item.NPCtoBanner( npc_type );
				if( banner_type == 0 ) { continue; }

				int banner_item_type = Item.BannerToItem( banner_type );
				if( banner_item_type >= Main.itemTexture.Length || banner_item_type <= 0 ) { continue; }

				npc_types_to_banner_item_types[npc_type] = banner_item_type;
			}

			// Initialize banners
			NPCBannerHelpers.InitializeBanners( npc_types_to_banner_item_types );

			string any = Lang.misc[37].ToString();
			RecipeGroup evil_boss_drops_grp = new RecipeGroup( () => any + " Evil Biome Boss Chunk", new int[] { ItemID.ShadowScale, ItemID.TissueSample } );
			RecipeGroup mirror_grp = new RecipeGroup( () => any + " Magic Mirrors", new int[] { ItemID.MagicMirror, ItemID.IceMirror } );
			RecipeGroup banner_grp = new RecipeGroup( () => any + " Mob Banner", NPCBannerHelpers.GetBannerItemTypes().ToArray() );
			RecipeGroup musicbox_grp = new RecipeGroup( () => any + " Recorded Music Box", ItemMusicBoxHelpers.GetMusicBoxes().ToArray() );

			RecipeGroup.RegisterGroup( "HamstarHelpers:EvilBiomeBossDrops", evil_boss_drops_grp );
			RecipeGroup.RegisterGroup( "HamstarHelpers:MagicMirrors", mirror_grp );
			RecipeGroup.RegisterGroup( "HamstarHelpers:NpcBanners", banner_grp );
			RecipeGroup.RegisterGroup( "HamstarHelpers:RecordedMusicBoxes", musicbox_grp );
		}
	}
}
