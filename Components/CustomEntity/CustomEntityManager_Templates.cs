using HamstarHelpers.Helpers.DebugHelpers;
using System.Collections.Generic;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntityManager {
		public int AddEntityTemplate( string name, IList<CustomEntityComponent> components ) {
			int id = this.LatestEntityID++;

			this.EntityTemplates[id] = new CustomEntity( id, name, components );
			return id;
		}


		public int TotalEntityTemplates() {
			return this.LatestEntityID;
		}

		////////////////

		public CustomEntity CreateEntityFromTemplate( int id ) {
			CustomEntity val = null;

			if( !this.EntityTemplates.TryGetValue( id, out val ) ) {
				return null;
			}

			return val.Clone();
		}
	}
}
