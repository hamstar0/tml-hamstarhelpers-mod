using HamstarHelpers.Components.Config;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Internals.ControlPanel.Inbox;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Internals.ControlPanel;
using HamstarHelpers.Services.AnimatedColor;
using HamstarHelpers.Services.EntityGroups;
using HamstarHelpers.Services.Messages;
using HamstarHelpers.Services.Timers;
using HamstarHelpers.Services.Promises;
using HamstarHelpers.Services.DataStore;
using HamstarHelpers.Helpers.MiscHelpers;
using HamstarHelpers.Helpers.TmlHelpers.ModHelpers;
using HamstarHelpers.Helpers.WorldHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.NetHelpers;
using HamstarHelpers.Helpers.BuffHelpers;
using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Helpers.NPCHelpers;
using HamstarHelpers.Helpers.ProjectileHelpers;
using HamstarHelpers.Helpers.RecipeHelpers;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Services.DataDumper;
using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.Services.CustomHotkeys;
using HamstarHelpers.Internals.Menus;

namespace HamstarHelpers {
	partial class ModHelpersMod : Mod {
		private static void UnhandledLogger( object sender, UnhandledExceptionEventArgs e ) {
			LogHelpers.Log( "UNHANDLED crash? "+e.IsTerminating+" \nSender: "+sender.ToString()+" \nMessage: "+e.ExceptionObject.ToString() );
		}



		////////////////

		internal JsonConfig<HamstarHelpersConfigData> ConfigJson;
		public HamstarHelpersConfigData Config { get { return ConfigJson.Data; } }
		
		// Components
		internal HamstarExceptionManager ExceptionMngr;
		internal MenuUIManager MenuUIMngr;
		internal MenuItemManager MenuItemMngr;
		internal CustomEntityManager CustomEntMngr;
		internal IDictionary<int, Type> PacketProtocols = new Dictionary<int, Type>();
		internal PacketProtocolDataConstructorLock PacketProtocolCtorLock = new PacketProtocolDataConstructorLock();

		// Services
		internal Promises Promises;
		internal Timers Timers;
		internal EntityGroups EntityGroups;
		internal AnimatedColorsManager AnimatedColors;
		internal PlayerMessages PlayerMessages;
		internal DataStore DataStore;
		internal CustomHotkeys CustomHotkeys;

		// Helpers
		internal LogHelpers LogHelpers;
		internal ModMetaDataManager ModMetaDataManager;
		internal NetHelpers NetHelpers;
		internal BuffHelpers BuffHelpers;
		internal ItemIdentityHelpers ItemIdentityHelpers;
		internal NPCIdentityHelpers NPCIdentityHelpers;
		internal ProjectileIdentityHelpers ProjectileIdentityHelpers;
		internal BuffIdentityHelpers BuffIdentityHelpers;
		internal NPCBannerHelpers NPCBannerHelpers;
		internal RecipeHelpers RecipeHelpers;
		internal LoadHelpers LoadHelpers;
		internal TmlPlayerHelpers TmlPlayerHelpers;
		internal WorldHelpers WorldHelpers;
		internal ModLockHelpers ModLockHelpers;
		internal MusicHelpers MusicHelpers;

		// Internals
		internal InboxControl Inbox;
		internal ModVersionGet ModVersionGet;
		internal ServerBrowserReporter ServerBrowser;
		internal UIControlPanel ControlPanel;


		public bool HasSetupContent { get; private set; }
		public bool HasAddedRecipeGroups { get; private set; }
		public bool HasAddedRecipes { get; private set; }

		public ModHotKey ControlPanelHotkey = null;
		public ModHotKey DataDumpHotkey = null;

		private int LastSeenCPScreenWidth = -1;
		private int LastSeenCPScreenHeight = -1;


		private bool HasUnhandledExceptionLogger = false;



		////////////////

		public ModHelpersMod() {
			this.HasSetupContent = false;
			this.HasAddedRecipeGroups = false;
			this.HasAddedRecipes = false;
			
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};

			this.ExceptionMngr = new HamstarExceptionManager();
			this.AnimatedColors = new AnimatedColorsManager();
			this.ConfigJson = new JsonConfig<HamstarHelpersConfigData>( HamstarHelpersConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath, new HamstarHelpersConfigData() );
		}

