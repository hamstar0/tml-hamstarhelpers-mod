using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.TmlHelpers {
	public class TmlLoadHelpers {
		[System.Obsolete( "use TmlLoadHelpers.IsModLoaded", true )]
		public static bool IsLoaded() {
			return TmlLoadHelpers.IsModLoaded();
		}

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

			if( Main.netMode == 0 || Main.netMode == 1 ) {	// Client or single
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

		[System.Obsolete( "use TmlLoadHelpers.AddPostModLoadPromise", true )]
		public static void AddPostLoadPromise( Action action ) {
			TmlLoadHelpers.AddPostModLoadPromise( action );
		}

		public static void AddPostModLoadPromise( Action action ) {
				var mymod = HamstarHelpersMod.Instance;

			if( mymod.TmlLoadHelpers.PostModLoadPromiseConditionsMet ) {
				action();
			} else {
				mymod.TmlLoadHelpers.PostModLoadPromises.Add( action );
			}
		}



		////////////////

		internal IList<Action> PostModLoadPromises = new List<Action>();
		internal bool PostModLoadPromiseConditionsMet = false;

		internal int StartupDelay = 0;

		internal bool IsClientPlaying = false;
		internal bool HasServerBegunHavingPlayers = false;


		////////////////

		internal void FulfillPostModLoadPromises() {
			foreach( Action promise in this.PostModLoadPromises ) {
				promise();
			}

			this.PostModLoadPromises.Clear();
			this.PostModLoadPromiseConditionsMet = true;
		}


		////////////////
		
		internal void Update() {
			this.StartupDelay++;    // Seems needed for day/night tracking (and possibly other things?)
		}
	}
}
