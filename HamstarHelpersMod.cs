using HamstarHelpers.ControlPanel;
using HamstarHelpers.NPCHelpers;
using HamstarHelpers.TmlHelpers;
using HamstarHelpers.Utilities.AnimatedColor;
using HamstarHelpers.Utilities.Config;
using HamstarHelpers.Utilities.Messages;
using HamstarHelpers.Utilities.Network;
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

		internal IDictionary<int, Type> PacketProtocols = new Dictionary<int, Type>();

		internal DebugHelpers.LogHelpers LogHelpers;
		internal TmlHelpers.ModMetaDataManager ModMetaDataManager;
		internal BuffHelpers.BuffHelpers BuffHelpers;
		internal ItemHelpers.ItemIdentityHelpers ItemIdentityHelpers;
		internal NPCHelpers.NPCIdentityHelpers NPCIdentityHelpers;
		internal ProjectileHelpers.ProjectileIdentityHelpers ProjectileIdentityHelpers;
		internal BuffHelpers.BuffIdentityHelpers BuffIdentityHelpers;
		internal NPCHelpers.NPCBannerHelpers NPCBannerHelpers;
		internal RecipeHelpers.RecipeHelpers RecipeHelpers;
		internal TmlHelpers.TmlPlayerHelpers TmlPlayerHelpers;
		internal WorldHelpers.WorldHelpers WorldHelpers;
		//internal UserHelpers.UserHelpers UserHelpers;
		internal TmlHelpers.ModHelpers.ModLockHelpers ModLockHelpers;
		internal AnimatedColorsManager AnimatedColors;
		internal PlayerMessages PlayerMessages;
		
		public bool HasRecipesBeenAdded { get; private set; }
		public bool HasSetupContent { get; private set; }

		public ModHotKey ControlPanelHotkey = null;

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

			this.AnimatedColors = new AnimatedColorsManager();
			this.JsonConfig = new JsonConfig<HamstarHelpersConfigData>( HamstarHelpersConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath, new HamstarHelpersConfigData() );
		}

		public override void Load() {
			HamstarHelpersMod.Instance = this;

			this.LoadConfigs();
			
			this.LogHelpers = new DebugHelpers.LogHelpers();
			this.ModMetaDataManager = new TmlHelpers.ModMetaDataManager();
			this.BuffHelpers = new BuffHelpers.BuffHelpers();
			this.ItemIdentityHelpers = new ItemHelpers.ItemIdentityHelpers();
			this.NPCIdentityHelpers = new NPCHelpers.NPCIdentityHelpers();
			this.ProjectileIdentityHelpers = new ProjectileHelpers.ProjectileIdentityHelpers();
			this.BuffIdentityHelpers = new BuffHelpers.BuffIdentityHelpers();
			this.NPCBannerHelpers = new NPCHelpers.NPCBannerHelpers();
			this.RecipeHelpers = new RecipeHelpers.RecipeHelpers();
			this.TmlPlayerHelpers = new TmlHelpers.TmlPlayerHelpers();
			this.WorldHelpers = new WorldHelpers.WorldHelpers();
			this.ControlPanel = new ControlPanel.ControlPanelUI();
			//this.UserHelpers = new UserHelpers.UserHelpers();
			this.ModLockHelpers = new TmlHelpers.ModHelpers.ModLockHelpers();
			this.PlayerMessages = new PlayerMessages();

			AltNPCInfo.DataInitialize();
			AltProjectileInfo.DataInitialize();

			this.ControlPanelHotkey = this.RegisterHotKey( "Hamstar's Helper Control Panel", "O" );
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
			this.PacketProtocols = PacketProtocol.GetProtocols();

			this.BuffHelpers.OnPostSetupContent();
			this.ItemIdentityHelpers.OnPostSetupContent();
			this.NPCIdentityHelpers.OnPostSetupContent();
			this.ProjectileIdentityHelpers.OnPostSetupContent();
			this.BuffIdentityHelpers.OnPostSetupContent();
			this.ModMetaDataManager.OnPostSetupContent();

			if( !Main.dedServ ) {
				ControlPanelUI.OnPostSetupContent( this );
			}

			this.HasSetupContent = true;
		}

		////////////////

		public override void PreSaveAndQuit() {
			var modworld = this.GetModWorld<HamstarHelpersWorld>();

			this.LogHelpers.OnWorldExit();
			this.ModLockHelpers.OnWorldExit();
			modworld.OnWorldExit();
		}


		////////////////

		public override void HandlePacket( BinaryReader reader, int player_who ) {
			try {
				PacketProtocol.HandlePacket( reader, player_who );
			} catch( Exception e ) {
				DebugHelpers.LogHelpers.Log( "(Hamstar's Helpers) HandlePacket - " + e.ToString() );
			}
		}


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

			if( modworld.WorldLogic != null ) {
				modworld.WorldLogic.IsClientPlaying = true;  // Ugh!
			}
		}


		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			var modworld = this.GetModWorld<HamstarHelpersWorld>();
			if( !modworld.WorldLogic.IsPlaying() ) { return; }

			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Mouse Text" ) );
			if( idx == -1 ) { return; }

			GameInterfaceDrawMethod debug_layer_draw = delegate {
				var sb = Main.spriteBatch;

				this.PlayerMessages.Draw( sb );
				SimpleMessage.DrawMessage( sb );

				DebugHelpers.DebugHelpers.PrintToBatch( sb );
				DebugHelpers.DebugHelpers.Once = false;
				DebugHelpers.DebugHelpers.OnceInAWhile--;
				return true;
			};

			GameInterfaceDrawMethod cp_layer_draw = delegate {
				var sb = Main.spriteBatch;
				
				if( !this.Config.DisableControlPanel ) {
					this.ControlPanel.UpdateToggler();
					this.ControlPanel.DrawToggler( sb );
				}
				if( this.LastSeenScreenWidth != Main.screenWidth || this.LastSeenScreenHeight != Main.screenHeight ) {
					this.LastSeenScreenWidth = Main.screenWidth;
					this.LastSeenScreenHeight = Main.screenHeight;
					this.ControlPanel.RecalculateMe();
				}
//sb.DrawString( Main.fontDeathText, "ALERT", new Vector2(128, 128), this.AnimatedColors.Alert.CurrentColor );
//sb.DrawString( Main.fontDeathText, "STROBE", new Vector2(128, 256), this.AnimatedColors.Strobe.CurrentColor );
//sb.DrawString( Main.fontDeathText, "FIRE", new Vector2(128, 320), this.AnimatedColors.Fire.CurrentColor );
//sb.DrawString( Main.fontDeathText, "WATER", new Vector2(128, 384), this.AnimatedColors.Water.CurrentColor );
//sb.DrawString( Main.fontDeathText, "AIR", new Vector2(128, 448), this.AnimatedColors.Air.CurrentColor );
				return true;
			};

			GameInterfaceDrawMethod modlock_layer_draw = delegate {
				this.ModLockHelpers.DrawWarning( Main.spriteBatch );
				return true;
			};


			var debug_layer = new LegacyGameInterfaceLayer( "HamstarHelpers: Debug Display",
				debug_layer_draw, InterfaceScaleType.UI );
			layers.Insert( idx, debug_layer );

			var modlock_layer = new LegacyGameInterfaceLayer( "HamstarHelpers: Mod Lock",
				modlock_layer_draw, InterfaceScaleType.UI );
			layers.Insert( idx, modlock_layer );

			if( !this.Config.DisableControlPanel ) {
				var cp_layer = new LegacyGameInterfaceLayer( "HamstarHelpers: Control Panel",
					cp_layer_draw, InterfaceScaleType.UI );
				layers.Insert( idx, cp_layer );
			}
		}
	}
}
