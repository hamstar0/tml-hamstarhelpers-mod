using System;


namespace HamstarHelpers.Classes.Loadable {
	/// <summary>
	/// Affixed to classes that wish to create evens on mod load, post load, and unload.
	/// </summary>
	public interface ILoadable {
		/// <summary>
		/// Called during `Mod.Load()`.
		/// </summary>
		void OnModsLoad();
		/// <summary>
		/// Called after `Mod.PostSetupContent()`, `Mod.AddRecipeGroups()`, and `Mod.PostAddRecipes()`.
		/// </summary>
		void OnPostModsLoad();
		/// <summary>
		/// Called during `Mod.Unload()`.
		/// </summary>
		void OnModsUnload();
	}
}
