using HamstarHelpers.ControlPanel;
using HamstarHelpers.NetProtocol;
using HamstarHelpers.NPCHelpers;
using HamstarHelpers.TmlHelpers;
using HamstarHelpers.Utilities.Messages;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;


namespace HamstarHelpers {
	class HamstarHelpersMod : Mod {
		public static HamstarHelpersMod Instance { get; private set; }

		public static string GithubUserName { get { return "hamstar0"; } }
		public static string GithubProjectName { get { return "tml-hamstarhelpers-mod"; } }


		////////////////

		public bool HasRecipesBeenAdded { get; private set; }
		public bool HasSetupContent { get; private set; }
		public bool HasCurrentPlayerEnteredWorld { get; internal set; }

		public ControlPanelUI ControlPanel = null;
		private int LastSeenScreenWidth = -1;
		private int LastSeenScreenHeight = -1;

		////////////////

		//internal ModEvents ModEvents = new ModEvents();
		//internal WorldEvents WorldEvents = new WorldEvents();
		//internal PlayerEvents PlayerEvents = new PlayerEvents();
		//internal ItemEvents ItemEvents = new ItemEvents();
		//internal NPCEvents NPCEvents = new NPCEvents();
		//internal ProjectileEvents ProjectileEvents = new ProjectileEvents();
		//internal TileEvents TileEvents = new TileEvents();



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
			HamstarHelpersMod.Instance = this;

			//this.ModEvents.OnLoad();

			this.ControlPanel = new ControlPanelUI();
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

		public override void Unload() {
			//this.ModEvents.OnUnload();

			HamstarHelpersMod.Instance = null;

			TmlPlayerHelpers.Reset();
			_ModMetaDataManagerLoader.Unload();
		}

		////////////////

		public override void PostAddRecipes() {
			this.HasRecipesBeenAdded = true;
		}

		public override void PostSetupContent() {
			//this.ModEvents.OnPostSetupContent();
			BuffHelpers.BuffHelpers.Initialize();
			_ModMetaDataManagerLoader.Load();

			if( !Main.dedServ ) {
				ControlPanelUI.Load( (HamstarHelpersMod)this );
			}

			this.HasSetupContent = true;
		}

		////////////////

		public override void PreSaveAndQuit() {
			//this.ModEvents.OnPreSaveAndQuit();

			var modworld = this.GetModWorld<HamstarHelpersWorld>();

			this.HasCurrentPlayerEnteredWorld = false;
			modworld.HasCorrectID = false;
		}


		////////////////

