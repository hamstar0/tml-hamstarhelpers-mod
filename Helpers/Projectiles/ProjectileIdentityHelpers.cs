using HamstarHelpers.Components.DataStructures;
using Terraria;


namespace HamstarHelpers.Helpers.Projectiles {
	/** <summary>Assorted static "helper" functions pertaining to players relative to projectile identification.</summary> */
	public partial class ProjectileIdentityHelpers {
		public static string GetProperUniqueId( int projType ) {
			var proj = new Projectile();
			proj.SetDefaults( projType );

			return ProjectileIdentityHelpers.GetProperUniqueId( proj );
		}

		public static string GetProperUniqueId( Projectile proj ) {
			if( proj.modProjectile == null ) {
				return "Terraria." + proj.type;
			}
			return proj.modProjectile.mod.Name + "." + proj.modProjectile.Name;
		}

		////

		public static string GetQualifiedName( Projectile proj ) {
			return ProjectileIdentityHelpers.GetQualifiedName( proj.type );
		}

		public static string GetQualifiedName( int projType ) {
			string name = Lang.GetProjectileName( projType ).Value;
			return name;
		}

		// TODO: GetVanillaSnapshotHash()


		////////////////

		public static ReadOnlyDictionaryOfSets<string, int> NamesToIds {
			get { return ModHelpersMod.Instance.ProjectileIdentityHelpers._NamesToIds; }
		}
	}
}
