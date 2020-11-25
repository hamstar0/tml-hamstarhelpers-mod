using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.World.Generation;
using HamstarHelpers.Helpers.DotNET.Reflection;


namespace HamstarHelpers.Helpers.World {
	/// <summary>
	/// Assorted static "helper" functions pertaining to world generation.
	/// </summary>
	public class WorldGenHelpers {
		/// <summary>
		/// Gets the full list of world gen passes.
		/// </summary>
		/// <returns></returns>
		public static IList<GenPass> GetWorldGenPasses() {
			if( !ReflectionHelpers.Get(typeof(WorldGen), null, "_generator", out WorldGenerator generator) ) {
				return null;
			}
			if( !ReflectionHelpers.Get(typeof(WorldGenerator), generator, "_passes", out IList<GenPass> passes) ) {
				return null;
			}
			return passes;
		}
	}
}
