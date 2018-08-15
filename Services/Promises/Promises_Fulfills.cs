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

			Action[] world_load_once_promises;
			Action[] world_load_each_promises;
			Action[] post_world_load_once_promises;
			Action[] post_world_load_each_promises;

			lock( Promises.WorldLoadOnceLock ) {
				world_load_once_promises = this.WorldLoadOncePromises.ToArray();
				this.WorldLoadOncePromises.Clear();
			}
			lock( Promises.WorldLoadEachLock ) {
				world_load_each_promises = this.WorldLoadEachPromises.ToArray();
			}
			lock( Promises.PostWorldLoadOnceLock ) {
				post_world_load_once_promises = this.PostWorldLoadOncePromises.ToArray();
				this.PostWorldLoadOncePromises.Clear();
			}
			lock( Promises.PostWorldLoadEachLock ) {
				post_world_load_each_promises = this.PostWorldLoadEachPromises.ToArray();
			}

			foreach( Action promise in world_load_once_promises ) {
				promise();
			}
			foreach( Action promise in world_load_each_promises ) {
				promise();
			}
			foreach( Action promise in post_world_load_once_promises ) {
				promise();
			}
			foreach( Action promise in post_world_load_each_promises ) {
				promise();
			}
		}


		internal void FulfillWorldUnloadPromises() {
			if( this.WorldUnloadPromiseConditionsMet ) { return; }
			this.WorldUnloadPromiseConditionsMet = true;

			Action[] world_unload_once_promises;
			Action[] world_unload_each_promises;

			lock( Promises.WorldUnloadOnceLock ) {
				world_unload_once_promises = this.WorldUnloadOncePromises.ToArray();
				this.WorldUnloadOncePromises.Clear();
			}
			lock( Promises.WorldUnloadEachLock ) {
				world_unload_each_promises = this.WorldUnloadEachPromises.ToArray();
			}

			foreach( Action promise in world_unload_once_promises ) {
				promise();
			}
			foreach( Action promise in world_unload_each_promises ) {
				promise();
			}
		}


		internal void FulfillPostWorldUnloadPromises() {
			if( this.PostWorldUnloadPromiseConditionsMet ) { return; }
			this.PostWorldUnloadPromiseConditionsMet = true;

			Action[] post_world_unload_once_promises;
			Action[] post_world_unload_each_promises;

			lock( Promises.PostWorldUnloadOnceLock ) {
				post_world_unload_once_promises = this.PostWorldUnloadOncePromises.ToArray();
				this.PostWorldUnloadOncePromises.Clear();
			}
			lock( Promises.PostWorldUnloadEachLock ) {
				post_world_unload_each_promises = this.PostWorldUnloadEachPromises.ToArray();
			}
			
			foreach( Action promise in post_world_unload_once_promises ) {
				promise();
			}
			foreach( Action promise in post_world_unload_each_promises ) {
				promise();
			}
		}


		internal void FulfillWorldInPlayPromises() {
			if( this.WorldInPlayPromiseConditionsMet ) { return; }
			this.WorldInPlayPromiseConditionsMet = true;

			Action[] in_play_once_promises;
			Action[] in_play_each_promises;

			lock( Promises.WorldInPlayOnceLock ) {
				in_play_once_promises = this.WorldInPlayOncePromises.ToArray();
				this.WorldInPlayOncePromises.Clear();
			}
			lock( Promises.WorldInPlayEachLock ) {
				in_play_each_promises = this.WorldInPlayEachPromises.ToArray();
			}

			foreach( Action promise in in_play_once_promises ) {
				promise();
			}
			foreach( Action promise in in_play_each_promises ) {
				promise();
			}
		}


		internal void FulfillSafeWorldLoadPromises() {
			if( this.SafeWorldLoadPromiseConditionsMet ) { return; }
			this.SafeWorldLoadPromiseConditionsMet = true;

			Action[] safe_world_load_once_promises;
			Action[] safe_world_load_each_promises;

			lock( Promises.SafeWorldLoadOnceLock ) {
				safe_world_load_once_promises = this.SafeWorldLoadOncePromises.ToArray();
				this.SafeWorldLoadOncePromises.Clear();
			}
			lock( Promises.SafeWorldLoadEachLock ) {
				safe_world_load_each_promises = this.SafeWorldLoadEachPromises.ToArray();
			}

			foreach( Action promise in safe_world_load_once_promises ) {
				promise();
			}
			foreach( Action promise in safe_world_load_each_promises ) {
				promise();
			}
		}
		

		internal void FulfillCurrentPlayerLoadPromises() {
			if( this.CurrentPlayerLoadPromiseConditionsMet ) { return; }
			this.CurrentPlayerLoadPromiseConditionsMet = true;

			Action[] current_player_load_once_promises;
			Action[] current_player_load_each_promises;

			lock( Promises.CurrentPlayerLoadOnceLock ) {
				current_player_load_once_promises = this.CurrentPlayerLoadOncePromises.ToArray();
				this.CurrentPlayerLoadOncePromises.Clear();
			}
			lock( Promises.CurrentPlayerLoadEachLock ) {
				current_player_load_each_promises = this.CurrentPlayerLoadEachPromises.ToArray();
			}

			foreach( Action promise in current_player_load_once_promises ) {
				promise();
			}
			foreach( Action promise in current_player_load_each_promises ) {
				promise();
			}
		}
	}
}
