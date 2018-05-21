using Newtonsoft.Json;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Utilities.EntityGroups {
	public class ProjectileGroupDefinition : EntityGroupDefinition<Projectile> {
		public static IList<Projectile> GetProjectileGroup( string query_json ) {
			var def = JsonConvert.DeserializeObject<ProjectileGroupDefinition>( query_json );
			return def.GetGroup();
		}


		////////////////

		private readonly Projectile[] MyPool = null;

		public override Projectile[] GetPool() {
			if( this.MyPool == null ) {
				for( int i = 0; i < Main.projectileTexture.Length; i++ ) {
					this.MyPool[i] = new Projectile();
					this.MyPool[i].SetDefaults( i );
				}
			}
			return this.MyPool;
		}
	}
}
