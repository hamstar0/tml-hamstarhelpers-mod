using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Services.Timers {
	public class Timers {
		private readonly static object MyLock = new object();
		
		public static bool MainOnTickGo { get; private set; }



		public static void SetTimer( string name, int tick_duration, Func<bool> action ) {
			var timers = HamstarHelpersMod.Instance.Timers;

			lock( Timers.MyLock ) {
				timers.Running[name] = new KeyValuePair<Func<bool>, int>( action, tick_duration );
				timers.Elapsed[name] = 0;
				timers.Expired.Remove( name );
			}
		}


		public static int GetTimerTickDuration( string name ) {
			var timers = HamstarHelpersMod.Instance.Timers;

			lock( Timers.MyLock ) {
				if( timers.Running.ContainsKey( name ) ) {
					return timers.Running[name].Value - timers.Elapsed[ name ];
				}
			}

			return 0;
		}


		public static void UnsetTimer( string name ) {
			var timers = HamstarHelpersMod.Instance.Timers;

			lock( Timers.MyLock ) {
				if( timers.Running.ContainsKey( name ) ) {
					timers.Running.Remove( name );
					timers.Elapsed.Remove( name );
					timers.Expired.Remove( name );
				}
			}
		}


		public static void ResetAll() {
			var timers = HamstarHelpersMod.Instance.Timers;

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

		private long PreviousTicks = 0;



		////////////////

		internal Timers() {
			Main.OnTick += Timers._RunTimers;

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
				Main.OnTick -= Timers._RunTimers;
			} catch { }
		}


		////////////////

		private static void _RunTimers() {  // <- Just in case references are doing something funky...
			HamstarHelpersMod mymod = HamstarHelpersMod.Instance;
			if( mymod == null ) { return; }
			
			mymod.Timers.RunTimers();
		}
		
//private int Ticks = 0;
//private long TickStop = 0;
		internal void RunTimers() {
			long now = DateTime.Now.Ticks;

			Timers.MainOnTickGo = ( now - this.PreviousTicks ) < ( 10000000 / 90 );

			if( Timers.MainOnTickGo ) {
				return;
			}
//this.Ticks++;
//var span2 = new TimeSpan( now - this.TickStop );
//if( span2.TotalMilliseconds > 1000 ) {
//	DebugHelpers.Print("ticks", "fps: "+(this.Ticks), 20);
//	this.TickStop = now;
//	this.Ticks = 0;
//}
			this.PreviousTicks = now;

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
