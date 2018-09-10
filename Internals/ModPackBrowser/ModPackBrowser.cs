using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Components.UI.Menu;
using System.Collections.Generic;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModPackBrowser {
	class ModPackBrowser {
		public static IDictionary<string, string> Tags = new Dictionary<string, string> {
			{ "Core Game Mechanics", "Adds \"bullet hell\" gameplay, adds a stamina bar, removes mining, etc." },
			{ "Combat Mechanics", "Adds weapon reloading, dual-wielding, changes mob behaviors, etc." },
			{ "Movement Mechanics", "Gives super speed, adds dodging mechanics, adds unlimited flight, etc." },
			{ "Mining Mechanics", "Adds new types of mining tools or makes specific types of excavation faster." },
			{ "Building Mechanics", "Enables placing prefabs, provides measuring tools, adds abilities for editing world, etc." },
			{ "Creativity", "Adds decorative items, enables changing scenery or colors, adds tools for abstract constructrs, etc." },
			{ "Quests", "Adds goals for player to progress or profit from." },
			{ "Game Mode(s)", "Alters game " },
			{ "Story or Lore", "" },
			{ "Traveling", "" },
			{ "Player Class(es)", "" },
			{ "Player Stats", "" },
			{ "NPC Stats", "" },
			{ "Informational", "" },
			{ "Specialized", "" },
			{ "Replaces Game Element(s)", "" },
			{ "Visuals", "" },
			{ "Special FX", "" },
			{ "Affects Physical World", "" },
			{ "Esoteric", "" },
			{ "Challenge", "" },
			{ "Makes Game Easier", "" },
			{ "Adds Convenience", "" },
			{ "Vanilla Balanced", "" },
			{ "Loosely Balanced", "" },
			{ "Cheat-like", "" },
			{ "Theme: Dark", "" },
			{ "Theme: Silly", "" },
			{ "Theme: Fantasy", "" },
			{ "Theme: Military", "" },
			{ "Theme: Futuristic", "" },
			{ "Theme: Mixed", "" },
			{ "Content: Music", "" },
			{ "Content: Rich Art", "" },
			{ "Content: Sounds", "" },
			{ "Content: Item Collections", "" },
			{ "Content: Weapons", "" },
			{ "Content: Hostile NPCs", "" },
			{ "Content: Town NPCs", "" },
			{ "Content: Critters", "" },
			{ "Content: Bosses", "" },
			{ "Content: Invasions", "" },
			{ "Content: Building Blocks", "" },
			{ "Content: Wiring", "" },
			{ "Content: Decorative", "" },
			{ "Content: Fishing", "" },
			{ "Content: Buffs & Potions", "" },
			{ "Content: Accessories", "" },
			{ "Content: Biomes", "" },
			{ "Content: Vanity", "" },
			{ "Content: Ores", "" },
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
			{ "When: Post-Moon Lord", "" },
			{ "Multiplayer Compatible", "" },
			{ "PvP", "" },
			{ "Teams", "" },
			{ "Open Source", "" },
			{ "Made By Team", "" },
			{ "Unmaintained", "" },
			{ "Unfinished", "" },
			{ "Buggy", "" },
			{ "Non-functional", "" },
			{ "Minimalistic", "" },
			{ "Simplistic", "" },
			{ "Shows Effort", "" },
			{ "Polished", "" }
		};
		


		public static void Initialize() {
			var buttons = new UIElement();

			foreach( var kv in ModPackBrowser.Tags ) {
				var button = new UICheckbox( kv.Key, kv.Value, true, 0.5f, false );
				button.Top.Set( 11f, 0f );
				button.Left.Set( -104f, 0.5f );
				button.Width.Set( 208f, 0f );
				button.Height.Set( 20f, 0f );

				buttons.Append( button );
			}

			MenuUI.AddMenuLoader( "UIModInfo", "ModHelpers: Mod Info Tags", buttons );
		}
	}
}
