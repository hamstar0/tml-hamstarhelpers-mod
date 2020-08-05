using HamstarHelpers.Classes.DataStructures;
using ReLogic.Reflection;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace HamstarHelpers.Helpers.Projectiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to players relative to projectile identification
	/// </summary>
	public partial class ProjectileIdentityHelpers {
		/// @private
		[Obsolete( "use ProjectileDefinition's ctor", true )]
		public static ProjectileDefinition GetProjectileDefinition( string uniqueKey ) {
			string[] segs = uniqueKey.Split( new char[] { ' ' }, 2 );
			return new ProjectileDefinition( segs[0], segs[1] );
		}


		// TODO: GetVanillaSnapshotHash()
	}
}
