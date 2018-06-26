using HamstarHelpers.DebugHelpers;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.Promises {
	public partial class Promises {
		internal IList<Action> PostModLoadPromises = new List<Action>();
		internal IList<Action> ModUnloadPromises = new List<Action>();
		internal IList<Action> WorldLoadOncePromises = new List<Action>();
		internal IList<Action> WorldLoadEachPromises = new List<Action>();
		internal IList<Action> PostWorldLoadOncePromises = new List<Action>();
		internal IList<Action> PostWorldLoadEachPromises = new List<Action>();
		internal IList<Action> WorldUnloadOncePromises = new List<Action>();
		internal IList<Action> WorldUnloadEachPromises = new List<Action>();
		internal IList<Action> PostWorldUnloadOncePromises = new List<Action>();
		internal IList<Action> PostWorldUnloadEachPromises = new List<Action>();
		internal IDictionary<string, List<Func<bool>>> CustomPromise = new Dictionary<string, List<Func<bool>>>();

		internal bool PostModLoadPromiseConditionsMet = false;
		internal bool WorldLoadPromiseConditionsMet = false;
		internal bool WorldUnloadPromiseConditionsMet = false;
		internal bool PostWorldUnloadPromiseConditionsMet = false;
		internal ISet<string> CustomPromiseConditionsMet = new HashSet<string>();


		////////////////

		internal Promises() {
			Main.OnTick += Promises._Update;
		}

		~Promises() {
			try {
				Main.OnTick -= Promises._Update;

				if( this.WorldLoadPromiseConditionsMet && !this.WorldUnloadPromiseConditionsMet ) {
					this.FulfillWorldUnloadPromises();
					this.FulfillPostWorldUnloadPromises();
				}
			} catch { }
		}


		internal void OnPostSetupContent() {
			Promises.AddWorldLoadEachPromise( () => {
				this.WorldUnloadPromiseConditionsMet = false;
				this.PostWorldUnloadPromiseConditionsMet = false;
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

			mymod.Promises.Update();
		}

		private void Update() {
			if( Main.netMode != 2 ) {
				if( this.WorldLoadPromiseConditionsMet && Main.gameMenu ) {
					this.WorldLoadPromiseConditionsMet = false; // Does this work?
				}
			}

			if( this.WorldUnloadPromiseConditionsMet ) {
				if( Main.gameMenu && Main.menuMode == 0 ) {
					this.FulfillPostWorldUnloadPromises();
				}
			}
		}

		
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

		internal void FulfillPostWorldUnloadPromises() {
			if( this.PostWorldUnloadPromiseConditionsMet ) { return; }
			this.PostWorldUnloadPromiseConditionsMet = true;

			foreach( Action promise in this.PostWorldUnloadOncePromises ) {
				promise();
			}
			foreach( Action promise in this.PostWorldUnloadEachPromises ) {
				promise();
			}

			this.PostWorldUnloadOncePromises.Clear();
		}
	}
}
