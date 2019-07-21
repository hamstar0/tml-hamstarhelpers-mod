using Terraria;


namespace HamstarHelpers.Helpers.Projectiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to players relative to projectiles.
	/// </summary>
	public partial class ProjectileHelpers {
		/// <summary>
		/// Applies projectile "hits", as if to make the effect of impacting something (including consuming penetrations).
		/// </summary>
		/// <param name="projectile"></param>
		public static void Hit( Projectile projectile ) {
			if( projectile.penetrate <= 0 ) {
				projectile.Kill();
			} else {
				projectile.penetrate--;
				projectile.netUpdate = true;
			}
		}
	}
}
