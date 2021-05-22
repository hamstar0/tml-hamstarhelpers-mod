using HamstarHelpers.Libraries.Debug;


namespace HamstarHelpers.Services.Network {
	/// <summary>
	/// Supplies assorted server informations and tools.
	/// </summary>
	public class Server {
		/// <summary></summary>
		public int AveragePing { get; internal set; }



		////////////////

		internal Server() {
			this.AveragePing = -1;
		}


		////////////////

		internal void UpdatePingAverage( int ping ) {
			if( this.AveragePing == -1 ) {
				this.AveragePing = ping;
			} else {
				//this.AveragePing = (ping + (this.AveragePing * 2)) / 3;
				this.AveragePing = (ping + this.AveragePing) / 2;
			}
		}
	}
}
