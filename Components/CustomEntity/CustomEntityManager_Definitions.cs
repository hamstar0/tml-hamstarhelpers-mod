using HamstarHelpers.Helpers.DebugHelpers;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntityManager {
		public int AddEntityTemplate( CustomEntity entity ) {
			int id = this.LatestEntityID++;

			this.EntityTemplates[id] = entity;
			return id;
		}


		public int TotalEntityTemplates() {
			return this.LatestEntityID;
		}


		public CustomEntity CreateEntityOfType( int id ) {
			CustomEntity val = null;

			if( !this.EntityTemplates.TryGetValue( id, out val ) ) {
				return null;
			}

			return val.Clone();
		}
	}
}
