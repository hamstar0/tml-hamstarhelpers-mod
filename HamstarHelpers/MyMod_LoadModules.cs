using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
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
using HamstarHelpers.Libraries.Audio;
using HamstarHelpers.Libraries.Misc;
using HamstarHelpers.Libraries.TModLoader;
using HamstarHelpers.Libraries.World;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.Net;
using HamstarHelpers.Libraries.Buffs;
using HamstarHelpers.Libraries.Items.Attributes;
using HamstarHelpers.Libraries.NPCs;
using HamstarHelpers.Libraries.NPCs.Attributes;
using HamstarHelpers.Libraries.Projectiles.Attributes;
using HamstarHelpers.Libraries.Recipes;
using HamstarHelpers.Libraries.XNA;
using HamstarHelpers.Libraries.Players;
using HamstarHelpers.Libraries.ModHelpers;
using HamstarHelpers.Libraries.DotNET.Reflection;
using HamstarHelpers.Libraries.TModLoader.Mods;


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
#pragma warning disable CS0618 // Type or member is obsolete
		internal RecipeHack RecipeHack;
#pragma warning restore CS0618 // Type or member is obsolete
		internal ExtendedPlayerHooks PlayerHooks;
		internal WorldTimeHooks WorldTimeHooks;

		// Helpers
		internal ModFeaturesLibraries ModFeaturesHelpers;
		internal LogLibraries LogHelpers;
		internal NetPlayLibraries NetHelpers;
		internal BuffLibraries BuffHelpers;
		internal NPCAttributeLibraries NPCAttributeHelpers;
		internal ProjectileAttributeLibraries ProjectileAttributeHelpers;
		internal BuffAttributesLibraries BuffIdentityHelpers;
		internal NPCBannerLibraries NPCBannerHelpers;
		internal RecipeFinderLibraries RecipeFinderHelpers;
		internal RecipeGroupLibraries RecipeGroupHelpers;
		internal LoadLibraries LoadHelpers;
		internal WorldStateLibraries WorldStateHelpers;
		internal Libraries.Audio.MusicLibraries MusicHelpers;
		internal PlayerIdentityLibraries PlayerIdentityHelpers;
		internal ReflectionLibraries ReflectionHelpers;
		internal XNALibraries XnaHelpers;
		internal ModListLibraries ModListHelpers;
		internal ItemAttributeLibraries ItemAttributeHelpers;

		// Internals
		internal InboxControl Inbox;
		internal GetModTags GetModTags;
		internal GetModInfo GetModInfo;
		internal UIControlPanel ControlPanelUI;
		internal SupportInfoDisplay SupportInfo;

		
		public ModHotKey ControlPanelHotkey = null;
		public ModHotKey DataDumpHotkey = null;



		////////////////
		
		private void InitializeModules() {
			this.LogHelpers = new LogLibraries();
			this.Loadables = new LoadableManager();
			this.ReflectionHelpers = new ReflectionLibraries();

			this.ExceptionMngr = new ModHelpersExceptionManager();
			this.AnimatedColors = new AnimatedColorsManager();
			this.AnimatedTextures = new AnimatedTextureManager();
		}

		private void LoadModules() {
			this.Loadables.OnModsLoad();

			this.DataStore = new DataStore();
			this.LoadHooks = new LoadHooks();
			this.CustomLoadHooks = new CustomLoadHooks();
			this.LoadHelpers = new LoadLibraries();

			this.Timers = new Timers();
			this.ModFeaturesHelpers = new ModFeaturesLibraries();
			this.PacketProtocolMngr = new PacketProtocolManager();

			this.BuffHelpers = new BuffLibraries();
			this.NetHelpers = new NetPlayLibraries();
			this.NPCAttributeHelpers = new NPCAttributeLibraries();
			this.ProjectileAttributeHelpers = new ProjectileAttributeLibraries();
			this.BuffIdentityHelpers = new BuffAttributesLibraries();
			this.NPCBannerHelpers = new NPCBannerLibraries();
			this.RecipeFinderHelpers = new RecipeFinderLibraries();
			this.RecipeGroupHelpers = new RecipeGroupLibraries();
			this.PlayerHooks = new ExtendedPlayerHooks();
			this.WorldTimeHooks = new WorldTimeHooks();
			this.WorldStateHelpers = new WorldStateLibraries();
			this.ControlPanelUI = new UIControlPanel();
			this.ModLock = new ModLockService();
			this.EntityGroups = new EntityGroups();
			this.PlayerMessages = new PlayerMessages();
			this.Inbox = new InboxControl();
			this.GetModInfo = new GetModInfo();
			this.GetModTags = new GetModTags();
			this.MenuItemMngr = new MenuItemManager();
			this.MenuContextMngr = new MenuContextServiceManager();
			this.MusicHelpers = new Libraries.Audio.MusicLibraries();
			this.PlayerIdentityHelpers = new PlayerIdentityLibraries();
			this.CustomHotkeys = new CustomHotkeys();
			this.XnaHelpers = new XNALibraries();
			this.Server = new Server();
			//this.PlayerDataMngr = new PlayerDataManager();
			this.SupportInfo = new SupportInfoDisplay();
#pragma warning disable CS0618 // Type or member is obsolete
			this.RecipeHack = new RecipeHack();
#pragma warning restore CS0618 // Type or member is obsolete
			this.ModListHelpers = new ModListLibraries();
			this.ItemAttributeHelpers = new ItemAttributeLibraries();
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
			this.ControlPanelUI = null;
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

			foreach( var kv in RecipeGroupLibraries.Groups ) {
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
