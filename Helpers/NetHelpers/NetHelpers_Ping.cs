using HamstarHelpers.Components.Errors;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.NetHelpers {
	public partial class NetHelpers {
		public static int GetServerPing() {
			if( Main.netMode != 1 ) {
				throw new HamstarException( "Only clients can gauge ping.");
			}
			
			return ModHelpersMod.Instance.NetHelpers.CurrentPing;
		}


		////////////////

		private int CurrentPing = -1;


		////////////////

		internal void UpdatePing( int ping ) {
			this.CurrentPing = (this.CurrentPing + this.CurrentPing + ping) / 3;
		}
	}
}
