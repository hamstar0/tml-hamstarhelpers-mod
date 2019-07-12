using HamstarHelpers.Helpers.TModLoader;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.Hooks.WorldHooks {
	/// @private
	public partial class WorldTimeHooks {
		private bool IsDay;

		internal IDictionary<string, Action> DayHooks = new Dictionary<string, Action>();
		internal IDictionary<string, Action> NightHooks = new Dictionary<string, Action>();



		////////////////
		
		internal void Update() {
			var mymod = ModHelpersMod.Instance;

			if( !LoadHelpers.IsWorldSafelyBeingPlayed() ) {
				this.IsDay = Main.dayTime;
			} else {
				if( this.IsDay != Main.dayTime ) {
					if( !this.IsDay ) {
						foreach( Action hook in mymod.WorldTimeHooks.DayHooks.Values ) {
							hook();
						}
					} else {
						foreach( Action hook in mymod.WorldTimeHooks.NightHooks.Values ) {
							hook();
						}
					}
				}

				this.IsDay = Main.dayTime;
			}
		}
	}
}
