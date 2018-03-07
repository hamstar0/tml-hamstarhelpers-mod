using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Utilities.Timers {
	public class Timers {
		private IDictionary<string, KeyValuePair<Action, int>> Repeaters = new Dictionary<string, KeyValuePair<Action, int>>();
		private IDictionary<string, int> Elapsed = new Dictionary<string, int>();



		////////////////

		internal void Begin() {
			Main.OnTick += this.RunTimers;
		}

		internal void End() {
			Main.OnTick -= this.RunTimers;
		}


		////////////////

		internal void RunTimers() {
			foreach( var kv in this.Repeaters ) {
				this.Elapsed[kv.Key]++;

				if( this.Elapsed[kv.Key] >= kv.Value.Value ) {
					this.Elapsed[kv.Key] = 0;
					kv.Value.Key();
				}
			}
		}


		////////////////

		public void SetRepeater( string name, int duration, Action action ) {
			this.Repeaters[ name ] = new KeyValuePair<Action, int>( action, duration );
			this.Elapsed[ name ] = 0;
		}


		public void UnsetRepeater( string name ) {
			this.Repeaters.Remove( name );
			this.Elapsed.Remove( name );
		}


		public void Reset() {
			this.Repeaters = new Dictionary<string, KeyValuePair<Action, int>>();
			this.Elapsed = new Dictionary<string, int>();
		}
	}
}
