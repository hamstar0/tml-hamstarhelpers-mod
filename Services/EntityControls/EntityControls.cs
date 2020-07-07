using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Services.EntityControls {
	/// <summary>
	/// Implements functions for controlling the targetting of entities.
	/// </summary>
	public class EntityControls {
		private static int OldTarget = -1;



		////////////////

		/// <summary>
		/// Forces an NPC to target a specific point (via. dummy player invisibly moved to that position).
		/// </summary>
		/// <param name="npc"></param>
		/// <param name="position"></param>
		public static void SetTarget( NPC npc, Vector2 position ) {
			//if( EntityControls.OldTarget != -1 ) {
			//	throw new ModHelpersException( "Cannot set target while an NPC is targetting." );
			//}
			var mynpc = npc.GetGlobalNPC<ModHelpersNPC>();
			mynpc.FakeTargetPosition = position;
			mynpc.FakeTarget = null;
		}

		/// <summary>
		/// Forces an NPC's target to be a specific NPC.
		/// </summary>
		/// <param name="npc"></param>
		/// <param name="target"></param>
		public static void SetTarget( NPC npc, NPC target ) {
			var mynpc = npc.GetGlobalNPC<ModHelpersNPC>();
			mynpc.FakeTargetPosition = null;
			mynpc.FakeTarget = target;
		}

		/// <summary>
		/// Forces an NPC's target to be a specific player.
		/// </summary>
		/// <param name="npc"></param>
		/// <param name="target"></param>
		public static void SetTarget( NPC npc, Player target ) {
			var mynpc = npc.GetGlobalNPC<ModHelpersNPC>();
			mynpc.FakeTargetPosition = null;
			mynpc.FakeTarget = target;
		}

		////

		/// <summary>
		/// Unsets NPC's forced target.
		/// </summary>
		/// <param name="npc"></param>
		public static void UnsetTarget( NPC npc ) {
			var mynpc = npc.GetGlobalNPC<ModHelpersNPC>();
			mynpc.FakeTarget = null;
			mynpc.FakeTargetPosition = null;
		}


		////////////////

		/// <summary>
		/// Sets entries in an NPC's `ai` array to be locked to a given value. Enter a `null` value to clear a lock.
		/// </summary>
		/// <param name="npc"></param>
		/// <param name="ai1"></param>
		/// <param name="ai2"></param>
		/// <param name="ai3"></param>
		/// <param name="ai4"></param>
		public static void LockAI( NPC npc, float? ai1, float? ai2, float? ai3, float? ai4 ) {
			var mynpc = npc.GetGlobalNPC<ModHelpersNPC>();
			mynpc.LockedAI0 = ai1;
			mynpc.LockedAI1 = ai2;
			mynpc.LockedAI2 = ai3;
			mynpc.LockedAI3 = ai4;
		}


		////////////////

		internal static void ApplyFakeTarget( NPC npc, Entity target, Vector2? targetPosition ) {
			if( target != null ) {
				EntityControls.OldTarget = npc.target;

				if( target is NPC ) {
					npc.target = target.whoAmI + 300;
				} else if( target is Player ) {
					npc.target = target.whoAmI;
				}
			} else if( targetPosition.HasValue ) {
				Main.player[255].Center = targetPosition.Value;
				Main.player[255].active = true;
				Main.player[255].dead = false;
				Main.player[255].statLife = Main.player[255].statLifeMax2;

				npc.target = 254;

				Timers.Timers.RunNow( () => Main.player[255].active = false );
			}
		}

		internal static void RevertFakeTarget( NPC npc, Entity target, Vector2? targetPosition ) {
			if( target != null ) {
				if( EntityControls.OldTarget != -1 ) {
					npc.target = EntityControls.OldTarget;
					EntityControls.OldTarget = -1;
				}
			} else if( targetPosition.HasValue ) {
				Main.player[255].active = false;
			}
		}
	}
}
