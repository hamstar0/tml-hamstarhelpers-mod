using HamstarHelpers.ControlPanel;
using HamstarHelpers.NetProtocol;
using HamstarHelpers.NPCHelpers;
using HamstarHelpers.TmlHelpers;
using HamstarHelpers.Utilities.Config;
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

		public static string ConfigFileRelativePath {
			get { return JsonConfig<HamstarHelpersConfigData>.RelativePath + Path.DirectorySeparatorChar + HamstarHelpersConfigData.ConfigFileName; }
		}
		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reload configs outside of single player." );
			}
			if( HamstarHelpersMod.Instance != null ) {
				if( !HamstarHelpersMod.Instance.JsonConfig.LoadFile() ) {
					HamstarHelpersMod.Instance.JsonConfig.SaveFile();
				}
			}
		}


		////////////////

		internal JsonConfig<HamstarHelpersConfigData> JsonConfig;
		public HamstarHelpersConfigData Config { get { return JsonConfig.Data; } }

		internal DebugHelpers.LogHelpers LogHelpers;
		internal TmlHelpers.ModMetaDataManager ModMetaDataManager;
		internal BuffHelpers.BuffHelpers BuffHelpers;
		internal ItemHelpers.ItemIdentityHelpers ItemIdentityHelpers;
		internal NPCHelpers.NPCBannerHelpers NPCBannerHelpers;
		internal RecipeHelpers.RecipeHelpers RecipeHelpers;
		internal TmlHelpers.TmlPlayerHelpers TmlPlayerHelpers;
		internal WorldHelpers.WorldHelpers WorldHelpers;

		public bool HasRecipesBeenAdded { get; private set; }
		public bool HasSetupContent { get; private set; }

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

			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};

			this.JsonConfig = new JsonConfig<HamstarHelpersConfigData>( HamstarHelpersConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath, new HamstarHelpersConfigData() );
		}

		public override void Load() {
			HamstarHelpersMod.Instance = this;

			this.LogHelpers = new DebugHelpers.LogHelpers();
			this.ModMetaDataManager = new TmlHelpers.ModMetaDataManager();
			this.BuffHelpers = new BuffHelpers.BuffHelpers();
			this.ItemIdentityHelpers = new ItemHelpers.ItemIdentityHelpers();
			this.NPCBannerHelpers = new NPCHelpers.NPCBannerHelpers();
			this.RecipeHelpers = new RecipeHelpers.RecipeHelpers();
			this.TmlPlayerHelpers = new TmlHelpers.TmlPlayerHelpers();
			this.WorldHelpers = new WorldHelpers.WorldHelpers();
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

			this.LoadConfigs();
		}

		private void LoadConfigs() {
			if( !this.JsonConfig.LoadFile() ) {
				this.JsonConfig.SaveFile();
			}

			if( this.Config.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Hamstar's Helpers updated to " + HamstarHelpersConfigData.ConfigVersion.ToString() );
				this.JsonConfig.SaveFile();
			}
		}

		public override void Unload() {
			HamstarHelpersMod.Instance = null;
		}

		////////////////

		public override void PostAddRecipes() {
			this.HasRecipesBeenAdded = true;
		}

		public override void PostSetupContent() {
			this.BuffHelpers.Initialize();
			this.ItemIdentityHelpers.Initialize();
			this.ModMetaDataManager.Initialize();

			if( !Main.dedServ ) {
				ControlPanelUI.Load( (HamstarHelpersMod)this );
			}

			this.HasSetupContent = true;
		}

		////////////////

		public override void PreSaveAndQuit() {
			var modworld = this.GetModWorld<HamstarHelpersWorld>();
			
			modworld.HasCorrectID = false;
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
				DebugHelpers.LogHelpers.Log( "(Hamstar's Helpers) HandlePacket - " + e.ToString() );
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
			if( this.Config.AddCrimsonLeatherRecipe ) {
				var vertebrae_to_leather = new ModRecipe( this );
				vertebrae_to_leather.AddIngredient( ItemID.Vertebrae, 5 );
				vertebrae_to_leather.SetResult( ItemID.Leather );
				vertebrae_to_leather.AddRecipe();
			}
		}

		public override void AddRecipeGroups() {
			NPCBannerHelpers.InitializeBanners();

			foreach( var kv in HamstarHelpers.RecipeHelpers.RecipeHelpers.GetRecipeGroups() ) {
				RecipeGroup.RegisterGroup( kv.Key, kv.Value );
			}
		}


		////////////////

		public override void PostDrawInterface( SpriteBatch sb ) {
			var modworld = this.GetModWorld<HamstarHelpersWorld>();

			PlayerMessage.DrawPlayerLabels( sb );
			SimpleMessage.DrawMessage( sb );

			DebugHelpers.DebugHelpers.PrintToBatch( sb );
			DebugHelpers.DebugHelpers.Once = false;
			DebugHelpers.DebugHelpers.OnceInAWhile--;

			if( modworld.Logic != null ) {
				modworld.Logic.IsClientPlaying = true;  // Ugh!
			}
		}


		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			var modworld = this.GetModWorld<HamstarHelpersWorld>();

			if( modworld.Logic.IsPlaying() ) {
				int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Mouse Text" ) );
				if( idx != -1 ) {
					GameInterfaceDrawMethod draw_method = delegate {
						try {
							if( this.LastSeenScreenWidth != Main.screenWidth || this.LastSeenScreenHeight != Main.screenHeight ) {
								this.LastSeenScreenWidth = Main.screenWidth;
								this.LastSeenScreenHeight = Main.screenHeight;
								this.ControlPanel.Recalculate();
							}

							this.ControlPanel.UpdateInteractivity( Main._drawInterfaceGameTime );
							this.ControlPanel.UpdateDialog();

							this.ControlPanel.Draw( Main.spriteBatch );

							if( !this.Config.DisableControlPanel ) {
								this.ControlPanel.UpdateToggler();
								this.ControlPanel.DrawToggler( Main.spriteBatch );
							}
						} catch( Exception e ) { Main.NewText(e.Message); }
						
						return true;
					};

					var interface_layer = new LegacyGameInterfaceLayer( "HamstarHelpers: Control Panel",
						draw_method, InterfaceScaleType.UI );

					layers.Insert( idx, interface_layer );
				}
			}
		}
	}
}
