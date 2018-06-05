using HamstarHelpers.DebugHelpers;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.TmlHelpers {
	public partial class TmlLoadHelpers {
		public static bool IsModLoaded() {
			var mymod = HamstarHelpersMod.Instance;

			if( !mymod.HasSetupContent ) { return false; }
			if( !mymod.HasAddedRecipeGroups ) { return false; }
			if( !mymod.HasAddedRecipes ) { return false; }

			return true;
		}


		public static bool IsWorldLoaded() {
			if( !TmlLoadHelpers.IsModLoaded() ) { return false; }

			var mymod = HamstarHelpersMod.Instance;
			var myworld = mymod.GetModWorld<HamstarHelpersWorld>();
			if( !myworld.HasCorrectID ) { return false; }

			return true;
		}


		public static bool IsWorldBeingPlayed() {
			if( !TmlLoadHelpers.IsWorldLoaded() ) { return false; }

			var mymod = HamstarHelpersMod.Instance;

			if( Main.netMode == 0 || Main.netMode == 1 ) {  // Client or single
				if( !mymod.TmlLoadHelpers.IsClientPlaying ) {
					return false;
				}

				var myplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>();
				return myplayer.Logic.IsSynced();
			} else {  // Server
				if( !mymod.TmlLoadHelpers.HasServerBegunHavingPlayers ) {
					return false;
				}

				return true;
			}
		}


		public static bool IsWorldSafelyBeingPlayed() {
			return HamstarHelpersMod.Instance.TmlLoadHelpers.StartupDelay >= ( 60 * 2 );
		}


		////////////////
		
		public static void AddPostModLoadPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.TmlLoadHelpers.PostModLoadPromiseConditionsMet ) {
				action();
			} else {
				mymod.TmlLoadHelpers.PostModLoadPromises.Add( action );
			}
		}

		public static void AddModUnloadPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			mymod.TmlLoadHelpers.ModUnloadPromises.Add( action );
		}


		public static void AddWorldLoadOncePromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;
			
			if( mymod.TmlLoadHelpers.WorldLoadPromiseConditionsMet ) {
				action();
			} else {
				mymod.TmlLoadHelpers.WorldLoadOncePromises.Add( action );
			}
		}
		
		public static void AddWorldLoadEachPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.TmlLoadHelpers.WorldLoadPromiseConditionsMet ) {
				action();
			}
			mymod.TmlLoadHelpers.WorldLoadEachPromises.Add( action );
		}

		public static void AddPostWorldLoadOncePromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.TmlLoadHelpers.WorldLoadPromiseConditionsMet ) {
				action();
			}
			mymod.TmlLoadHelpers.PostWorldLoadOncePromises.Add( action );
		}

		public static void AddPostWorldLoadEachPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.TmlLoadHelpers.WorldLoadPromiseConditionsMet ) {
				action();
			}
			mymod.TmlLoadHelpers.PostWorldLoadEachPromises.Add( action );
		}

		public static void AddWorldUnloadOncePromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.TmlLoadHelpers.WorldUnloadPromiseConditionsMet ) {
				action();
			}
			mymod.TmlLoadHelpers.WorldUnloadOncePromises.Add( action );
		}

		public static void AddWorldUnloadEachPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.TmlLoadHelpers.WorldUnloadPromiseConditionsMet ) {
				action();
			}
			mymod.TmlLoadHelpers.WorldUnloadEachPromises.Add( action );
		}

		////////////////

		public static void AddCustomPromise( string name, Func<bool> action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.TmlLoadHelpers.CustomPromiseConditionsMet.Contains(name) ) {
				if( !action() ) {
					return;
				}
			}

			if( !mymod.TmlLoadHelpers.CustomPromise.ContainsKey(name) ) {
				mymod.TmlLoadHelpers.CustomPromise[ name ] = new List<Func<bool>>();
			}
			mymod.TmlLoadHelpers.CustomPromise[ name ].Add( action );
		}

		public static void TriggerCustomPromise( string name ) {
			var mymod = HamstarHelpersMod.Instance;

			mymod.TmlLoadHelpers.CustomPromiseConditionsMet.Add( name );

			if( mymod.TmlLoadHelpers.CustomPromise.ContainsKey(name) ) {
				var func_list = mymod.TmlLoadHelpers.CustomPromise[ name ];

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

			mymod.TmlLoadHelpers.CustomPromiseConditionsMet.Remove( name );

			if( mymod.TmlLoadHelpers.CustomPromise.ContainsKey( name ) ) {
				mymod.TmlLoadHelpers.CustomPromise.Remove( name );
			}
		}



		////////////////

		private IList<Action> PostModLoadPromises = new List<Action>();
		private IList<Action> ModUnloadPromises = new List<Action>();
		private IList<Action> WorldLoadOncePromises = new List<Action>();
		private IList<Action> WorldLoadEachPromises = new List<Action>();
		private IList<Action> PostWorldLoadOncePromises = new List<Action>();
		private IList<Action> PostWorldLoadEachPromises = new List<Action>(); 
		private IList<Action> WorldUnloadOncePromises = new List<Action>();
		private IList<Action> WorldUnloadEachPromises = new List<Action>();
		private IDictionary<string, List<Func<bool>>> CustomPromise = new Dictionary<string, List<Func<bool>>>();

		private bool PostModLoadPromiseConditionsMet = false;
		private bool WorldLoadPromiseConditionsMet = false;
		private bool WorldUnloadPromiseConditionsMet = false;
		private ISet<string> CustomPromiseConditionsMet = new HashSet<string>();

		private int StartupDelay = 0;

		internal bool IsClientPlaying = false;
		internal bool HasServerBegunHavingPlayers = false;



		////////////////
		
		internal void FulfillPostModLoadPromises() {
			if( this.PostModLoadPromiseConditionsMet ) { return; }
			this.PostModLoadPromiseConditionsMet = true;
			
			foreach( Action promise in this.PostModLoadPromises ) {
				promise();
			}
			this.PostModLoadPromises.Clear();
		}

		internal void FulfillModUnloadPromises() {
			foreach( Action promise in this.ModUnloadPromises ) {
				promise();
			}
			this.ModUnloadPromises.Clear();
		}


		internal void FulfillWorldLoadPromises() {
			if( this.WorldLoadPromiseConditionsMet ) { return; }
			this.WorldLoadPromiseConditionsMet = true;
			
			foreach( Action promise in this.WorldLoadOncePromises ) {
				promise();
			}
			foreach( Action promise in this.WorldLoadEachPromises ) {
				promise();
			}
			
			foreach( Action promise in this.PostWorldLoadOncePromises ) {
				promise();
			}
			foreach( Action promise in this.PostWorldLoadEachPromises ) {
				promise();
			}

			this.WorldLoadOncePromises.Clear();
			this.PostWorldLoadOncePromises.Clear();
		}

		internal void FulfillWorldUnloadPromises() {
			if( this.WorldUnloadPromiseConditionsMet ) { return; }
			this.WorldUnloadPromiseConditionsMet = true;

			foreach( Action promise in this.WorldUnloadOncePromises ) {
				promise();
			}
			foreach( Action promise in this.WorldUnloadEachPromises ) {
				promise();
			}

			this.WorldUnloadOncePromises.Clear();
		}


		////////////////

		internal TmlLoadHelpers() {
			Main.OnTick += TmlLoadHelpers._Update;

			TmlLoadHelpers.AddWorldLoadEachPromise( () => {
				this.WorldUnloadPromiseConditionsMet = false;
			} );
		}

		~TmlLoadHelpers() {
			//internal void Unload() {
			try {
				Main.OnTick -= TmlLoadHelpers._Update;
			} catch { }
		}


		////////////////

		internal void OnWorldExit() {
			this.FulfillWorldUnloadPromises();
		}


		////////////////

		private static void _Update() { // <- Just in case references are doing something funky...
			HamstarHelpersMod mymod = HamstarHelpersMod.Instance;
			if( mymod == null ) { return; }

			mymod.TmlLoadHelpers.Update();
		}

		internal void Update() {
			if( Main.netMode != 2 ) {
				if( this.WorldLoadPromiseConditionsMet && Main.gameMenu ) {
					this.WorldLoadPromiseConditionsMet = false; // Does this work?
				}
			}
		}

		internal void PostWorldLoadUpdate() {
			this.StartupDelay++;    // Seems needed for day/night tracking (and possibly other things?)
		}
	}
}
