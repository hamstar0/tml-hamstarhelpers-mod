﻿using Terraria;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public class PeriodicSyncEntityComponent : CustomEntityComponent {
		private int LastSynced;



		////////////////

		public PeriodicSyncEntityComponent() {
			this.LastSynced = Main.rand.Next( 60 * 10 );

			this.ConfirmLoad();
		}

		////////////////

		public override void UpdateServer( CustomEntity ent ) {
			if( this.LastSynced-- <= 0 ) {
				this.LastSynced = 60 * 10;

				ent.Sync();
			}
		}
	}
}
