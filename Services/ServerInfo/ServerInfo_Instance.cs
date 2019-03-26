using HamstarHelpers.Helpers.DebugHelpers;


namespace HamstarHelpers.Services.ServerInfo {
	public class ServerInfo {
		public int AveragePing { get; internal set; }


		////////////////

		internal ServerInfo() {
			this.AveragePing = -1;
		}


		////////////////

		internal void UpdatePingAverage( int ping ) {
			if( this.AveragePing == -1 ) {
				this.AveragePing = ping;
			} else {
				this.AveragePing = (ping + ( this.AveragePing * 2 )) / 3;
			}
		}
	}
}
