using HamstarHelpers.Utilities.AnimatedColor;
using HamstarHelpers.Utilities.Config;
using HamstarHelpers.Utilities.Messages;
using HamstarHelpers.Utilities.Network;
using HamstarHelpers.Utilities.Timers;
using HamstarHelpers.WebRequests;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers {
	partial class HamstarHelpersMod : Mod {
		private static void UnhandledLogger( object sender, UnhandledExceptionEventArgs e ) {
			DebugHelpers.LogHelpers.Log( "UNHANDLED crash? "+e.IsTerminating+" \nSender: "+sender.ToString()+" \nMessage: "+e.ExceptionObject.ToString() );
		}



		////////////////

		internal JsonConfig<HamstarHelpersConfigData> JsonConfig;
		public HamstarHelpersConfigData Config { get { return JsonConfig.Data; } }

		internal IDictionary<int, Type> PacketProtocols = new Dictionary<int, Type>();

		internal Timers Timers;
		internal DebugHelpers.LogHelpers LogHelpers;
		internal TmlHelpers.ModMetaDataManager ModMetaDataManager;
		internal NetHelpers.NetHelpers NetHelpers;
		internal BuffHelpers.BuffHelpers BuffHelpers;
		internal ItemHelpers.ItemIdentityHelpers ItemIdentityHelpers;
		internal NPCHelpers.NPCIdentityHelpers NPCIdentityHelpers;
		internal ProjectileHelpers.ProjectileIdentityHelpers ProjectileIdentityHelpers;
		internal BuffHelpers.BuffIdentityHelpers BuffIdentityHelpers;
		internal NPCHelpers.NPCBannerHelpers NPCBannerHelpers;
		internal RecipeHelpers.RecipeHelpers RecipeHelpers;
		internal TmlHelpers.TmlLoadHelpers TmlLoadHelpers;
		internal TmlHelpers.TmlPlayerHelpers TmlPlayerHelpers;
		internal WorldHelpers.WorldHelpers WorldHelpers;
		//internal UserHelpers.UserHelpers UserHelpers;
		internal TmlHelpers.ModHelpers.ModLockHelpers ModLockHelpers;
		internal AnimatedColorsManager AnimatedColors;
		internal PlayerMessages PlayerMessages;
		internal ModVersionGet ModVersionGet;
		internal ServerBrowserReport ServerBrowser;

		public bool HasSetupContent { get; private set; }
		public bool HasAddedRecipeGroups { get; private set; }
		public bool HasAddedRecipes { get; private set; }

		public ModHotKey ControlPanelHotkey = null;

		public ControlPanel.ControlPanelUI ControlPanel = null;
		 private int LastSeenScreenWidth = -1;
		 private int LastSeenScreenHeight = -1;


		private bool HasUnhandledExceptionLogger = false;



		////////////////

		public HamstarHelpersMod() {
			this.HasSetupContent = false;
			this.HasAddedRecipeGroups = false;
			this.HasAddedRecipes = false;

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

			if( !this.HasUnhandledExceptionLogger && this.Config.DebugModeUnhandledExceptionLogging ) {
				this.HasUnhandledExceptionLogger = true;
				AppDomain.CurrentDomain.UnhandledException += HamstarHelpersMod.UnhandledLogger;
			}
			
			this.TmlLoadHelpers = new TmlHelpers.TmlLoadHelpers();
			this.Timers = new Timers();
			this.LogHelpers = new DebugHelpers.LogHelpers();
			this.ModMetaDataManager = new TmlHelpers.ModMetaDataManager();
			this.BuffHelpers = new BuffHelpers.BuffHelpers();
			this.NetHelpers = new NetHelpers.NetHelpers();
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
			this.ModVersionGet = new ModVersionGet();
			this.ServerBrowser = new ServerBrowserReport();

			this.Timers.Begin();

			TmlHelpers.AltNPCInfo.DataInitialize();
			TmlHelpers.AltProjectileInfo.DataInitialize();

			if( !this.Config.DisableControlPanelHotkey ) {
				this.ControlPanelHotkey = this.RegisterHotKey( "Hamstar's Helper Control Panel", "O" );
			}
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
			if( this.HasUnhandledExceptionLogger ) {
				this.HasUnhandledExceptionLogger = false;
				AppDomain.CurrentDomain.UnhandledException -= HamstarHelpersMod.UnhandledLogger;
			}

			this.Timers.End();
			this.TmlLoadHelpers.Unload();

			this.Timers = null;
			this.JsonConfig = null;
			this.PacketProtocols = null;
			this.LogHelpers = null;
			this.ModMetaDataManager = null;
			this.BuffHelpers = null;
			this.NetHelpers = null;
			this.ItemIdentityHelpers = null;
			this.NPCIdentityHelpers = null;
			this.ProjectileIdentityHelpers = null;
			this.BuffIdentityHelpers = null;
			this.NPCBannerHelpers = null;
			this.RecipeHelpers = null;
			this.TmlPlayerHelpers = null;
			this.TmlLoadHelpers = null;
			this.ModVersionGet = null;
			this.WorldHelpers = null;
			this.ModLockHelpers = null;
			this.AnimatedColors = null;
			this.PlayerMessages = null;
			this.ControlPanelHotkey = null;
			this.ControlPanel = null;
			this.ServerBrowser = null;

			HamstarHelpersMod.Instance = null;
		}

		////////////////

		public override void PostSetupContent() {
			this.PacketProtocols = PacketProtocol.GetProtocols();

			//this.ItemIdentityHelpers.PopulateNames();
			//this.NPCIdentityHelpers.PopulateNames();
			//this.ProjectileIdentityHelpers.PopulateNames();
			//this.BuffIdentityHelpers.PopulateNames();

			this.ModMetaDataManager.OnPostSetupContent();

			if( !Main.dedServ ) {
				HamstarHelpers.ControlPanel.ControlPanelUI.OnPostSetupContent( this );
			}

			this.HasSetupContent = true;
			this.CheckAndProcessLoadFinish();
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

			this.HasAddedRecipeGroups = true;
			this.CheckAndProcessLoadFinish();
		}

		public override void PostAddRecipes() {
			this.ItemIdentityHelpers.PopulateNames();
			this.NPCIdentityHelpers.PopulateNames();
			this.ProjectileIdentityHelpers.PopulateNames();
			this.BuffIdentityHelpers.PopulateNames();
			
			this.HasAddedRecipes = true;
			this.CheckAndProcessLoadFinish();
		}


		////////////////

		private void CheckAndProcessLoadFinish() {
			if( !this.HasSetupContent ) { return; }
			if( !this.HasAddedRecipeGroups ) { return; }
			if( !this.HasAddedRecipes ) { return; }

			this.TmlLoadHelpers.FulfillPostModLoadPromises();
		}
	}
}
