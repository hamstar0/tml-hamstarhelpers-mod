using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Timers;


namespace HamstarHelpers.Helpers.DotNET.Threading {
	class TaskLauncher : ILoadable {
		public static Task Run( Action<CancellationToken> action ) {
			var tl = ModContent.GetInstance<TaskLauncher>();
			CancellationToken token = tl.CancelTokenSrc.Token;

			if( token.IsCancellationRequested ) {
				return (Task)null;
			}

			Task task = Task.Run( () => {
				action( token );
			}, token );

			tl.Tasks.Add( task );

			return task;
		}
		


		////////////////

		private IList<Task> Tasks = new List<Task>();
		private CancellationTokenSource CancelTokenSrc = new CancellationTokenSource();



		////////////////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnPostModsLoad() {
			Timers.SetTimer( "ModHelpersTasksPrune", 1, false, () => {
				int count = this.Tasks.Count;
				for( int i = 0; i < count; i++ ) {
					if( this.Tasks[i].IsCompleted || this.Tasks[i].IsCanceled ) {
						this.Tasks.RemoveAt( i );
					}
				}

				return ModHelpersMod.Instance != null;
			} );
		}

		void ILoadable.OnModsUnload() {
			this.CancelTokenSrc.Cancel();

			if( !Task.WaitAll( this.Tasks.ToArray(), new TimeSpan(0, 0, 10) ) ) {
				LogHelpers.Alert( "Not all tasks successfully cancelled." );
			}

			this.CancelTokenSrc.Dispose();
		}
	}
}
