using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.LoadHooks;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Helpers.Players {
	/// @private
	public partial class PlayerIdentityHelpers {
		internal IDictionary<int, string> PlayerIds = new Dictionary<int, string>();



		////////////////

		internal void OnPostSetupContent() {
			LoadHooks.AddPostWorldUnloadEachHook( () => {
				this.PlayerIds = new Dictionary<int, string>();
			} );
		}
	}
}
