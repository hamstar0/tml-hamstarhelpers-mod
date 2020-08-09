using System;
using System.Collections.Generic;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Hooks.LoadHooks;


namespace HamstarHelpers.Helpers.Players {
	/// @private
	public partial class PlayerIdentityHelpers {
		internal IDictionary<int, string> PlayerIds = new Dictionary<int, string>();



		////////////////

		internal void OnPostModsLoad() {
			LoadHooks.AddPostWorldUnloadEachHook( () => {
				this.PlayerIds = new Dictionary<int, string>();
			} );
		}
	}
}
