using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.Promises {
	public partial class Promises {
		private static object PostModLoadLock = new object();
		private static object ModUnloadLock = new object();
		private static object WorldLoadOnceLock = new object();
		private static object WorldLoadEachLock = new object();
		private static object PostWorldLoadOnceLock = new object();
		private static object PostWorldLoadEachLock = new object();
		private static object WorldUnloadOnceLock = new object();
		private static object WorldUnloadEachLock = new object();
		private static object PostWorldUnloadOnceLock = new object();
		private static object PostWorldUnloadEachLock = new object();
		private static object SafeWorldUnloadOnceLock = new object();
		private static object SafeWorldUnloadEachLock = new object();
		private static object ValidatedPromiseLock = new object();


		////////////////

		private IList<Action> PostModLoadPromises = new List<Action>();
		private IList<Action> ModUnloadPromises = new List<Action>();
		private IList<Action> WorldLoadOncePromises = new List<Action>();
		private IList<Action> WorldLoadEachPromises = new List<Action>();
		private IList<Action> PostWorldLoadOncePromises = new List<Action>();
		private IList<Action> PostWorldLoadEachPromises = new List<Action>();
		private IList<Action> WorldUnloadOncePromises = new List<Action>();
		private IList<Action> WorldUnloadEachPromises = new List<Action>();
		private IList<Action> PostWorldUnloadOncePromises = new List<Action>();
		private IList<Action> PostWorldUnloadEachPromises = new List<Action>();
		private IList<Action> SafeWorldUnloadOncePromise = new List<Action>();
		private IList<Action> SafeWorldUnloadEachPromise = new List<Action>();

		private bool PostModLoadPromiseConditionsMet = false;
		private bool WorldLoadPromiseConditionsMet = false;
		private bool WorldUnloadPromiseConditionsMet = false;
		private bool PostWorldUnloadPromiseConditionsMet = false;
		private bool SafeWorldLoadPromiseConditionsMet = false;

		private IDictionary<PromiseValidator, List<Func<PromiseArguments, bool>>> ValidatedPromise = new Dictionary<PromiseValidator, List<Func<PromiseArguments, bool>>>();
		private ISet<PromiseValidator> ValidatedPromiseConditionsMet = new HashSet<PromiseValidator>();
		private IDictionary<PromiseValidator, PromiseArguments> ValidatedPromiseArgs = new Dictionary<PromiseValidator, PromiseArguments>();

		private Func<bool> OnTickGet;



		////////////////

		internal Promises() {
			this.OnTickGet = Timers.Timers.MainOnTickGet();
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
			var mymod = HamstarHelpersMod.Instance;
			if( mymod == null ) { return; }
			
			if( mymod.Promises.OnTickGet() ) {	// <- Throttles to 60fps
				mymod.Promises.Update();
			}
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
	}
}