		public override void HandlePacket( BinaryReader reader, int player_who ) {
			//this.ModEvents.OnHandlePacket( reader, ref player_who );

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

		public override void AddRecipes() {
			var vertebrae_to_leather = new ModRecipe( this );
			vertebrae_to_leather.AddIngredient( ItemID.Vertebrae, 5 );
			vertebrae_to_leather.SetResult( ItemID.Leather );
			vertebrae_to_leather.AddRecipe();

			/*var vertebrae_to_chunk = new ModRecipe( this );
			vertebrae_to_chunk.AddIngredient( ItemID.Vertebrae, 1 );
			vertebrae_to_chunk.SetResult( ItemID.RottenChunk );
			vertebrae_to_chunk.AddRecipe();

			var chunk_to_vertebrae = new ModRecipe( this );
			chunk_to_vertebrae.AddIngredient( ItemID.RottenChunk, 1 );
			chunk_to_vertebrae.SetResult( ItemID.Vertebrae );
			chunk_to_vertebrae.AddRecipe();*/
		}

		public override void AddRecipeGroups() {
			//this.ModEvents.OnAddRecipeGroups();
			NPCBannerHelpers.InitializeBanners();

			foreach( var kv in RecipeHelpers.RecipeHelpers.GetRecipeGroups() ) {
				RecipeGroup.RegisterGroup( kv.Key, kv.Value );
			}
		}


		////////////////

		public override void PostDrawInterface( SpriteBatch sb ) {
			//this.ModEvents.OnPostDrawInterface( sb );

			var modworld = this.GetModWorld<HamstarHelpersWorld>();

			PlayerMessage.DrawPlayerLabels( sb );
			SimpleMessage.DrawMessage( sb );

			DebugHelpers.DebugHelpers.PrintToBatch( sb );
			DebugHelpers.DebugHelpers.Once = false;
			DebugHelpers.DebugHelpers.OnceInAWhile--;

			if( modworld.Logic != null ) {
				modworld.Logic.ReadyClient = true;  // Ugh!
			}
		}


		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			//this.ModEvents.OnModifyInterfaceLayers( layers );

			var modworld = this.GetModWorld<HamstarHelpersWorld>();

			if( modworld.Logic.IsReady() ) {
				int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Mouse Text" ) );
				if( idx != -1 ) {
					GameInterfaceDrawMethod draw_method = delegate {
						if( this.LastSeenScreenWidth != Main.screenWidth || this.LastSeenScreenHeight != Main.screenHeight ) {
							this.LastSeenScreenWidth = Main.screenWidth;
							this.LastSeenScreenHeight = Main.screenHeight;
							this.ControlPanel.RecalculateBackend();
						}
						
						this.ControlPanel.UpdateInteractivity( Main._drawInterfaceGameTime );
						this.ControlPanel.UpdateDialog();
						this.ControlPanel.UpdateToggler();

						this.ControlPanel.Draw( Main.spriteBatch );
						this.ControlPanel.DrawToggler( Main.spriteBatch );
						
						return true;
					};

					var interface_layer = new LegacyGameInterfaceLayer( "HamstarHelpers: Control Panel",
						draw_method, InterfaceScaleType.UI );

					layers.Insert( idx, interface_layer );
				}
			}
		}


		////////////////

		/*public override void AddRecipes() {
			this.ModEvents.OnAddRecipes();
			base.AddRecipes();
		}
		public override object Call( params object[] args ) {
			this.ModEvents.OnCall( args );
			return base.Call( args );
		}
		public override bool HijackGetData( ref byte messageType, ref BinaryReader reader, int playerNumber ) {
			this.ModEvents.OnHijackGetData( ref messageType, ref reader, playerNumber );
			return base.HijackGetData( ref messageType, ref reader, playerNumber );
		}
		public override bool HijackSendData( int whoAmI, int msgType, int remoteClient, int ignoreClient, NetworkText text, int number, float number2, float number3, float number4, int number5, int number6, int number7 ) {
			this.ModEvents.OnHijackSendData( ref whoAmI, ref msgType, ref remoteClient, ref ignoreClient, text, ref number, ref number2, ref number3, ref number4, ref number5, ref number6, ref number7 );
			return base.HijackSendData( whoAmI, msgType, remoteClient, ignoreClient, text, number, number2, number3, number4, number5, number6, number7 );
		}
		public override void HotKeyPressed( string name ) {
			this.ModEvents.OnHotKeyPressed( name );
			base.HotKeyPressed( name );
		}
		public override void ModifyLightingBrightness( ref float scale ) {
			this.ModEvents.OnModifyLightingBrightness( ref scale );
			base.ModifyLightingBrightness( ref scale );
		}
		public override void ModifySunLightColor( ref Color tileColor, ref Color backgroundColor ) {
			this.ModEvents.OnModifySunLightColor( ref tileColor, ref backgroundColor );
			base.ModifySunLightColor( ref tileColor, ref backgroundColor );
		}
		public override Matrix ModifyTransformMatrix( Matrix transform ) {
			this.ModEvents.OnModifyTransformMatrix( ref transform );
			return base.ModifyTransformMatrix( transform );
		}
		public override void PostDrawFullscreenMap( ref string mouseText ) {
			base.PostDrawFullscreenMap( ref mouseText );
		}
		public override void PostUpdateInput() {
			base.PostUpdateInput();
		}
		public override void UpdateMusic( ref int music ) {
			base.UpdateMusic( ref music );
		}*/
	}
}
