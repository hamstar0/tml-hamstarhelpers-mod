using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader.Config;
using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.NPCs;
using HamstarHelpers.Helpers.Info;
using System.Linq;


namespace HamstarHelpers.Classes.Context {
	/// <summary>
	/// Defines a general-purpose context of the game.
	/// </summary>
	public class Context {
		/// <summary>
		/// Tile 'pattern' that must exist at the location of the context. Leave null to skip.
		/// </summary>
		public TilePattern TilePattern = null;

		//public ISet<string> Biomes = null;

		/// <summary>
		/// NPCs that must be active for this context to exist. Leave null to skip.
		/// </summary>
		public ISet<NPCDefinition> ActiveNPCs = null;

		/// <summary>
		/// Context is active when bosses are (true), or else only when they're not (false). Leave null to skip.
		/// </summary>
		public bool? IsBoss = null;

		/// <summary>
		/// Context can be active at day.
		/// </summary>
		public bool IsDay = true;

		/// <summary>
		/// Context can be active at night.
		/// </summary>
		public bool IsNight = true;

		/// <summary>
		/// Moon phases that the current context can apply during. Leave null to skip.
		/// </summary>
		public ISet<int> MoonPhases = null;

		/// <summary>
		/// Sets of vanilla events flags that must be set for the current context to apply. Leave null to skip.
		/// </summary>
		public ISet<VanillaEventFlag> Events = null;

		/// <summary>
		/// Progress point of the game (via. GameInfoHelpers.GetVanillaProgress()) that must exist for the current context to apply.
		/// Leave null to skip.
		/// </summary>
		public ISet<string> Progress = null;

		/// <summary>
		/// Custom condition for the current context to apply. Leave null to skip.
		/// </summary>
		public Func<bool> Custom = null;

		/// <summary>
		/// Other contexts that can also apply or the current one to also apply. Leave null to skip.
		/// </summary>
		public ISet<Context> AnyOthers = null;



		////////////////

		/// <summary>
		/// Evaluates the current context with the game's global state.
		/// </summary>
		/// <returns></returns>
		public bool Check() {
			return this.Check( 0, 0 );
		}

		/// <summary>
		/// Evaluates the current context with the game's global state at a specific point in the world.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <returns></returns>
		public bool Check( int tileX, int tileY ) {
			if( !this.IsDay && Main.dayTime ) {
				return false;
			}

			if( !this.IsNight && !Main.dayTime ) {
				return false;
			}

			if( this.TilePattern != null ) {
				if( !this.TilePattern.Check(tileX, tileY) ) {
					return false;
				}
			}

			/*if( this.TileRegions != null ) {
				foreach( Rectangle region in this.TileRegions ) {
					if( region.Contains(tileX, tileY) ) {
						return false;
					}
				}
			}*/

			if( this.ActiveNPCs != null ) {
				foreach( NPCDefinition npcDef in this.ActiveNPCs ) {
					if( !NPC.AnyNPCs(npcDef.Type) ) {
						return false;
					}
				}
			}

			if( this.IsBoss.HasValue ) {
				if( Main.npc.Any(n=>n?.active == true && n.boss == true && n != Main.npc[Main.maxNPCs]) != this.IsBoss.Value ) {
					return false;
				}
			}

			if( this.MoonPhases != null ) {
				if( !this.MoonPhases.Contains(Main.moonPhase) ) {
					return false;
				}
			}

			if( this.Events != null ) {
				VanillaEventFlag currentEvents = NPCInvasionHelpers.GetCurrentEventTypeSet();

				foreach( VanillaEventFlag events in this.Events ) {
					if( (events & currentEvents) != events ) {
						return false;
					}
				}
			}

			if( this.Progress != null ) {
				if( !this.Progress.Contains(GameInfoHelpers.GetVanillaProgress()) ) {
					return false;
				}
			}

			if( this.Custom != null ) {
				if( !this.Custom.Invoke() ) {
					return false;
				}
			}

			if( this.AnyOthers != null ) {
				if( !this.AnyOthers.Any(c => c.Check()) ) {
					return false;
				}
			}

			return true;
		}
	}
}
