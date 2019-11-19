using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Helpers.Draw {
	class DrawHelpersInternal : ILoadable {
		internal ISet<Func<bool>> PostDrawTilesActions = new HashSet<Func<bool>>();



		////////////////

		/// @ private
		void ILoadable.OnModsLoad() { }
		/// @ private
		void ILoadable.OnModsUnload() { }
		/// @ private
		void ILoadable.OnPostModsLoad() { }


		////////////////

		internal void RunPostDrawTilesActions() {
			foreach( Func<bool> action in this.PostDrawTilesActions.ToArray() ) {
				if( !action() ) {
					this.PostDrawTilesActions.Remove( action );
				}
			}
		}
	}
}
