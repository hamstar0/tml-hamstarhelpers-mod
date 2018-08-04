using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
using System.Collections.Generic;


namespace HamstarHelpers.Components.CustomEntity {
	public class CustomEntityTemplates {
		public static CustomEntity CreateEntityFromTemplate( int id ) {
			CustomEntityTemplates templates = HamstarHelpersMod.Instance.CustomEntMngr.Templates;
			CustomEntity val = null;

			if( !templates.EntityTemplates.TryGetValue( id, out val ) ) {
				return null;
			}

			return val.Clone();
		}


		////////////////

		public static int AddEntityTemplate( CustomEntity ent ) {
			CustomEntityTemplates templates = HamstarHelpersMod.Instance.CustomEntMngr.Templates;

			int id = templates.LatestEntityID++;

			templates.EntityTemplates[id] = ent.Clone();
			return id;
		}

		public static int AddEntityTemplate( string name, int width, int height, IList<CustomEntityComponent> components ) {
			CustomEntityTemplates templates = HamstarHelpersMod.Instance.CustomEntMngr.Templates;

			int id = templates.LatestEntityID++;

			templates.EntityTemplates[id] = new CustomEntity( id, name, width, height, components );
			return id;
		}

		////////////////

		internal static void ClearEntityTemplates() {
			CustomEntityTemplates templates = HamstarHelpersMod.Instance.CustomEntMngr.Templates;

			templates.EntityTemplates.Clear();
		}

		////////////////

		public static int TotalEntityTemplates() {
			CustomEntityTemplates templates = HamstarHelpersMod.Instance.CustomEntMngr.Templates;

			return templates.LatestEntityID;
		}


		////////////////

		public static int GetTemplateID( IList<CustomEntityComponent> components ) {
			CustomEntityTemplates templates = HamstarHelpersMod.Instance.CustomEntMngr.Templates;
			int count = components.Count;

			foreach( var kv in templates.EntityTemplates ) {
				int other_count = kv.Value.Components.Count;
				bool found = true;

				for( int i = 0; i < count; i++ ) {
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



		////////////////

		private int LatestEntityID = 0;

		private readonly IDictionary<int, CustomEntity> EntityTemplates = new Dictionary<int, CustomEntity>();


		////////////////

		internal CustomEntityTemplates() {
			Promises.AddModUnloadPromise( () => {
				this.LatestEntityID = 0;
				this.EntityTemplates.Clear();
			} );
		}
	}
}
