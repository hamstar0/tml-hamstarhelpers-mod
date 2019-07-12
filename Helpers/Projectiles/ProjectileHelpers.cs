using Terraria;


namespace HamstarHelpers.Helpers.Projectiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to players relative to projectiles.
	/// </summary>
	public partial class ProjectileHelpers {
		/// <summary>
		/// Applies projectile "hits", as if to make the effect of impacting something (including consuming penetrations).
		/// </summary>
		/// <param name="proj"></param>
		public static void Hit( Projectile proj ) {
			if( proj.penetrate <= 0 ) {
				proj.Kill();
			} else {
				proj.penetrate--;
				proj.netUpdate = true;
			}
		}
	}
}
