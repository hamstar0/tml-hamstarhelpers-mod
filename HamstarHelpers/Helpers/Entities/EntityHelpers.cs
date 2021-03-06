﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Items.Attributes;
using HamstarHelpers.Helpers.NPCs.Attributes;
using HamstarHelpers.Helpers.Projectiles.Attributes;


namespace HamstarHelpers.Helpers.Entities {
	/// <summary>
	/// Assorted static "helper" functions pertaining to `Entity`s (parent class of Item, NPC, Player, and Projectile).
	/// </summary>
	public class EntityHelpers {
		/// <summary>
		/// Gets a hash value to attempt to uniquely identify a given entity. Not recommended if the specific entity's
		/// `GetVanillaSnapshotHash(...)` (via. the respective Helper) is available.
		/// </summary>
		/// <param name="ent">Entity to attempt to identify.</param>
		/// <param name="noContext">Omits `whoAmI`.</param>
		/// <returns>The identifying hash of the entity.</returns>
		public static int GetVanillaSnapshotHash( Entity ent, bool noContext ) {
			int pow = 1;
			int Pow() {
				pow *= 2;
				if( pow > 16777216 ) { pow = 1; }
				return pow;
			}

			//

			int hash = ("active"+ent.active).GetHashCode();

			if( !noContext ) {
				//hash ^= ("position"+ent.position).GetHashCode();
				//hash ^= ("velocity"+ent.velocity).GetHashCode();
				//hash ^= ("oldPosition"+ent.oldPosition).GetHashCode();
				//hash ^= ("oldVelocity"+ent.oldVelocity).GetHashCode();
				//hash ^= ("oldDirection"+ent.oldDirection).GetHashCode();
				//hash ^= ("direction"+ent.direction).GetHashCode();
				hash += ("whoAmI"+ent.whoAmI).GetHashCode() + Pow();
				//hash ^= ("wet"+ent.wet).GetHashCode();
				//hash ^= ("honeyWet"+ent.honeyWet).GetHashCode();
				//hash ^= ("wetCount"+ent.wetCount).GetHashCode();
				//hash ^= ("lavaWet"+ent.lavaWet).GetHashCode();
			} else {
				Pow();
			}
			hash += ("width"+ent.width).GetHashCode() + Pow();
			hash += ("height"+ent.height).GetHashCode() + Pow();
			
			return hash;
		}


		/// <summary>
		/// Applies a given amount of force to the given entity from a given outside point.
		/// </summary>
		/// <param name="ent">Entity being acted upon.</param>
		/// <param name="worldPosFrom">Source of force.</param>
		/// <param name="force">Amount of force.</param>
		/// <param name="sync">Indicates whether to sync the entity in multiplayer.</param>
		public static void ApplyForce( Entity ent, Vector2 worldPosFrom, float force, bool sync ) {
			Vector2 offset = worldPosFrom - ent.Center;
			Vector2 forceVector = Vector2.Normalize( offset ) * force;
			ent.velocity += forceVector;

			if( sync && Main.netMode != NetmodeID.SinglePlayer ) {
				NetMessage.SendData( MessageID.SyncNPC, -1, ( Main.netMode == NetmodeID.Server ? -1 : Main.myPlayer ), null, ent.whoAmI );
			}
		}


		/// <summary>
		/// Attempts a basic trace to see if a given entity is visible from a given point by line of sight (no obstructing tiles).
		/// </summary>
		/// <param name="position"></param>
		/// <param name="ent"></param>
		/// <param name="blockingTilePatten"></param>
		/// <returns></returns>
		public static bool SimpleLineOfSight( Vector2 position, Entity ent, TilePattern blockingTilePatten ) {
			var trace = new Utils.PerLinePoint( delegate ( int tileX, int tileY ) {
				return !blockingTilePatten.Check( tileX, tileY );
			} );
			return Utils.PlotTileLine( position, ent.Center, 1, trace );
		}


		////////////////

		/// <summary>
		/// Gets the "qualified" name (the name the player sees) of a given entity.
		/// </summary>
		/// <param name="ent"></param>
		/// <returns></returns>
		public static string GetQualifiedName( Entity ent ) {
			if( ent is Item ) {
				return ItemAttributeHelpers.GetQualifiedName( (Item)ent );
			}
			if( ent is NPC ) {
				return NPCAttributeHelpers.GetQualifiedName( (NPC)ent );
			}
			if( ent is Projectile ) {
				return ProjectileAttributeHelpers.GetQualifiedName( (Projectile)ent );
			}
			if( ent is Player ) {
				return ( (Player)ent ).name;
			}
			return "...a "+ent.GetType().Name;
		}
	}
}
