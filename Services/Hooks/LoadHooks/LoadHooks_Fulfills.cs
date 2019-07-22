using HamstarHelpers.Helpers.Debug;
using System;
using System.Linq;


namespace HamstarHelpers.Services.Hooks.LoadHooks {
	/// <summary>
	/// Allows defining load hooks. These are of a set of presets corresponding to important game code "load" events.
	/// </summary>
	public partial class LoadHooks {
		internal void FulfillPostModLoadHooks() {
			if( this.PostModLoadHookConditionsMet ) { return; }
			this.PostModLoadHookConditionsMet = true;

			Action[] hooks;

			lock( LoadHooks.PostModLoadHookLock ) {
				hooks = this.PostModLoadHooks.ToArray();
				this.PostModLoadHooks.Clear();
			}

			foreach( Action hook in hooks ) {
				hook();
			}
		}

		internal void FulfillModUnloadHooks() {
			Action[] hooks;

			lock( LoadHooks.ModUnloadHookLock ) {
				hooks = this.ModUnloadHooks.ToArray();
				this.ModUnloadHooks.Clear();
			}

			foreach( Action hook in hooks ) {
				hook();
			}
		}


		internal void FulfillWorldLoadHooks() {
			if( this.WorldLoadHookConditionsMet ) { return; }
			this.WorldLoadHookConditionsMet = true;

			Action[] worldLoadOnceHooks;
			Action[] worldLoadEachHooks;
			Action[] postWorldLoadOnceHooks;
			Action[] postWorldLoadEachHooks;

			lock( LoadHooks.WorldLoadOnceHookLock ) {
				worldLoadOnceHooks = this.WorldLoadOnceHooks.ToArray();
				this.WorldLoadOnceHooks.Clear();
			}
			lock( LoadHooks.WorldLoadEachHookLock ) {
				worldLoadEachHooks = this.WorldLoadEachHooks.ToArray();
			}
			lock( LoadHooks.PostWorldLoadOnceHookLock ) {
				postWorldLoadOnceHooks = this.PostWorldLoadOnceHooks.ToArray();
				this.PostWorldLoadOnceHooks.Clear();
			}
			lock( LoadHooks.PostWorldLoadEachHookLock ) {
				postWorldLoadEachHooks = this.PostWorldLoadEachHooks.ToArray();
			}

			foreach( Action hook in worldLoadOnceHooks ) {
				hook();
			}
			foreach( Action hook in worldLoadEachHooks ) {
				hook();
			}
			foreach( Action hook in postWorldLoadOnceHooks ) {
				hook();
			}
			foreach( Action hook in postWorldLoadEachHooks ) {
				hook();
			}
		}


		internal void FulfillWorldInPlayHooks() {
			if( this.WorldInPlayHookConditionsMet ) { return; }
			this.WorldInPlayHookConditionsMet = true;

			Action[] inPlayOnceHooks;
			Action[] inPlayEachHooks;

			lock( LoadHooks.WorldInPlayOnceHookLock ) {
				inPlayOnceHooks = this.WorldInPlayOnceHooks.ToArray();
				this.WorldInPlayOnceHooks.Clear();
			}
			lock( LoadHooks.WorldInPlayEachHookLock ) {
				inPlayEachHooks = this.WorldInPlayEachHooks.ToArray();
			}

			foreach( Action hook in inPlayOnceHooks ) {
				hook();
			}
			foreach( Action hook in inPlayEachHooks ) {
				hook();
			}
		}


		internal void FulfillSafeWorldLoadHook() {
			if( this.SafeWorldLoadHookConditionsMet ) { return; }
			this.SafeWorldLoadHookConditionsMet = true;

			Action[] safeWorldLoadOnceHooks;
			Action[] safeWorldLoadEachHooks;

			lock( LoadHooks.SafeWorldLoadOnceHookLock ) {
				safeWorldLoadOnceHooks = this.SafeWorldLoadOnceHooks.ToArray();
				this.SafeWorldLoadOnceHooks.Clear();
			}
			lock( LoadHooks.SafeWorldLoadEachHookLock ) {
				safeWorldLoadEachHooks = this.SafeWorldLoadEachHooks.ToArray();
			}

			foreach( Action hook in safeWorldLoadOnceHooks ) {
				hook();
			}
			foreach( Action hook in safeWorldLoadEachHooks ) {
				hook();
			}
		}


		internal void FulfillWorldUnloadHooks() {
			if( this.WorldUnloadHookConditionsMet ) { return; }
			this.WorldUnloadHookConditionsMet = true;

			Action[] worldUnloadOnceHooks;
			Action[] worldUnloadEachHooks;

			lock( LoadHooks.WorldUnloadOnceHookLock ) {
				worldUnloadOnceHooks = this.WorldUnloadOnceHooks.ToArray();
				this.WorldUnloadOnceHooks.Clear();
			}
			lock( LoadHooks.WorldUnloadEachHookLock ) {
				worldUnloadEachHooks = this.WorldUnloadEachHooks.ToArray();
			}

			foreach( Action hook in worldUnloadOnceHooks ) {
				hook();
			}
			foreach( Action hook in worldUnloadEachHooks ) {
				hook();
			}
		}


		internal void FulfillPostWorldUnloadHooks() {
			if( this.PostWorldUnloadHookConditionsMet ) { return; }
			this.PostWorldUnloadHookConditionsMet = true;

			Action[] postWorldUnloadOnceHooks;
			Action[] postWorldUnloadEachHooks;

			lock( LoadHooks.PostWorldUnloadOnceHookLock ) {
				postWorldUnloadOnceHooks = this.PostWorldUnloadOnceHooks.ToArray();
				this.PostWorldUnloadOnceHooks.Clear();
			}
			lock( LoadHooks.PostWorldUnloadEachHookLock ) {
				postWorldUnloadEachHooks = this.PostWorldUnloadEachHooks.ToArray();
			}

			foreach( Action hook in postWorldUnloadOnceHooks ) {
				hook();
			}
			foreach( Action hook in postWorldUnloadEachHooks ) {
				hook();
			}
		}
	}
}
