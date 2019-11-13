using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Services.Hooks.LoadHooks;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Services.Timers {
	/// <summary>
	/// Provides a way to delay the onset of a given action by a set amount of ticks. As a secondary function,
	/// MainOnTickGet() provides a way to use Main.OnTick for running background tasks at 60FPS.
	/// </summary>
	public partial class Timers {
		private IDictionary<string, (bool RunsWhilePaused, Func<bool> Callback, int Elapsed)> Running
			= new Dictionary<string, (bool, Func<bool>, int)>();
		private IDictionary<string, int> Elapsed = new Dictionary<string, int>();

		private ISet<string> Expired = new HashSet<string>();

		private readonly Func<bool> OnTickGet;



		////////////////

		internal Timers() {
			this.OnTickGet = Timers.MainOnTickGet();
			Main.OnTick += Timers._Update;
//TICKSTART = DateTime.Now.Ticks;

			LoadHooks.AddWorldUnloadEachHook( () => {
				lock( Timers.MyLock ) {
					foreach( (string timerName, (bool, Func<bool>, int) timer) in this.Running ) {
						LogHelpers.Log( "Aborted timer " + timerName );
					}

					this.Running.Clear();
					this.Elapsed.Clear();
					this.Expired.Clear();
				}
			} );

			LoadHooks.AddModUnloadHook( () => {
				//internal void Unload() {
				try {
					Main.OnTick -= Timers._Update;
				} catch { }
			} );
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
				if( Main.gamePaused && !this.Running[name].RunsWhilePaused ) {
					continue;
				}

				this.Elapsed[name]++;

				if( this.Elapsed[name] >= this.Running[name].Elapsed ) {
					this.Expired.Add( name );

					if( this.Running[name].Callback() ) {
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
