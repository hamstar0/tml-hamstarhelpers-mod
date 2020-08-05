using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Services.Hooks.Draw {
	class DrawHooksInternal : ILoadable {
		internal ISet<Func<bool>> PostDrawTilesHooks = new HashSet<Func<bool>>();



		////////////////

		/// @ private
		void ILoadable.OnModsLoad() { }
		/// @ private
		void ILoadable.OnModsUnload() { }
		/// @ private
		void ILoadable.OnPostModsLoad() { }


		////////////////

		internal void RunPostDrawTilesActions() {
			foreach( Func<bool> action in this.PostDrawTilesHooks.ToArray() ) {
				if( !action() ) {
					this.PostDrawTilesHooks.Remove( action );
				}
			}
		}
	}
}
