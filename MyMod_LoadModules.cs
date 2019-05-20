using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Internals.ControlPanel;
using HamstarHelpers.Internals.Menus;
using HamstarHelpers.Internals.Menus.Support;
using HamstarHelpers.Internals.Inbox;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Services.AnimatedColor;
using HamstarHelpers.Services.EntityGroups;
using HamstarHelpers.Services.Messages;
using HamstarHelpers.Services.Timers;
using HamstarHelpers.Services.Promises;
using HamstarHelpers.Services.DataStore;
using HamstarHelpers.Services.Menus;
using HamstarHelpers.Services.ServerInfo;
using HamstarHelpers.Services.CustomHotkeys;
using HamstarHelpers.Services.ExtendedHooks;
using HamstarHelpers.Services.ModHelpers;
using HamstarHelpers.Services.RecipeHack;
using HamstarHelpers.Helpers.MiscHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
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
using HamstarHelpers.Helpers.DotNetHelpers.Reflection;
using HamstarHelpers.Helpers.TmlHelpers.ModHelpers;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;


namespace HamstarHelpers {
	partial class ModHelpersMod : Mod {
		// Components
		internal HamstarExceptionManager ExceptionMngr;
		internal MenuContextServiceManager MenuContextMngr;
		internal MenuItemManager MenuItemMngr;
		internal PacketProtocolManager PacketProtocolMngr;

		// Services
		internal Promises Promises;
		internal Timers Timers;
		internal EntityGroups EntityGroups;
		internal AnimatedColorsManager AnimatedColors;
		internal PlayerMessages PlayerMessages;
		internal DataStore DataStore;
		internal CustomHotkeys CustomHotkeys;
		internal ServerInfo ServerInfo;
		internal ModLockService ModLock;
		//internal PlayerDataManager PlayerDataMngr;
		internal RecipeHack RecipeHack;
		internal ExtendedPlayerHooks PlayerHooks;

		// Helpers
		internal ModFeaturesHelpers ModFeaturesHelpers;
		internal LogHelpers LogHelpers;
		internal NetHelpers NetHelpers;
		internal BuffHelpers BuffHelpers;
		internal ItemIdentityHelpers ItemIdentityHelpers;
		internal NPCIdentityHelpers NPCIdentityHelpers;
		internal ProjectileIdentityHelpers ProjectileIdentityHelpers;
		internal BuffIdentityHelpers BuffIdentityHelpers;
		internal NPCBannerHelpers NPCBannerHelpers;
		internal RecipeIdentityHelpers RecipeIdentityHelpers;
		internal RecipeGroupHelpers RecipeGroupHelpers;
		internal LoadHelpers LoadHelpers;
		internal WorldStateHelpers WorldStateHelpers;
		internal MusicHelpers MusicHelpers;
		internal PlayerIdentityHelpers PlayerIdentityHelpers;
		internal ReflectionHelpers ReflectionHelpers;
		internal XnaHelpers XnaHelpers;
		internal ModListHelpers ModListHelpers;

		// Internals
		internal InboxControl Inbox;
		internal GetModTags GetModTags;
		internal GetModInfo GetModInfo;
		internal UIControlPanel ControlPanel;
		internal SupportInfoDisplay SupportInfo;

		
		public ModHotKey ControlPanelHotkey = null;
		public ModHotKey DataDumpHotkey = null;



		////////////////

		private void InitializeModules() {
			this.ExceptionMngr = new HamstarExceptionManager();
			this.AnimatedColors = new AnimatedColorsManager();
		}

