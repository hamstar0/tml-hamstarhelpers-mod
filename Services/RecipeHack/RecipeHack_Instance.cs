using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.RecipeHack {
	public partial class RecipeHack {
		private static object MyLock = new object();



		////////////////

		private IDictionary<string, Func<IEnumerable<Item>>> RecipeOutsources = new Dictionary<string, Func<IEnumerable<Item>>>();



		////////////////

		internal RecipeHack() { }
	}
}
