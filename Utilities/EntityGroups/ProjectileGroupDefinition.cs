using Newtonsoft.Json;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Utilities.EntityGroups {
	public class ProjectileGroupDefinition : EntityGroupDefinition<Projectile> {
		public static ISet<Projectile> GetProjectileGroup( string query_json ) {
			var def = JsonConvert.DeserializeObject<ProjectileGroupDefinition>( query_json );
			return def.GetGroup();
		}


		////////////////

		private Projectile[] MyPool = null;

		////////////////

		public override Projectile[] GetPool() {
			if( this.MyPool == null ) {
				for( int i = 0; i < ProjectileLoader.ProjectileCount; i++ ) {
					this.MyPool[i] = new Projectile();
					this.MyPool[i].SetDefaults( i );
				}
			}
			return this.MyPool;
		}

		public override void ClearPool() {
			this.MyPool = null;
		}
	}
}
