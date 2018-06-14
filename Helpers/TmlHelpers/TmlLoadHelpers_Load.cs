using HamstarHelpers.DebugHelpers;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.TmlHelpers {
	public partial class TmlLoadHelpers {
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
		}

		~TmlLoadHelpers() {
			try {
				if( this.WorldLoadPromiseConditionsMet && !this.WorldUnloadPromiseConditionsMet ) {
					this.FulfillWorldUnloadPromises();
				}

				Main.OnTick -= TmlLoadHelpers._Update;
			} catch { }
		}


		internal void OnPostSetupContent() {
			TmlLoadHelpers.AddWorldLoadEachPromise( () => {
				this.WorldUnloadPromiseConditionsMet = false;
			} );
		}


		////////////////

		internal void PreSaveAndExit() {
			this.FulfillWorldUnloadPromises();
		}


		////////////////

		private static void _Update() { // <- Just in case references are doing something funky...
			HamstarHelpersMod mymod = HamstarHelpersMod.Instance;
			if( mymod == null ) { return; }

			mymod.TmlLoadHelpers.Update();
		}

		private void Update() {
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
