using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Promises;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.Players {
	public partial class PlayerIdentityHelpers {
		internal IDictionary<int, string> PlayerIds = new Dictionary<int, string>();



		////////////////

		internal void OnPostSetupContent() {
			Promises.AddPostWorldUnloadEachPromise( () => {
				this.PlayerIds = new Dictionary<int, string>();
			} );
		}
	}
}
