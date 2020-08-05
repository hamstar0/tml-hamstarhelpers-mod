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
	/// <summary>
	/// Assorted static "helper" functions pertaining to threading.
	/// </summary>
	public class TaskLauncher : ILoadable {
		/// <summary>
		/// Runs a given function (via. Task.Run), supplying it with the cancellation token used when mods are unloaded. Also handles
		/// waiting for the thread to close on mod unload.
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
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
				foreach( Task task in this.Tasks.ToArray() ) {
					if( task == null || task.IsCompleted || task.IsCanceled ) {
						this.Tasks.Remove( task );
					}
				}

				return ModHelpersMod.Instance != null;
			} );
		}

		void ILoadable.OnModsUnload() {
			this.CancelTokenSrc.Cancel();

			Task[] tasks = this.Tasks.Where(t=>t!=null).ToArray();
			if( !Task.WaitAll( tasks, new TimeSpan( 0, 0, 10 ) ) ) {
				LogHelpers.Alert( "Not all tasks successfully cancelled." );
			}

			this.CancelTokenSrc.Dispose();
		}
	}
}
