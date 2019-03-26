using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Services.Timers {
	public partial class Timers {
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
	}
}
