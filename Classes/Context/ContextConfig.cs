using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader.Config;
using HamstarHelpers.Helpers.NPCs;
using HamstarHelpers.Classes.Tiles.TilePattern;


namespace HamstarHelpers.Classes.Context {
	/// <summary>
	/// Defines a ModConfig-friendly class for defining a Context.
	/// </summary>
	public class ContextConfig {
		/// <summary></summary>
		public TilePatternConfig TilePattern { get; set; } = null;

		/// <summary></summary>
		public ISet<Rectangle> TileRegions { get; set; } = new HashSet<Rectangle>();

		/// <summary></summary>
		public ISet<NPCDefinition> ActiveNPCs { get; set; } = new HashSet<NPCDefinition>();

		/// <summary></summary>
		public int IsBoss { get; set; } = 0;

		//public ISet<string> Biomes = null;

		/// <summary></summary>
		[DefaultValue( true )]
		public bool IsDay { get; set; } = true;

		/// <summary></summary>
		[DefaultValue(true)]
		public bool IsNight { get; set; } = true;

		/// <summary></summary>
		public ISet<int> MoonPhases { get; set; } = new HashSet<int>();

		/// <summary></summary>
		public ISet<int> Events { get; set; } = new HashSet<int>();   //VanillaEventFlag

		/// <summary></summary>
		public ISet<string> Progress { get; set; } = new HashSet<string>();

		/// <summary></summary>
		public ISet<ContextConfig> AnyOthers { get; set; } = new HashSet<ContextConfig>();



		////////////////

		/// <summary>
		/// Converts this class to a valid `Context`.
		/// </summary>
		/// <returns></returns>
		public Context ToContext() {
			return new Context {
				TilePattern = this.TilePattern.ToTilePattern(),
				TileRegions = this.TileRegions.Count > 0 ? this.TileRegions : null,
				ActiveNPCs = this.ActiveNPCs.Count > 0 ? this.ActiveNPCs : null,
				IsBoss = this.IsBoss != 0 ? (bool?)(this.IsBoss == 1) : null,
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
