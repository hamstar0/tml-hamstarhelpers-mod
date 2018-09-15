using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModPackBrowser {
	internal class UIMenuTextPanelButton : UITextPanelButton {
		public const int ColumnHeightTall = 31;
		public const int ColumnHeightShort = 8;
		public const int ColumnsInMid = 5;

		public int Column;
		public int Row;


		////////////////
		
		public UIMenuTextPanelButton( int pos, string label, float scale=1f, bool large=false )
				: base( UITheme.Vanilla, label, scale, large ) {
			int col_tall = UIMenuTextPanelButton.ColumnHeightTall;
			int col_short = UIMenuTextPanelButton.ColumnHeightShort;
			int cols_in_mid = UIMenuTextPanelButton.ColumnsInMid;

			if( pos < col_tall ) {
				this.Column = 0;
				this.Row = pos;
			} else if( pos > ( col_tall + ( col_short * cols_in_mid ) ) ) {
				this.Column = 7;
				this.Row = pos - ( col_tall + ( col_short * cols_in_mid ) );
			} else {
				this.Column = 1 + (( pos - col_tall ) / col_short );
				this.Row = ( pos - col_tall ) % col_short;
			}
			
			this.RecalculatePos();
		}

		////////////////

		private void RecalculatePos() {
			float width = this.Width.Pixels;
			float left = (( ( Main.screenWidth / 2 ) - 296 ) - (width - 8)) + ( (width - 2) * this.Column );
			float top = (16 * this.Row) + 48;

			this.Left.Set( left, 0f );
			this.Top.Set( top, 0f );
		}

		public override void Recalculate() {
			this.RecalculatePos();
			base.Recalculate();
		}
	}




	class ModPackBrowser {
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
			var buttons = new UIElement();
			int col_height = 35;
			
			var hover_text = new UIText( "" );
			hover_text.Width.Set( 0, 0 );
			hover_text.Height.Set( 0, 0 );

			int i = 0;
			foreach( var kv in ModPackBrowser.Tags ) {
				string tag_text = kv.Key;
				string tag_desc = kv.Value;

				var button = new UIMenuTextPanelButton( i, tag_text, 0.6f );
				button.Width.Set( 120f, 0f );
				button.Height.Set( 16f, 0f );
				button.OnClick += (UIMouseEvent evt, UIElement listeningElement) => {
					LogHelpers.Log( "1" );
				};
				button.OnMouseOver += ( UIMouseEvent evt, UIElement listeningElement ) => {
					hover_text.SetText( tag_desc );
					hover_text.Left.Set( Main.mouseX, 0f );
					hover_text.Top.Set( Main.mouseY, 0f );
					hover_text.Recalculate();
				};
				button.OnMouseOut += ( UIMouseEvent evt, UIElement listeningElement ) => {
					if( hover_text.Text == tag_desc ) {
						hover_text.SetText( "" );
						hover_text.Recalculate();
					}
				};

				MenuUI.AddMenuLoader( "UIModInfo", "ModHelpers: Mod Info Tags "+i, button, false );
				
				i++;
			}

			MenuUI.AddMenuLoader( "UIModInfo", "ModHelpers: Mod Info Tags Hover", hover_text, false );
		}
	}
}
