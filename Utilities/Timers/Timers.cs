using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Utilities.Timers {
	public class Timers {
		private readonly static object MyLock = new object();



		public static void SetTimer( string name, int tick_duration, Func<bool> action ) {
			var timers = HamstarHelpersMod.Instance.Timers;

			lock( Timers.MyLock ) {
				timers.Running[name] = new KeyValuePair<Func<bool>, int>( action, tick_duration );
				timers.Elapsed[name] = 0;
				timers.Expired.Remove( name );
			}
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



		////////////////

		internal Timers() {
			Main.OnTick += this.RunTimers;
		}

		~Timers() {
		//internal void Unload() {
			try {
				Main.OnTick -= this.RunTimers;
			} catch { }
		}


		////////////////

		internal void RunTimers() {
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
