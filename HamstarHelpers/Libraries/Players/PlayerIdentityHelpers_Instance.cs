using System;
using System.Collections.Generic;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Services.Hooks.LoadHooks;


namespace HamstarHelpers.Libraries.Players {
	/// @private
	public partial class PlayerIdentityLibraries {
		internal IDictionary<int, string> PlayerIds = new Dictionary<int, string>();



		////////////////

		internal void OnPostModsLoad() {
			LoadHooks.AddPostWorldUnloadEachHook( () => {
				this.PlayerIds = new Dictionary<int, string>();
			} );
		}
	}
}
