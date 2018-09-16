using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Promises;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.IO;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModPackBrowser {
	class ModTags {
		public static IDictionary<string, string> Tags = new Dictionary<string, string> {
			{ "Core Game Mechanics", "Adds a \"bullet hell\" mode, adds a stamina bar, removes mining, etc." },
			{ "Combat Mechanics", "Adds weapon reloading, dual-wielding, changes mob behaviors, etc." },
			{ "Movement Mechanics", "Gives super speed, adds dodging mechanics, adds unlimited flight, etc." },
			{ "Mining Mechanics", "Adds new types of mining tools or makes specific types of excavation faster." },
			{ "Building Mechanics", "Enables placing prefabs, provides measuring tools, adds abilities for editing world, etc." },
			{ "Creativity", "Adds decorative items, enables changing scenery or colors, adds tools for abstract constructrs, etc." },
			{ "Quests", "Adds goals for player to progress or profit from." },
			{ "Game Mode(s)", "New game rules; added end goals, progression, setting, session, etc." },
			{ "Story or Lore", "Implements elements of story telling or universe lore." },
			{ "Traveling", "Adds fast travel options, adds new minecarts, rewards for distance traveled, etc." },
			{ "Player Class(es)", "Adds new player 'classes', usually via. custom damage types or special forms of attack/defense." },
			{ "Player Stats", "Modifies player attack, defense, and other intrinsic elements." },
			{ "NPC Stats", "Modifies NPC attack, defense, and other intrinsic elements." },
			{ "NPC Behavior", "Modifies NPC AIs for new or tweaked behaviors." },
			{ "Informational", "Adds informational items (time, weather, etc.), reports game statistics, gives specific information, etc." },
			{ "Specialized", "Focuses on a specific, well-defined function." },
			{ "Replacements", "Primarily meant as an alternative to something the game already provides." },
			{ "Visuals", "Implements new or improved sprites, adds new background details, etc." },
			{ "Special FX", "Adds gore effects, adds motion blurs, improves particle effects, etc." },
			{ "Affects World", "Generates new set pieces, alters biome shapes, populates areas with new types of growth, etc." },
			{ "Esoteric", "Does something uncommon or unexpected." },
			{ "Challenge", "Increases difficulty of something, e.g. for player bragging rights." },
			{ "Nerfs", "Decreases difficulty of something, e.g. to make beating bosses easier." },
			{ "Adds Convenience", "Reduces annoyances; adds auto-reactivating buffs, auto-trashes junk items, centralizes chests, etc." },
			{ "Vanilla Balanced", "Balanced around plain Terraria; progress will not happen faster than usual." },
			{ "Loosely Balanced", "Little concern for affect on progress; may enable Vanilla sequence breaks." },
			{ "Cheat-like", "Significantly reduces or removes some game challenges completely; may be 'unfair'." },
			{ "Theme: Dark", "Gloomy, edgy, or just plain poor visibility." },
			{ "Theme: Silly", "Light-hearted, immersion-breaking, or just plain absurd." },
			{ "Theme: Fantasy", "Elements of ancient mythologies, swords & sorcery, and maybe a hobbit or 2." },
			{ "Theme: Military", "Guns and stuff." },
			{ "Theme: Futuristic", "Robots, lasers, flying machines, etc." },
			//{ "Theme: Mixed", "Mashup of genres, but not purely in a silly way." },
			{ "Content: Music", "Adds new music." },
			{ "Content: Rich Art", "Adds extensive or detailed art for new or existing game entities." },
			{ "Content: Sounds", "Adds new sound effects or ambience." },
			{ "Content: Item Sets", "Adds discrete (often themed) sets of items." },
			{ "Content: Weapons", "" },
			{ "Content: Hostile NPCs", "" },
			{ "Content: Town NPCs", "" },
			{ "Content: Critters", "Adds (passive) biome fauna." },
			{ "Content: Bosses", "" },
			{ "Content: Invasions", "" },
			{ "Content: Blocks", "" },
			{ "Content: Wiring", "Adds tools and toys for use for wiring." },
			{ "Content: Decorative", "Adds decorative objects (e.g. furniture for houses)." },
			{ "Content: Fishing", "Adds tools or mechanics for fishing." },
			{ "Content: Buffs & Pots.", "" },
			{ "Content: Accessories", "" },
			{ "Content: Biomes", "" },
			{ "Content: Vanity", "" },
			{ "Content: Ores", "Adds mineable ores (and probably matching equipment tiers)." },
			{ "Where: Surface", "" },
			{ "Where: Underground", "" },
			{ "Where: Ocean", "" },
			{ "Where: Dungeon", "" },
			{ "Where: Jungle", "" },
			{ "Where: Hell", "" },
			{ "Where: Evil Biome", "" },
			{ "Where: Hallow Biome", "" },
			{ "When: Pre-Hard Mode", "" },
			{ "When: Hard Mode", "" },
			{ "When: Post-Plantera", "" },
			{ "When: Post-Moonlord", "" },
			{ "Multiplayer Compatible", "" },
			{ "PvP", "" },
			{ "Teams", "" },
			{ "Open Source", "" },
			//{ "Made By Team", "" },
			{ "Unmaintained", "" },
			{ "Unfinished", "" },
			{ "Non-functional", "" },
			{ "Simplistic", "" }
			//{ "Buggy", "" },
			//{ "Minimalistic", "" },
			//{ "Shows Effort", "" },
			//{ "Polished", "" }
		};


		////////////////

		public static void Initialize() {
			var hover_elem = new UIText( "" );
			hover_elem.Width.Set( 0, 0 );
			hover_elem.Height.Set( 0, 0 );

			var subup_button = new UISubmitUpdateButton();

			MenuUI.AddMenuLoader( "UIModInfo", "ModHelpers: Mod Info Tags Submit+Update", subup_button, false );
			MenuUI.AddMenuLoader( "UIModInfo", "ModHelpers: Mod Info Tags Hover", hover_elem, false );
			MenuUI.AddMenuLoader( "UIModInfo", "ModHelpers: Mod Info Load", ( ui ) => {
				Type ui_type = ui.GetType();
				FieldInfo ui_localmod_field = ui_type.GetField( "localMod", BindingFlags.NonPublic | BindingFlags.Instance );
				if( ui_localmod_field == null ) {
					LogHelpers.Log( "No 'localMod' field in " + ui_type );
					return;
				}
				
				object localmod = ui_localmod_field.GetValue( ui );
				Type localmod_type = localmod.GetType();
				FieldInfo localmod_modfile_field = localmod_type.GetField( "modFile", BindingFlags.Public | BindingFlags.Instance );
				if( localmod_modfile_field == null ) {
					LogHelpers.Log( "No 'modFile' field in " + localmod_type );
					return;
				}

				var modfile = (TmodFile)localmod_modfile_field.GetValue( localmod );
				if( modfile == null ) {
					LogHelpers.Log( "Empty 'mod' field" );
					return;
				}

				subup_button.SetMod( modfile.name );

				Promises.AddValidatedPromise<ModTagsPromiseArguments>( GetModTags.TagsReceivedPromiseValidator, ( args ) => {
					if( args.Found ) {
						ModTags.InitializeButtons( args.ModTags[modfile.name], hover_elem );
					} else {
						ModTags.InitializeButtons( new HashSet<string>(), hover_elem );
					}
					return false;
				} );
			}, _ => { } );
		}


		private static void InitializeButtons( ISet<string> modtags, UIText hover_elem ) {
			var buttons = new Dictionary<string, UIModTagButton>();

			int i = 0;
			foreach( var kv in ModTags.Tags ) {
				string tag_text = kv.Key;
				string tag_desc = kv.Value;
				bool has_tag = modtags.Contains( tag_text );

				var button = new UIModTagButton( i, tag_text, tag_desc, has_tag, hover_elem, 0.6f );
				button.OnClick += ( UIMouseEvent evt, UIElement listeningElement ) => {
					button.ToggleTag();
				};

				MenuUI.AddMenuLoader( "UIModInfo", "ModHelpers: Mod Info Tags " + i, button, false );
				buttons[tag_text] = button;

				i++;
			}
		}
	}
}
