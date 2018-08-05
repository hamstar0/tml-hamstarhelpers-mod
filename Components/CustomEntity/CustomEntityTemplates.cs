using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace HamstarHelpers.Components.CustomEntity {
	public class CustomEntityTemplates {
		public static CustomEntity CreateFromTemplateByID( int id ) {
			CustomEntityTemplates templates = HamstarHelpersMod.Instance.CustomEntMngr.Templates;
			CustomEntity val = null;

			if( !templates.EntityTemplates.TryGetValue( id, out val ) ) {
				return null;
			}

			return new CustomEntity( val.Core, val.Components );
		}


		////////////////

		public static int Add( CustomEntity template_ent ) {
			CustomEntityTemplates templates = HamstarHelpersMod.Instance.CustomEntMngr.Templates;

			int id = templates.LatestEntityID++;
			
			templates.EntityTemplates[id] = new CustomEntity( template_ent.Core, template_ent.Components );
			return id;
		}

		public static int Add( string name, int width, int height, IList<CustomEntityComponent> components ) {
			CustomEntityTemplates templates = HamstarHelpersMod.Instance.CustomEntMngr.Templates;

			int id = templates.LatestEntityID++;

			templates.EntityTemplates[id] = new CustomEntity( id, name, default(Vector2), width, height, components );
			return id;
		}

		////////////////

		internal static void Clear() {
			CustomEntityTemplates templates = HamstarHelpersMod.Instance.CustomEntMngr.Templates;

			templates.EntityTemplates.Clear();
		}

		////////////////

		public static int Count() {
			CustomEntityTemplates templates = HamstarHelpersMod.Instance.CustomEntMngr.Templates;

			return templates.LatestEntityID;
		}


		////////////////

		public static int GetID( IList<CustomEntityComponent> components ) {
			CustomEntityTemplates templates = HamstarHelpersMod.Instance.CustomEntMngr.Templates;
			int count = components.Count;

			foreach( var kv in templates.EntityTemplates ) {
				int id = kv.Key;
				CustomEntity ent = kv.Value;

				int other_count = ent.Components.Count;
				bool found = true;

				for( int i = 0; i < count; i++ ) {
					if( i >= other_count || components[i].GetType() != ent.Components[i].GetType() ) {
						found = false;
						break;
					}
				}

				if( found ) {
					return id;
				}
			}

			return -1;
		}



		////////////////

		private int LatestEntityID = 0;

		internal readonly IDictionary<int, CustomEntity> EntityTemplates = new Dictionary<int, CustomEntity>();


		////////////////

		internal CustomEntityTemplates() {
			Promises.AddModUnloadPromise( () => {
				this.LatestEntityID = 0;
				this.EntityTemplates.Clear();
			} );
		}
	}
}
