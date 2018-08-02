using HamstarHelpers.Helpers.DebugHelpers;
using System.Collections.Generic;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntityManager {
		public int AddEntityTemplate( string name, int width, int height, IList<CustomEntityComponent> components ) {
			int id = this.LatestEntityID++;

			this.EntityTemplates[id] = new CustomEntity( id, name, width, height, components );
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


		////////////////

		public int GetTemplateID( IList<CustomEntityComponent> components ) {
			int count = components.Count;

			foreach( var kv in this.EntityTemplates ) {
				int other_count = kv.Value.Components.Count;
				bool found = true;

				for( int i=0; i<count; i++ ) {
					if( i > other_count || components[i] != kv.Value.Components[i] ) {
						found = false;
						break;
					}
				}

				if( found ) {
					return kv.Key;
				}
			}

			return -1;
		}
	}
}
