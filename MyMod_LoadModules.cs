using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Internals.ControlPanel;
using HamstarHelpers.Internals.Menus;
using HamstarHelpers.Internals.Menus.Support;
using HamstarHelpers.Internals.Inbox;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.UI.Menu;
using HamstarHelpers.Classes.Protocols.Packet;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Services.Debug.CustomHotkeys;
using HamstarHelpers.Services.Hooks.ExtendedHooks;
using HamstarHelpers.Services.Hooks.LoadHooks;
using HamstarHelpers.Services.Hooks.WorldHooks;
using HamstarHelpers.Services.AnimatedColor;
using HamstarHelpers.Services.AnimatedTexture;
using HamstarHelpers.Services.EntityGroups;
using HamstarHelpers.Services.Messages.Player;
using HamstarHelpers.Services.Timers;
using HamstarHelpers.Services.DataStore;
using HamstarHelpers.Services.ModHelpers;
using HamstarHelpers.Services.Network;
using HamstarHelpers.Services.RecipeHack;
using HamstarHelpers.Services.UI.Menus;
using HamstarHelpers.Helpers.Misc;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Helpers.World;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Net;
using HamstarHelpers.Helpers.Buffs;
using HamstarHelpers.Helpers.Items.Attributes;
using HamstarHelpers.Helpers.NPCs;
using HamstarHelpers.Helpers.NPCs.Attributes;
using HamstarHelpers.Helpers.Projectiles.Attributes;
using HamstarHelpers.Helpers.Recipes;
using HamstarHelpers.Helpers.XNA;
using HamstarHelpers.Helpers.Players;
using HamstarHelpers.Helpers.ModHelpers;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.TModLoader.Mods;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;


namespace HamstarHelpers {
	/// @private
	partial class ModHelpersMod : Mod {
		// Classes
		internal LoadableManager Loadables;
		internal ModHelpersExceptionManager ExceptionMngr;
		internal MenuContextServiceManager MenuContextMngr;
		internal MenuItemManager MenuItemMngr;
		internal PacketProtocolManager PacketProtocolMngr;

		// Services
		internal LoadHooks LoadHooks;
		internal CustomLoadHooks CustomLoadHooks;
		internal Timers Timers;
		internal EntityGroups EntityGroups;
		internal AnimatedTextureManager AnimatedTextures;
		internal AnimatedColorsManager AnimatedColors;
		internal PlayerMessages PlayerMessages;
		internal DataStore DataStore;
		internal CustomHotkeys CustomHotkeys;
		internal Server Server;
		internal ModLockService ModLock;
		//internal PlayerDataManager PlayerDataMngr;
		internal RecipeHack RecipeHack;
		internal ExtendedPlayerHooks PlayerHooks;
		internal WorldTimeHooks WorldTimeHooks;

		// Helpers
		internal ModFeaturesHelpers ModFeaturesHelpers;
		internal LogHelpers LogHelpers;
		internal NetPlayHelpers NetHelpers;
		internal BuffHelpers BuffHelpers;
		internal NPCAttributeHelpers NPCAttributeHelpers;
		internal ProjectileAttributeHelpers ProjectileAttributeHelpers;
		internal BuffAttributesHelpers BuffIdentityHelpers;
		internal NPCBannerHelpers NPCBannerHelpers;
		internal RecipeFinderHelpers RecipeFinderHelpers;
		internal RecipeGroupHelpers RecipeGroupHelpers;
		internal LoadHelpers LoadHelpers;
		internal WorldStateHelpers WorldStateHelpers;
		internal MusicHelpers MusicHelpers;
		internal PlayerIdentityHelpers PlayerIdentityHelpers;
		internal ReflectionHelpers ReflectionHelpers;
		internal XNAHelpers XnaHelpers;
		internal ModListHelpers ModListHelpers;
		internal ItemAttributeHelpers ItemAttributeHelpers;

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
			this.LogHelpers = new LogHelpers();
			this.Loadables = new LoadableManager();
			this.ReflectionHelpers = new ReflectionHelpers();

			this.ExceptionMngr = new ModHelpersExceptionManager();
			this.AnimatedColors = new AnimatedColorsManager();
			this.AnimatedTextures = new AnimatedTextureManager();
		}

