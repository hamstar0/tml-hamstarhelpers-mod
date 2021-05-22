using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Terraria;
using Terraria.ModLoader.Config;
using HamstarHelpers.Libraries.NPCs;
using HamstarHelpers.Classes.Tiles.TilePattern;


namespace HamstarHelpers.Classes.Context {
	/// <summary>
	/// Defines a ModConfig-friendly class for defining a Context.
	/// </summary>
	public class ContextConfig {
		/// <summary></summary>
		public TilePatternConfig TilePattern { get; set; }

		/// <summary></summary>
		public HashSet<NPCDefinition> ActiveNPCs { get; set; }

		/// <summary></summary>
		public Ref<bool> IsBoss { get; set; }

		//public ISet<string> Biomes = null;

		/// <summary></summary>
		[DefaultValue( true )]
		public bool IsDay { get; set; } = true;

		/// <summary></summary>
		[DefaultValue(true)]
		public bool IsNight { get; set; } = true;

		/// <summary></summary>
		public HashSet<int> MoonPhases { get; set; }

		/// <summary></summary>
		public HashSet<int> Events { get; set; }   //VanillaEventFlag

		/// <summary></summary>
		public HashSet<string> Progress { get; set; }

		/// <summary></summary>
		public HashSet<ContextConfig> AnyOthers { get; set; }



		////////////////

		/// <summary>
		/// Converts this class to a valid `Context`.
		/// </summary>
		/// <returns></returns>
		public Context ToContext() {
			return new Context {
				TilePattern = this.TilePattern.ToTilePattern(),
				ActiveNPCs = this.ActiveNPCs.Count > 0 ? this.ActiveNPCs : null,
				IsBoss = this.IsBoss != null ? this.IsBoss.Value : (bool?)null,
				IsDay = this.IsDay,
				IsNight = this.IsNight,
				MoonPhases = this.MoonPhases.Count > 0 ? this.MoonPhases : null,
				Events = this.Events.Count > 0 ? new HashSet<VanillaEventFlag>( this.Events.Select(e => (VanillaEventFlag)e) ) : null,
				Progress = this.Progress.Count > 0 ? this.Progress : null,
				AnyOthers = this.AnyOthers.Count > 0 ? new HashSet<Context>( this.AnyOthers.Select(a => a.ToContext()) ) : null,
			};
		}
	}
}
