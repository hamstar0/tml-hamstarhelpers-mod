using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using Terraria;


namespace HamstarHelpers.Services.Promises {
	public partial class Promises {
		public static void AddPostModLoadPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.PostModLoadPromiseConditionsMet ) {
				action();
			} else {
				lock( Promises.PostModLoadLock ) {
					mymod.Promises.PostModLoadPromises.Add( action );
				}
			}
		}

		public static void AddModUnloadPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			lock( Promises.ModUnloadLock ) {
				mymod.Promises.ModUnloadPromises.Add( action );
			}
		}


		////////////////

		public static void AddWorldLoadOncePromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;
			
			if( mymod.Promises.WorldLoadPromiseConditionsMet ) {
				action();
			} else {
				lock( Promises.WorldLoadOnceLock ) {
					mymod.Promises.WorldLoadOncePromises.Add( action );
				}
			}
		}

		public static void AddPostWorldLoadOncePromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.WorldLoadPromiseConditionsMet ) {
				action();
			} else {
				lock( Promises.PostWorldLoadOnceLock ) {
					mymod.Promises.PostWorldLoadOncePromises.Add( action );
				}
			}
		}

		public static void AddWorldUnloadOncePromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.WorldUnloadPromiseConditionsMet ) {
				action();
			} else {
				lock( Promises.WorldUnloadOnceLock ) {
					mymod.Promises.WorldUnloadOncePromises.Add( action );
				}
			}
		}

		public static void AddPostWorldUnloadOncePromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.PostWorldUnloadPromiseConditionsMet ) {
				action();
			} else {
				lock( Promises.PostWorldUnloadOnceLock ) {
					mymod.Promises.PostWorldUnloadOncePromises.Add( action );
				}
			}
		}

		public static void AddWorldInPlayOncePromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;
			
			if( mymod.Promises.WorldInPlayPromiseConditionsMet ) {
				action();
			} else {
				lock( Promises.WorldInPlayOnceLock ) {
					mymod.Promises.WorldInPlayOncePromises.Add( action );
				}
			}
		}

		public static void AddSafeWorldLoadOncePromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.SafeWorldLoadPromiseConditionsMet ) {
				action();
			} else {
				lock( Promises.SafeWorldLoadOnceLock ) {
					mymod.Promises.SafeWorldLoadOncePromises.Add( action );
				}
			}
		}

		public static void AddCurrentPlayerLoadOncePromise( Action action ) {
			if( Main.dedServ || Main.netMode == 2 ) {
				throw new HamstarException( "Not for servers." );
			}

			var mymod = HamstarHelpersMod.Instance;
			
			if( mymod.Promises.CurrentPlayerLoadPromiseConditionsMet ) {
				action();
			} else {
				lock( Promises.CurrentPlayerLoadOnceLock ) {
					mymod.Promises.CurrentPlayerLoadOncePromises.Add( action );
				}
			}
		}


		////////////////

		public static void AddWorldLoadEachPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;
			
			if( mymod.Promises.WorldLoadPromiseConditionsMet ) {
				action();
			}
			lock( Promises.WorldLoadEachLock ) {
				mymod.Promises.WorldLoadEachPromises.Add( action );
			}
		}

		public static void AddPostWorldLoadEachPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.WorldLoadPromiseConditionsMet ) {
				action();
			}
			lock( Promises.PostWorldLoadEachLock ) {
				mymod.Promises.PostWorldLoadEachPromises.Add( action );
			}
		}

		public static void AddWorldUnloadEachPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.WorldUnloadPromiseConditionsMet ) {
				action();
			}
			lock( Promises.WorldUnloadEachLock ) {
				mymod.Promises.WorldUnloadEachPromises.Add( action );
			}
		}

		public static void AddPostWorldUnloadEachPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.PostWorldUnloadPromiseConditionsMet ) {
				action();
			}
			lock( Promises.PostWorldUnloadEachLock ) {
				mymod.Promises.PostWorldUnloadEachPromises.Add( action );
			}
		}

		public static void AddWorldInPlayEachPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;
			
			if( mymod.Promises.WorldInPlayPromiseConditionsMet ) {
				action();
			}
			lock( Promises.WorldInPlayEachLock ) {
				mymod.Promises.WorldInPlayEachPromises.Add( action );
			}
		}

		public static void AddSafeWorldLoadEachPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.SafeWorldLoadPromiseConditionsMet ) {
				action();
			}
			lock( Promises.SafeWorldLoadEachLock ) {
				mymod.Promises.SafeWorldLoadEachPromises.Add( action );
			}
		}

		public static void AddCurrentPlayerLoadEachPromise( Action action ) {
			if( Main.dedServ || Main.netMode == 2 ) {
				throw new HamstarException( "Not for servers." );
			}

			var mymod = HamstarHelpersMod.Instance;
			var myplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>();

			if( mymod.Promises.CurrentPlayerLoadPromiseConditionsMet ) {
				action();
			}
			lock( Promises.CurrentPlayerLoadEachLock ) {
				mymod.Promises.CurrentPlayerLoadEachPromises.Add( action );
			}
		}
	}
}
