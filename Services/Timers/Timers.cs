using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Services.Timers {
	public class Timers {
		private readonly static object MyLock = new object();
		

		public static Func<bool> MainOnTickGet() {
			long then = DateTime.Now.Ticks;
			int frames = 0, subFrames;
//int FRAMES=0;

			return () => {
				long now = DateTime.Now.Ticks;
				int span = (int)( now - then );

				frames += span / (10000000 / 90);
				subFrames = span % (10000000 / 90);

				then = now - subFrames;
//DebugHelpers.Print("blahh", "frames: "+FRAMES, 20);
				
				if( frames > 0 ) {
//FRAMES+=frames;
					frames = 0;//frames--;
					return true;
				}
				return false;
			};
		}


		////////////////

		public static void SetTimer( string name, int tickDuration, Func<bool> action ) {
			var timers = ModHelpersMod.Instance?.Timers;
			if( timers == null ) { return; }

			lock( Timers.MyLock ) {
				timers.Running[name] = new KeyValuePair<Func<bool>, int>( action, tickDuration );
				timers.Elapsed[name] = 0;
				timers.Expired.Remove( name );
			}
		}


		public static int GetTimerTickDuration( string name ) {
			var timers = ModHelpersMod.Instance?.Timers;
			if( timers == null ) { return 0; }

			lock( Timers.MyLock ) {
				if( timers.Running.ContainsKey( name ) ) {
					return timers.Running[name].Value - timers.Elapsed[ name ];
				}
			}

			return 0;
		}


		public static void UnsetTimer( string name ) {
			var timers = ModHelpersMod.Instance?.Timers;
			if( timers == null ) { return; }

			lock( Timers.MyLock ) {
				if( timers.Running.ContainsKey( name ) ) {
					timers.Running.Remove( name );
					timers.Elapsed.Remove( name );
					timers.Expired.Remove( name );
				}
			}
		}


		public static void ResetAll() {
			var timers = ModHelpersMod.Instance?.Timers;
			if( timers == null ) { return; }

			lock( Timers.MyLock ) {
				timers.Running = new Dictionary<string, KeyValuePair<Func<bool>, int>>();
				timers.Elapsed = new Dictionary<string, int>();
				timers.Expired = new HashSet<string>();
			}
		}



		////////////////

		private IDictionary<string, KeyValuePair<Func<bool>, int>> Running = new Dictionary<string, KeyValuePair<Func<bool>, int>>();
		private IDictionary<string, int> Elapsed = new Dictionary<string, int>();

		private ISet<string> Expired = new HashSet<string>();

		private readonly Func<bool> OnTickGet;



		////////////////

		internal Timers() {
			this.OnTickGet = Timers.MainOnTickGet();
			Main.OnTick += Timers._Update;
//TICKSTART = DateTime.Now.Ticks;

			Promises.Promises.AddWorldUnloadEachPromise( () => {
				lock( Timers.MyLock ) {
					foreach( var kv in this.Running ) {
						LogHelpers.Log( "Aborted timer " + kv.Key );
					}

					this.Running.Clear();
					this.Elapsed.Clear();
					this.Expired.Clear();
				}
			} );
		}

		~Timers() {
		//internal void Unload() {
			try {
				Main.OnTick -= Timers._Update;
			} catch { }
		}


		////////////////
		
//private static long TICKSTART=0;
//private static int TICKCOUNT=0;
		private static void _Update() {  // <- Just in case references are doing something funky...
			ModHelpersMod mymod = ModHelpersMod.Instance;
			if( mymod == null || mymod.Timers == null ) { return; }

			if( mymod.Timers.OnTickGet() ) {
//long NOW = DateTime.Now.Ticks;
//TICKCOUNT++;
//if( (NOW - TICKSTART) > 10000000 ) { 
//	DebugHelpers.Print("blah", ""+TICKCOUNT,20);
//	TICKSTART = NOW;
//	TICKCOUNT = 0;
//}
				mymod.Timers.Update();
			}
		}

		private void Update() {
			foreach( string name in this.Running.Keys.ToArray() ) {
				int duration = this.Running[ name ].Value;

				this.Elapsed[name]++;

				if( this.Elapsed[name] >= duration ) {
					Func<bool> func = this.Running[ name ].Key;

					this.Expired.Add( name );

					if( func() ) {
						this.Elapsed[name] = 0;
						this.Expired.Remove( name );
					}
				}
			}

			lock( Timers.MyLock ) {
				if( this.Expired.Count > 0 ) {
					foreach( string name in this.Expired ) {
						this.Running.Remove( name );
						this.Elapsed.Remove( name );
					}
					this.Expired.Clear();
				}
			}
		}
	}
}
