using System;
using System.Collections.Generic;


namespace HamstarHelpers.TmlHelpers {
	public class TmlLoadHelpers {
		public static bool IsLoaded() {
			var mymod = HamstarHelpersMod.Instance;

			if( !mymod.HasSetupContent ) { return false; }
			if( !mymod.HasAddedRecipeGroups ) { return false; }
			if( !mymod.HasAddedRecipes ) { return false; }

			return true;
		}


		public static void AddPostLoadPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.TmlLoadHelpers.PromiseConditionsMet ) {
				action();
			} else {
				mymod.TmlLoadHelpers.Promises.Add( action );
			}
		}



		////////////////

		internal IList<Action> Promises = new List<Action>();
		internal bool PromiseConditionsMet = false;


		////////////////

		internal void FulfillPromises() {
			foreach( Action promise in this.Promises ) {
				promise();
			}

			this.Promises.Clear();
			this.PromiseConditionsMet = true;
		}
	}
}
