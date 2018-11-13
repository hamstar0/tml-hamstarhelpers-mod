using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using HamstarHelpers.Services.DataDumper;
using HamstarHelpers.Services.Promises;
using HamstarHelpers.Services.Timers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Graphics.Effects;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntityManager {
		public readonly static object MyLock = new object();


		
		////////////////

		private int LatestId = 1;
		private IDictionary<string, int> EntTypeIds = new Dictionary<string, int>();
		private IDictionary<int, Type> TypeIdEnts = new Dictionary<int, Type>();

		private readonly IDictionary<int, CustomEntity> EntitiesByIndexes = new Dictionary<int, CustomEntity>();
		private readonly IDictionary<Type, ISet<int>> EntitiesByComponentType = new Dictionary<Type, ISet<int>>();

		private Func<bool> OnTickGet;



		////////////////

		internal CustomEntityManager() {
			this.OnTickGet = Timers.MainOnTickGet();
			Main.OnTick += CustomEntityManager._Update;

			// Initialize components
			var entity_types = ReflectionHelpers.GetAllAvailableSubTypes( typeof(CustomEntity) );

			foreach( Type entity_type in entity_types.OrderBy( e=>e.Name ) ) {
				this.CacheTypeIdInfo( entity_type );
			}

			// Initialize components
			var component_types = ReflectionHelpers.GetAllAvailableSubTypes( typeof(CustomEntityComponent) );

			foreach( var component_type in component_types ) {
				Type[] nested_types = component_type.GetNestedTypes( BindingFlags.Public | BindingFlags.NonPublic );

				foreach( var nested_type in nested_types ) {
					if( nested_type.IsSubclassOf( typeof( CustomEntityComponent.StaticInitializer ) ) ) {
						var static_init = (CustomEntityComponent.StaticInitializer)Activator.CreateInstance( nested_type );
						static_init.StaticInitializationWrapper();
					}
				}
			}

			if( !Main.dedServ ) {
				Overlays.Scene["CustomEntity"] = new CustomEntityOverlay();
				Overlays.Scene.Activate( "CustomEntity" );

				Main.OnPostDraw += CustomEntityManager._PostDrawAll;
			}

			Promises.AddPostWorldUnloadEachPromise( () => {
				lock( CustomEntityManager.MyLock ) {
					this.EntitiesByIndexes.Clear();
					this.EntitiesByComponentType.Clear();
				}
			} );

			DataDumper.SetDumpSource( "CustomEntityList", () => {
				lock( CustomEntityManager.MyLock ) {
					return string.Join( "\n  ", this.EntitiesByIndexes.OrderBy( kv => kv.Key )
								 .Select( kv => kv.Key + ": " + kv.Value.ToString() )
					);
				}
			} );
		}

		~CustomEntityManager() {
			if( !Main.dedServ ) {
				Main.OnPostDraw += CustomEntityManager._PostDrawAll;
			}
			Main.OnTick -= CustomEntityManager._Update;
		}


		////////////////

		private void CacheTypeIdInfo( Type ent_type ) {
			int id = this.LatestId++;

			this.TypeIdEnts[ id ] = ent_type;
			this.EntTypeIds[ ent_type.Name ] = id;
		}


		////////////////

		private static void _Update() { // <- Just in case references are doing something funky...
			var mymod = ModHelpersMod.Instance;
			if( mymod == null || mymod.CustomEntMngr == null ) { return; }

			if( mymod.CustomEntMngr.OnTickGet() ) {
				mymod.CustomEntMngr.Update();
			}
		}

		internal void Update() {
			if( !LoadHelpers.IsWorldBeingPlayed() ) { return; }

			lock( CustomEntityManager.MyLock ) {
				foreach( CustomEntity ent in this.EntitiesByIndexes.Values.ToArray() ) {
					ent.Update();
				}
			}
		}
	}
}