		private void LoadModules() {
			this.ReflectionHelpers = new ReflectionHelpers();
			this.DataStore = new DataStore();
			this.Promises = new Promises();
			this.LoadHelpers = new LoadHelpers();

			this.Timers = new Timers();
			this.LogHelpers = new LogHelpers();
			this.ModFeaturesHelpers = new ModFeaturesHelpers();
			this.PacketProtocolMngr = new PacketProtocolManager();

			this.BuffHelpers = new BuffHelpers();
			this.NetHelpers = new NetHelpers();
			this.ItemIdentityHelpers = new ItemIdentityHelpers();
			this.NPCIdentityHelpers = new NPCIdentityHelpers();
			this.ProjectileIdentityHelpers = new ProjectileIdentityHelpers();
			this.BuffIdentityHelpers = new BuffIdentityHelpers();
			this.NPCBannerHelpers = new NPCBannerHelpers();
			this.RecipeIdentityHelpers = new RecipeIdentityHelpers();
			this.RecipeGroupHelpers = new RecipeGroupHelpers();
			this.PlayerHooks = new ExtendedPlayerHooks();
			this.WorldStateHelpers = new WorldStateHelpers();
			this.ControlPanel = new UIControlPanel();
			this.ModLock = new ModLockService();
			this.EntityGroups = new EntityGroups();
			this.PlayerMessages = new PlayerMessages();
			this.Inbox = new InboxControl();
			this.GetModInfo = new GetModInfo();
			this.GetModTags = new GetModTags();
			this.MenuItemMngr = new MenuItemManager();
			this.MenuContextMngr = new MenuContextServiceManager();
			this.MusicHelpers = new MusicHelpers();
			this.PlayerIdentityHelpers = new PlayerIdentityHelpers();
			this.CustomHotkeys = new CustomHotkeys();
			this.XnaHelpers = new XnaHelpers();
			this.ServerInfo = new ServerInfo();
			//this.PlayerDataMngr = new PlayerDataManager();
			this.SupportInfo = new SupportInfoDisplay();
			this.RecipeHack = new RecipeHack();
			this.ModListHelpers = new ModListHelpers();
		}


		public void UnloadModules() {
			this.ReflectionHelpers = null;
			this.PacketProtocolMngr = null;
			this.ExceptionMngr = null;
			this.Timers = null;
			this.ConfigJson = null;
			this.LogHelpers = null;
			this.ModFeaturesHelpers = null;
			this.BuffHelpers = null;
			this.NetHelpers = null;
			this.ItemIdentityHelpers = null;
			this.NPCIdentityHelpers = null;
			this.ProjectileIdentityHelpers = null;
			this.BuffIdentityHelpers = null;
			this.NPCBannerHelpers = null;
			this.RecipeIdentityHelpers = null;
			this.RecipeGroupHelpers = null;
			this.PlayerHooks = null;
			this.LoadHelpers = null;
			this.GetModInfo = null;
			this.GetModTags = null;
			this.WorldStateHelpers = null;
			this.ModLock = null;
			this.EntityGroups = null;
			this.AnimatedColors = null;
			this.PlayerMessages = null;
			this.Inbox = null;
			this.ControlPanel = null;
			this.MenuItemMngr = null;
			this.MenuContextMngr = null;
			this.MusicHelpers = null;
			this.PlayerIdentityHelpers = null;
			this.Promises = null;
			this.DataStore = null;
			this.CustomHotkeys = null;
			this.XnaHelpers = null;
			this.ServerInfo = null;
			//this.PlayerDataMngr = null;
			this.SupportInfo = null;
			this.RecipeHack = null;
			this.ModListHelpers = null;

			this.ControlPanelHotkey = null;
			this.DataDumpHotkey = null;
		}

		////////////////

		private void PostSetupContentModules() {
			this.PacketProtocolMngr.OnPostSetupContent();
			this.Promises.OnPostSetupContent();
			this.ModFeaturesHelpers.OnPostSetupContent();
			this.PlayerIdentityHelpers.OnPostSetupContent();

			if( !Main.dedServ && Main.netMode != NetmodeID.Server ) {
				this.GetModInfo.OnPostSetupContent();
				this.GetModTags.OnPostSetupContent();

				Menus.OnPostSetupContent();
				UIControlPanel.OnPostSetupContent();
			}
		}

		////////////////

		private void AddRecipeGroupsModules() {
			NPCBannerHelpers.InitializeBanners();

			foreach( var kv in RecipeGroupHelpers.Groups ) {
				RecipeGroup.RegisterGroup( kv.Key, kv.Value );
			}
		}

		private void PostAddRecipesModules() {
			this.ItemIdentityHelpers.PopulateNames();
			this.NPCIdentityHelpers.PopulateNames();
			this.ProjectileIdentityHelpers.PopulateNames();
			this.BuffIdentityHelpers.PopulateNames();
		}
	}
}
