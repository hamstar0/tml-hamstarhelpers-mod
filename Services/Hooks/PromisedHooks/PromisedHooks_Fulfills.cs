using HamstarHelpers.Helpers.Debug;
using System;
using System.Linq;


namespace HamstarHelpers.Services.PromisedHooks {
	public partial class PromisedHooks {
		internal void FulfillPostModLoadPromises() {
			if( this.PostModLoadPromiseConditionsMet ) { return; }
			this.PostModLoadPromiseConditionsMet = true;

			Action[] promises;

			lock( PromisedHooks.PostModLoadLock ) {
				promises = this.PostModLoadPromises.ToArray();
				this.PostModLoadPromises.Clear();
			}

			foreach( Action promise in promises ) {
				promise();
			}
		}

		internal void FulfillModUnloadPromises() {
			Action[] promises;

			lock( PromisedHooks.ModUnloadLock ) {
				promises = this.ModUnloadPromises.ToArray();
				this.ModUnloadPromises.Clear();
			}
			
			foreach( Action promise in promises ) {
				promise();
			}
		}


		internal void FulfillWorldLoadPromises() {
			if( this.WorldLoadPromiseConditionsMet ) { return; }
			this.WorldLoadPromiseConditionsMet = true;

			Action[] worldLoadOncePromises;
			Action[] worldLoadEachPromises;
			Action[] postWorldLoadOncePromises;
			Action[] postWorldLoadEachPromises;

			lock( PromisedHooks.WorldLoadOnceLock ) {
				worldLoadOncePromises = this.WorldLoadOncePromises.ToArray();
				this.WorldLoadOncePromises.Clear();
			}
			lock( PromisedHooks.WorldLoadEachLock ) {
				worldLoadEachPromises = this.WorldLoadEachPromises.ToArray();
			}
			lock( PromisedHooks.PostWorldLoadOnceLock ) {
				postWorldLoadOncePromises = this.PostWorldLoadOncePromises.ToArray();
				this.PostWorldLoadOncePromises.Clear();
			}
			lock( PromisedHooks.PostWorldLoadEachLock ) {
				postWorldLoadEachPromises = this.PostWorldLoadEachPromises.ToArray();
			}

			foreach( Action promise in worldLoadOncePromises ) {
				promise();
			}
			foreach( Action promise in worldLoadEachPromises ) {
				promise();
			}
			foreach( Action promise in postWorldLoadOncePromises ) {
				promise();
			}
			foreach( Action promise in postWorldLoadEachPromises ) {
				promise();
			}
		}


		internal void FulfillWorldInPlayPromises() {
			if( this.WorldInPlayPromiseConditionsMet ) { return; }
			this.WorldInPlayPromiseConditionsMet = true;

			Action[] inPlayOncePromises;
			Action[] inPlayEachPromises;

			lock( PromisedHooks.WorldInPlayOnceLock ) {
				inPlayOncePromises = this.WorldInPlayOncePromises.ToArray();
				this.WorldInPlayOncePromises.Clear();
			}
			lock( PromisedHooks.WorldInPlayEachLock ) {
				inPlayEachPromises = this.WorldInPlayEachPromises.ToArray();
			}

			foreach( Action promise in inPlayOncePromises ) {
				promise();
			}
			foreach( Action promise in inPlayEachPromises ) {
				promise();
			}
		}


		internal void FulfillSafeWorldLoadPromises() {
			if( this.SafeWorldLoadPromiseConditionsMet ) { return; }
			this.SafeWorldLoadPromiseConditionsMet = true;

			Action[] safeWorldLoadOncePromises;
			Action[] safeWorldLoadEachPromises;

			lock( PromisedHooks.SafeWorldLoadOnceLock ) {
				safeWorldLoadOncePromises = this.SafeWorldLoadOncePromises.ToArray();
				this.SafeWorldLoadOncePromises.Clear();
			}
			lock( PromisedHooks.SafeWorldLoadEachLock ) {
				safeWorldLoadEachPromises = this.SafeWorldLoadEachPromises.ToArray();
			}

			foreach( Action promise in safeWorldLoadOncePromises ) {
				promise();
			}
			foreach( Action promise in safeWorldLoadEachPromises ) {
				promise();
			}
		}


		internal void FulfillWorldUnloadPromises() {
			if( this.WorldUnloadPromiseConditionsMet ) { return; }
			this.WorldUnloadPromiseConditionsMet = true;

			Action[] worldUnloadOncePromises;
			Action[] worldUnloadEachPromises;

			lock( PromisedHooks.WorldUnloadOnceLock ) {
				worldUnloadOncePromises = this.WorldUnloadOncePromises.ToArray();
				this.WorldUnloadOncePromises.Clear();
			}
			lock( PromisedHooks.WorldUnloadEachLock ) {
				worldUnloadEachPromises = this.WorldUnloadEachPromises.ToArray();
			}

			foreach( Action promise in worldUnloadOncePromises ) {
				promise();
			}
			foreach( Action promise in worldUnloadEachPromises ) {
				promise();
			}
		}


		internal void FulfillPostWorldUnloadPromises() {
			if( this.PostWorldUnloadPromiseConditionsMet ) { return; }
			this.PostWorldUnloadPromiseConditionsMet = true;

			Action[] postWorldUnloadOncePromises;
			Action[] postWorldUnloadEachPromises;

			lock( PromisedHooks.PostWorldUnloadOnceLock ) {
				postWorldUnloadOncePromises = this.PostWorldUnloadOncePromises.ToArray();
				this.PostWorldUnloadOncePromises.Clear();
			}
			lock( PromisedHooks.PostWorldUnloadEachLock ) {
				postWorldUnloadEachPromises = this.PostWorldUnloadEachPromises.ToArray();
			}

			foreach( Action promise in postWorldUnloadOncePromises ) {
				promise();
			}
			foreach( Action promise in postWorldUnloadEachPromises ) {
				promise();
			}
		}
	}
}
