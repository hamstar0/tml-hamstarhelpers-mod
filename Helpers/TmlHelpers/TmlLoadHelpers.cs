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
			//var mymod = HamstarHelpersMod.Instance;
			//var modworld = mymod.GetModWorld<HamstarHelpersWorld>();
			//var myplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>();
			//LogHelpers.Log( "HasSetupContent: "+ mymod.HasSetupContent + ", HasCorrectID: "+ modworld.HasCorrectID+ ", who: "+myplayer.player.whoAmI+", HasSyncedModSettings: "+ myplayer.Logic.HasSyncedModSettings + ", HasSyncedModData: " + myplayer.Logic.HasSyncedModData );
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

		public static void AddPostWorldLoadEachPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.TmlLoadHelpers.WorldLoadPromiseConditionsMet ) {
				action();
			}
			mymod.TmlLoadHelpers.PostWorldLoadEachPromises.Add( action );
		}



		////////////////

		internal IList<Action> PostGameLoadPromises = new List<Action>();
		internal IList<Action> PostModLoadPromises = new List<Action>();
		internal IList<Action> ModUnloadPromises = new List<Action>();
		internal IList<Action> WorldLoadOncePromises = new List<Action>();
		internal IList<Action> WorldLoadEachPromises = new List<Action>();
		internal IList<Action> PostWorldLoadEachPromises = new List<Action>();

		internal static bool PostGameLoadPromiseConditionsMet = false;
		internal bool PostModLoadPromiseConditionsMet = false;
		internal bool WorldLoadPromiseConditionsMet = false;

		internal int StartupDelay = 0;

		internal bool IsClientPlaying = false;
		internal bool HasServerBegunHavingPlayers = false;



		////////////////

		internal void FulfillPostGameLoadPromises() {
			TmlLoadHelpers.PostGameLoadPromiseConditionsMet = true;
			
			foreach( Action promise in this.PostGameLoadPromises ) {
				promise();
			}
			this.PostGameLoadPromises.Clear();
		}

		internal void FulfillPostModLoadPromises() {
			this.FulfillPostGameLoadPromises();

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
			foreach( Action promise in this.PostWorldLoadEachPromises ) {
				promise();
			}
			this.WorldLoadOncePromises.Clear();
			this.PostWorldLoadEachPromises.Clear();
		}


		////////////////

		internal TmlLoadHelpers() {
			Main.OnTick += TmlLoadHelpers._Update;
		}

		~TmlLoadHelpers() {
			//internal void Unload() {
			try {
				Main.OnTick -= TmlLoadHelpers._Update;
			} catch { }
		}

		////////////////

		private static void _Update() { // <- Just in case references are doing something funky...
			HamstarHelpersMod mymod = HamstarHelpersMod.Instance;
			if( mymod == null ) { return; }

			mymod.TmlLoadHelpers.Update();
		}

		internal void Update() {
			if( this.WorldLoadPromiseConditionsMet && Main.gameMenu ) {
				this.WorldLoadPromiseConditionsMet = false; // Does this work?
			}
		}

		internal void PostWorldLoadUpdate() {
			this.StartupDelay++;    // Seems needed for day/night tracking (and possibly other things?)
		}
	}
}