		public override void Load() {
			ModHelpersMod.Instance = this;

			this.LoadConfigs();

			if( !this.HasUnhandledExceptionLogger && this.Config.DebugModeUnhandledExceptionLogging ) {
				this.HasUnhandledExceptionLogger = true;
				AppDomain.CurrentDomain.UnhandledException += ModHelpersMod.UnhandledLogger;
			}
			
			this.DataStore = new DataStore();
			this.Promises = new Promises();
			this.LoadHelpers = new LoadHelpers();

			this.Timers = new Timers();
			this.LogHelpers = new LogHelpers();
			this.ModMetaDataManager = new ModMetaDataManager();
			this.BuffHelpers = new BuffHelpers();
			this.NetHelpers = new NetHelpers();
			this.ItemIdentityHelpers = new ItemIdentityHelpers();
			this.NPCIdentityHelpers = new NPCIdentityHelpers();
			this.ProjectileIdentityHelpers = new ProjectileIdentityHelpers();
			this.BuffIdentityHelpers = new BuffIdentityHelpers();
			this.NPCBannerHelpers = new NPCBannerHelpers();
			this.RecipeHelpers = new RecipeHelpers();
			this.TmlPlayerHelpers = new TmlPlayerHelpers();
			this.WorldHelpers = new WorldHelpers();
			this.ControlPanel = new UIControlPanel();
			this.ModLockHelpers = new ModLockHelpers();
			this.EntityGroups = new EntityGroups();
			this.PlayerMessages = new PlayerMessages();
			this.Inbox = new InboxControl();
			this.ModVersionGet = new ModVersionGet();
			this.ServerBrowser = new ServerBrowserReporter();
			this.MenuItemMngr = new MenuItemManager();
			this.MenuUIMngr = new MenuUIManager();
			this.MusicHelpers = new MusicHelpers();
			this.CustomEntMngr = new CustomEntityManager();
			this.CustomHotkeys = new CustomHotkeys();

			if( !this.Config.DisableControlPanelHotkey ) {
				this.ControlPanelHotkey = this.RegisterHotKey( "Toggle Control Panel", "O" );
			}
			this.DataDumpHotkey = this.RegisterHotKey( "Dump Debug Data", "P" );

			this.LoadModData();

			DataDumper.SetDumpSource( "WorldUidWithSeed", () => {
				return "  "+WorldHelpers.GetUniqueIdWithSeed();
			} );

			DataDumper.SetDumpSource( "PlayerUid", () => {
				if( Main.myPlayer < 0 || Main.myPlayer >= Main.player.Length ) {
					return "  Unobtainable";
				}

				bool success;
				string uid = PlayerIdentityHelpers.GetUniqueId( Main.LocalPlayer, out success );
				if( !success ) {
					return "  UID unobtainable";
				}

				return "  " + uid;
			} );
		}


		private void LoadConfigs() {
			if( !this.ConfigJson.LoadFile() ) {
				this.ConfigJson.SaveFile();
			}

			if( this.Config.UpdateToLatestVersion( this ) ) {
				ErrorLogger.Log( "Mod Helpers updated to " + this.Version.ToString() );
				this.ConfigJson.SaveFile();
			}
		}

		public override void Unload() {
			this.UnloadModData();

			this.Promises.FulfillModUnloadPromises();

			try {
				if( this.HasUnhandledExceptionLogger ) {
					this.HasUnhandledExceptionLogger = false;
					AppDomain.CurrentDomain.UnhandledException -= ModHelpersMod.UnhandledLogger;
				}
			} catch { }

			this.ExceptionMngr = null;
			this.Timers = null;
			this.ConfigJson = null;
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
			this.LoadHelpers = null;
			this.ModVersionGet = null;
			this.WorldHelpers = null;
			this.ModLockHelpers = null;
			this.EntityGroups = null;
			this.AnimatedColors = null;
			this.PlayerMessages = null;
			this.Inbox = null;
			this.ControlPanel = null;
			this.ServerBrowser = null;
			this.MenuItemMngr = null;
			this.MenuUIMngr = null;
			this.MusicHelpers = null;
			this.CustomEntMngr = null;
			this.Promises = null;
			this.DataStore = null;
			this.CustomHotkeys = null;

			this.ControlPanelHotkey = null;
			this.DataDumpHotkey = null;

			ModHelpersMod.Instance = null;
		}

		////////////////

		public override void PostSetupContent() {
			this.PacketProtocols = PacketProtocol.GetProtocolTypes();

			this.Promises.OnPostSetupContent();
			this.ModMetaDataManager.OnPostSetupContent();
			this.ModVersionGet.OnPostSetupContent();

			if( !Main.dedServ ) {
				Menus.OnPostSetupContent();
				UIControlPanel.OnPostSetupContent( this );
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

			foreach( var kv in RecipeHelpers.Groups ) {
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

			Promises.AddWorldUnloadEachPromise( () => {
				this.OnWorldExit();
			} );

			this.Promises.FulfillPostModLoadPromises();
		}


		////////////////

		public override void PreSaveAndQuit() {
			this.Promises.PreSaveAndExit();
		}


		////////////////
		
		private void OnWorldExit() {
			var myworld = this.GetModWorld<ModHelpersWorld>();
			myworld.OnWorldExit();
		}
	}
}
