using HamstarHelpers.Internals.ControlPanel.Inbox;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Internals.ControlPanel;
using HamstarHelpers.Internals.Menus;
using HamstarHelpers.Internals.Menus.Support;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Services.AnimatedColor;
using HamstarHelpers.Services.EntityGroups;
using HamstarHelpers.Services.Messages;
using HamstarHelpers.Services.Timers;
using HamstarHelpers.Services.Promises;
using HamstarHelpers.Services.DataStore;
using HamstarHelpers.Services.Menus;
using HamstarHelpers.Services.ServerInfo;
using HamstarHelpers.Services.CustomHotkeys;
using HamstarHelpers.Services.Players;
using HamstarHelpers.Helpers.MiscHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using HamstarHelpers.Helpers.TmlHelpers.ModHelpers;
using HamstarHelpers.Helpers.WorldHelpers;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.NetHelpers;
using HamstarHelpers.Helpers.BuffHelpers;
using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Helpers.NPCHelpers;
using HamstarHelpers.Helpers.ProjectileHelpers;
using HamstarHelpers.Helpers.RecipeHelpers;
using HamstarHelpers.Helpers.XnaHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers {
	partial class ModHelpersMod : Mod {
		// Components
		internal HamstarExceptionManager ExceptionMngr;
		internal MenuContextServiceManager MenuContextMngr;
		internal MenuItemManager MenuItemMngr;
		internal CustomEntityManager CustomEntMngr;
		internal IDictionary<int, Type> PacketProtocols = new Dictionary<int, Type>();

		// Services
		internal Promises Promises;
		internal Timers Timers;
		internal EntityGroups EntityGroups;
		internal AnimatedColorsManager AnimatedColors;
		internal PlayerMessages PlayerMessages;
		internal DataStore DataStore;
		internal CustomHotkeys CustomHotkeys;
		internal ServerInfo ServerInfo;
		//internal PlayerDataManager PlayerDataMngr;

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
		internal PlayerState PlayerState;
		internal WorldStateHelpers WorldStateHelpers;
		internal ModLockHelpers ModLockHelpers;
		internal MusicHelpers MusicHelpers;
		internal PlayerIdentityHelpers PlayerIdentityHelpers;

		// Internals
		internal InboxControl Inbox;
		internal GetModTags GetModTags;
		internal GetModVersion GetModVersion;
		internal UIControlPanel ControlPanel;
		internal SupportInfoDisplay SupportInfo;
		internal XnaHelpers XnaHelpers;

		
		public ModHotKey ControlPanelHotkey = null;
		public ModHotKey DataDumpHotkey = null;



		////////////////

		private void InitializeOuter() {
			this.ExceptionMngr = new HamstarExceptionManager();
			this.AnimatedColors = new AnimatedColorsManager();
		}

		private void LoadOuter() {
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
			this.PlayerState = new PlayerState();
			this.WorldStateHelpers = new WorldStateHelpers();
			this.ControlPanel = new UIControlPanel();
			this.ModLockHelpers = new ModLockHelpers();
			this.EntityGroups = new EntityGroups();
			this.PlayerMessages = new PlayerMessages();
			this.Inbox = new InboxControl();
			this.GetModVersion = new GetModVersion();
			this.GetModTags = new GetModTags();
			this.MenuItemMngr = new MenuItemManager();
			this.MenuContextMngr = new MenuContextServiceManager();
			this.MusicHelpers = new MusicHelpers();
			this.PlayerIdentityHelpers = new PlayerIdentityHelpers();
			this.CustomEntMngr = new CustomEntityManager();
			this.CustomHotkeys = new CustomHotkeys();
			this.XnaHelpers = new XnaHelpers();
			this.ServerInfo = new ServerInfo();
			//this.PlayerDataMngr = new PlayerDataManager();
			this.SupportInfo = new SupportInfoDisplay();
		}


		public void UnloadOuter() {
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
			this.PlayerState = null;
			this.LoadHelpers = null;
			this.GetModVersion = null;
			this.GetModTags = null;
			this.WorldStateHelpers = null;
			this.ModLockHelpers = null;
			this.EntityGroups = null;
			this.AnimatedColors = null;
			this.PlayerMessages = null;
			this.Inbox = null;
			this.ControlPanel = null;
			this.MenuItemMngr = null;
			this.MenuContextMngr = null;
			this.MusicHelpers = null;
			this.PlayerIdentityHelpers = null;
			this.CustomEntMngr = null;
			this.Promises = null;
			this.DataStore = null;
			this.CustomHotkeys = null;
			this.XnaHelpers = null;
			this.ServerInfo = null;
			//this.PlayerDataMngr = null;
			this.SupportInfo = null;

			this.ControlPanelHotkey = null;
			this.DataDumpHotkey = null;
		}

		////////////////

		private void PostSetupContentOuter() {
			this.PacketProtocols = PacketProtocol.GetProtocolTypes();

			this.Promises.OnPostSetupContent();
			this.ModMetaDataManager.OnPostSetupContent();
			this.GetModVersion.OnPostSetupContent();
			this.GetModTags.OnPostSetupContent();

			if( !Main.dedServ ) {
				Menus.OnPostSetupContent();
				UIControlPanel.OnPostSetupContent( this );
			}
		}

		////////////////

		private void AddRecipeGroupsOuter() {
			NPCBannerHelpers.InitializeBanners();

			foreach( var kv in RecipeHelpers.Groups ) {
				RecipeGroup.RegisterGroup( kv.Key, kv.Value );
			}
		}

		private void PostAddRecipesOuter() {
			this.ItemIdentityHelpers.PopulateNames();
			this.NPCIdentityHelpers.PopulateNames();
			this.ProjectileIdentityHelpers.PopulateNames();
			this.BuffIdentityHelpers.PopulateNames();
		}
	}
}
