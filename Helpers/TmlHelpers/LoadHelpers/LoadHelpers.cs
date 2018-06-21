using HamstarHelpers.DebugHelpers;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.TmlHelpers.LoadHelpers {
	public partial class LoadHelpers {
		public static bool IsModLoaded() {
			var mymod = HamstarHelpersMod.Instance;

			if( !mymod.HasSetupContent ) { return false; }
			if( !mymod.HasAddedRecipeGroups ) { return false; }
			if( !mymod.HasAddedRecipes ) { return false; }

			return true;
		}


		public static bool IsWorldLoaded() {
			if( !LoadHelpers.IsModLoaded() ) { return false; }

			var mymod = HamstarHelpersMod.Instance;
			var myworld = mymod.GetModWorld<HamstarHelpersWorld>();
			if( !myworld.HasCorrectID ) { return false; }

			return true;
		}


		public static bool IsWorldBeingPlayed() {
			if( !LoadHelpers.IsWorldLoaded() ) { return false; }

			var mymod = HamstarHelpersMod.Instance;

			if( Main.netMode == 0 || Main.netMode == 1 ) {  // Client or single
				if( !mymod.LoadHelpers.IsClientPlaying ) {
					return false;
				}

				var myplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>();
				return myplayer.Logic.IsSynced();
			} else {  // Server
				if( !mymod.LoadHelpers.HasServerBegunHavingPlayers ) {
					return false;
				}

				return true;
			}
		}


		public static bool IsWorldSafelyBeingPlayed() {
			return HamstarHelpersMod.Instance.LoadHelpers.StartupDelay >= ( 60 * 2 );
		}


		////////////////
		
		public static void AddPostModLoadPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.LoadHelpers.PostModLoadPromiseConditionsMet ) {
				action();
			} else {
				mymod.LoadHelpers.PostModLoadPromises.Add( action );
			}
		}

		public static void AddModUnloadPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			mymod.LoadHelpers.ModUnloadPromises.Add( action );
		}

		////////////////

		public static void AddWorldLoadOncePromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;
			
			if( mymod.LoadHelpers.WorldLoadPromiseConditionsMet ) {
				action();
			} else {
				mymod.LoadHelpers.WorldLoadOncePromises.Add( action );
			}
		}
		
		public static void AddWorldLoadEachPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.LoadHelpers.WorldLoadPromiseConditionsMet ) {
				action();
			}
			mymod.LoadHelpers.WorldLoadEachPromises.Add( action );
		}

		public static void AddPostWorldLoadOncePromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.LoadHelpers.WorldLoadPromiseConditionsMet ) {
				action();
			}
			mymod.LoadHelpers.PostWorldLoadOncePromises.Add( action );
		}

		public static void AddPostWorldLoadEachPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.LoadHelpers.WorldLoadPromiseConditionsMet ) {
				action();
			}
			mymod.LoadHelpers.PostWorldLoadEachPromises.Add( action );
		}

		////////////////

		public static void AddWorldUnloadOncePromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.LoadHelpers.WorldUnloadPromiseConditionsMet ) {
				action();
			}
			mymod.LoadHelpers.WorldUnloadOncePromises.Add( action );
		}

		public static void AddWorldUnloadEachPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.LoadHelpers.WorldUnloadPromiseConditionsMet ) {
				action();
			}
			mymod.LoadHelpers.WorldUnloadEachPromises.Add( action );
		}


		public static void AddPostWorldUnloadOncePromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.LoadHelpers.PostWorldUnloadPromiseConditionsMet ) {
				action();
			}
			mymod.LoadHelpers.PostWorldUnloadOncePromises.Add( action );
		}

		public static void AddPostWorldUnloadEachPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.LoadHelpers.PostWorldUnloadPromiseConditionsMet ) {
				action();
			}
			mymod.LoadHelpers.PostWorldUnloadEachPromises.Add( action );
		}


		////////////////

		public static void AddCustomPromise( string name, Func<bool> action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.LoadHelpers.CustomPromiseConditionsMet.Contains(name) ) {
				if( !action() ) {
					return;
				}
			}

			if( !mymod.LoadHelpers.CustomPromise.ContainsKey(name) ) {
				mymod.LoadHelpers.CustomPromise[ name ] = new List<Func<bool>>();
			}
			mymod.LoadHelpers.CustomPromise[ name ].Add( action );
		}

		public static void TriggerCustomPromise( string name ) {
			var mymod = HamstarHelpersMod.Instance;

			mymod.LoadHelpers.CustomPromiseConditionsMet.Add( name );

			if( mymod.LoadHelpers.CustomPromise.ContainsKey(name) ) {
				var func_list = mymod.LoadHelpers.CustomPromise[ name ];

				for( int i=0; i<func_list.Count; i++ ) {
					if( !func_list[i]() ) {
						func_list.RemoveAt( i );
						i--;
					}
				}
			}
		}

		public static void ClearCustomPromise( string name ) {
			var mymod = HamstarHelpersMod.Instance;

			mymod.LoadHelpers.CustomPromiseConditionsMet.Remove( name );

			if( mymod.LoadHelpers.CustomPromise.ContainsKey( name ) ) {
				mymod.LoadHelpers.CustomPromise.Remove( name );
			}
		}
	}
}
