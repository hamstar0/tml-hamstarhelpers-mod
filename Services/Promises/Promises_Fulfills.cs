using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Linq;


namespace HamstarHelpers.Services.Promises {
	public partial class Promises {
		internal void FulfillPostModLoadPromises() {
			if( this.PostModLoadPromiseConditionsMet ) { return; }
			this.PostModLoadPromiseConditionsMet = true;

			Action[] promises;

			lock( Promises.PostModLoadLock ) {
				promises = this.PostModLoadPromises.ToArray();
				this.PostModLoadPromises.Clear();
			}

			foreach( Action promise in promises ) {
				promise();
			}
		}

		internal void FulfillModUnloadPromises() {
			Action[] promises;

			lock( Promises.ModUnloadLock ) {
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

			lock( Promises.WorldLoadOnceLock ) {
				worldLoadOncePromises = this.WorldLoadOncePromises.ToArray();
				this.WorldLoadOncePromises.Clear();
			}
			lock( Promises.WorldLoadEachLock ) {
				worldLoadEachPromises = this.WorldLoadEachPromises.ToArray();
			}
			lock( Promises.PostWorldLoadOnceLock ) {
				postWorldLoadOncePromises = this.PostWorldLoadOncePromises.ToArray();
				this.PostWorldLoadOncePromises.Clear();
			}
			lock( Promises.PostWorldLoadEachLock ) {
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

			lock( Promises.WorldInPlayOnceLock ) {
				inPlayOncePromises = this.WorldInPlayOncePromises.ToArray();
				this.WorldInPlayOncePromises.Clear();
			}
			lock( Promises.WorldInPlayEachLock ) {
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

			lock( Promises.SafeWorldLoadOnceLock ) {
				safeWorldLoadOncePromises = this.SafeWorldLoadOncePromises.ToArray();
				this.SafeWorldLoadOncePromises.Clear();
			}
			lock( Promises.SafeWorldLoadEachLock ) {
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

			lock( Promises.WorldUnloadOnceLock ) {
				worldUnloadOncePromises = this.WorldUnloadOncePromises.ToArray();
				this.WorldUnloadOncePromises.Clear();
			}
			lock( Promises.WorldUnloadEachLock ) {
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

			lock( Promises.PostWorldUnloadOnceLock ) {
				postWorldUnloadOncePromises = this.PostWorldUnloadOncePromises.ToArray();
				this.PostWorldUnloadOncePromises.Clear();
			}
			lock( Promises.PostWorldUnloadEachLock ) {
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
