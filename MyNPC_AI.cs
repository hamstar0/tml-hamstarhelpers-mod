using System;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.EntityControls;


namespace HamstarHelpers {
	/// @private
	partial class ModHelpersNPC : GlobalNPC {
		public override bool PreAI( NPC npc ) {
			EntityControls.ApplyFakeTarget( npc, this.FakeTarget, this.FakeTargetPosition );

			if( this.LockedAI0.HasValue ) {
				npc.ai[0] = this.LockedAI0.Value;
			}
			if( this.LockedAI1.HasValue ) {
				npc.ai[1] = this.LockedAI1.Value;
			}
			if( this.LockedAI2.HasValue ) {
				npc.ai[2] = this.LockedAI2.Value;
			}
			if( this.LockedAI3.HasValue ) {
				npc.ai[3] = this.LockedAI3.Value;
			}

			return base.PreAI( npc );
		}

		public override void PostAI( NPC npc ) {
			EntityControls.RevertFakeTarget( npc, this.FakeTarget, this.FakeTargetPosition );
			base.PostAI( npc );
		}
	}
}