		private void LoadModules() {
			this.Loadables.OnModsLoad();

			this.DataStore = new DataStore();
			this.LoadHooks = new LoadHooks();
			this.CustomLoadHooks = new CustomLoadHooks();
			this.LoadHelpers = new LoadHelpers();

			this.Timers = new Timers();
			this.ModFeaturesHelpers = new ModFeaturesHelpers();
			this.PacketProtocolMngr = new PacketProtocolManager();

			this.BuffHelpers = new BuffHelpers();
			this.NetHelpers = new NetPlayHelpers();
			this.NPCAttributeHelpers = new NPCAttributeHelpers();
			this.ProjectileAttributeHelpers = new ProjectileAttributeHelpers();
			this.BuffIdentityHelpers = new BuffAttributesHelpers();
			this.NPCBannerHelpers = new NPCBannerHelpers();
			this.RecipeFinderHelpers = new RecipeFinderHelpers();
			this.RecipeGroupHelpers = new RecipeGroupHelpers();
			this.PlayerHooks = new ExtendedPlayerHooks();
			this.WorldTimeHooks = new WorldTimeHooks();
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
			this.XnaHelpers = new XNAHelpers();
			this.Server = new Server();
			//this.PlayerDataMngr = new PlayerDataManager();
			this.SupportInfo = new SupportInfoDisplay();
			this.RecipeHack = new RecipeHack();
			this.ModListHelpers = new ModListHelpers();
			this.ItemAttributeHelpers = new ItemAttributeHelpers();
		}


		public void UnloadModules() {
			this.Loadables.OnModsUnload();
			
			this.Loadables = null;
			this.ReflectionHelpers = null;
			this.PacketProtocolMngr = null;
			this.ExceptionMngr = null;
			this.Timers = null;
			this.LogHelpers = null;
			this.ModFeaturesHelpers = null;
			this.BuffHelpers = null;
			this.NetHelpers = null;
			this.NPCAttributeHelpers = null;
			this.ProjectileAttributeHelpers = null;
			this.BuffIdentityHelpers = null;
			this.NPCBannerHelpers = null;
			this.RecipeFinderHelpers = null;
			this.RecipeGroupHelpers = null;
			this.PlayerHooks = null;
			this.LoadHelpers = null;
			this.GetModInfo = null;
			this.GetModTags = null;
			this.WorldStateHelpers = null;
			this.ModLock = null;
			this.EntityGroups = null;
			this.AnimatedColors = null;
			this.AnimatedTextures = null;
			this.PlayerMessages = null;
			this.Inbox = null;
			this.ControlPanel = null;
			this.MenuItemMngr = null;
			this.MenuContextMngr = null;
			this.MusicHelpers = null;
			this.PlayerIdentityHelpers = null;
			this.LoadHooks = null;
			this.CustomLoadHooks = null;
			this.DataStore = null;
			this.CustomHotkeys = null;
			this.XnaHelpers = null;
			this.Server = null;
			//this.PlayerDataMngr = null;
			this.SupportInfo = null;
			this.RecipeHack = null;
			this.ModListHelpers = null;
			this.ItemAttributeHelpers = null;
			this.WorldTimeHooks = null;

			this.ControlPanelHotkey = null;
			this.DataDumpHotkey = null;
		}

		////////////////
		
		private void PostSetupFullModules() {
			this.LogHelpers.OnPostModsLoad();
			this.Loadables.OnPostModsLoad();

			this.SupportInfo.OnPostModsLoad();
			this.AnimatedColors.OnPostModsLoad();
			this.AnimatedTextures.OnPostModsLoad();
			this.PacketProtocolMngr.OnPostModsLoad();
			this.LoadHooks.OnPostModsLoad();
			this.ModFeaturesHelpers.OnPostModsLoad();
			this.PlayerIdentityHelpers.OnPostModsLoad();

			if( !Main.dedServ && Main.netMode != NetmodeID.Server ) {
				this.GetModInfo.OnPostModsLoad();
				this.GetModTags.OnPostModsLoad();

				Menus.OnPostModsLoad();
				UIControlPanel.OnPostModsLoad();
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
			this.ItemAttributeHelpers.PopulateNames();
			this.NPCAttributeHelpers.PopulateNames();
			this.ProjectileAttributeHelpers.PopulateNames();
			this.BuffIdentityHelpers.PopulateNames();
		}
	}
}
