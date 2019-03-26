using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Services.Timers {
	public partial class Timers {
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
