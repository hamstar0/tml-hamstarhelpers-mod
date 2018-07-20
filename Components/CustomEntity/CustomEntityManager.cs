using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntityManager : IEnumerable<CustomEntity> {
		public static CustomEntityManager Instance { get { return HamstarHelpersMod.Instance.CustomEntMngr; } }



		////////////////

		private readonly IDictionary<int, CustomEntity> EntitiesToIds = new Dictionary<int, CustomEntity>();
		private readonly IDictionary<string, ISet<int>> EntitiesByComponentName = new Dictionary<string, ISet<int>>();



		////////////////

		internal CustomEntityManager() {
			Main.OnTick += CustomEntityManager._Update;

			// Initialize components:
			IEnumerable<Type> component_types = ReflectionHelpers.GetAllAvailableSubTypes( typeof( CustomEntityComponent ) );
			foreach( var component_type in component_types ) {
				var component = (CustomEntityComponent)Activator.CreateInstance( component_type, new object[] { true } );
				component.StaticInitializeInternalWrapper();
			}

			Promises.AddWorldUnloadEachPromise( () => {
				this.EntitiesToIds.Clear();
				this.EntitiesByComponentName.Clear();
			} );
		}

		~CustomEntityManager() {
			Main.OnTick -= CustomEntityManager._Update;
		}


		////////////////

		private static void _Update() { // <- Just in case references are doing something funky...
			HamstarHelpersMod mymod = HamstarHelpersMod.Instance;
			if( mymod == null ) { return; }

			mymod.CustomEntMngr.Update();
		}

		internal void Update() {
			foreach( CustomEntity ent in this.EntitiesToIds.Values.ToArray() ) {
				ent.Update();
			}
		}


		////////////////

		internal void DrawAll( SpriteBatch sb ) {
			foreach( CustomEntity ent in this.EntitiesToIds.Values ) {
				var draw_comp = (DrawsEntityComponent)ent.GetComponentByType<DrawsEntityComponent>();
				if( draw_comp != null ) {
					draw_comp.Draw( sb, ent );
				}
			}
		}
	}
}
