using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using HamstarHelpers.Internals.Logic;
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

			// Initialize drawing layer
			if( !Main.dedServ ) {
				Overlays.Scene["CustomEntity"] = new CustomEntityOverlay();
				Overlays.Scene.Activate( "CustomEntity" );

				Main.OnPostDraw += CustomEntityManager._PostDrawAll;
			}

			// Reset any data from previous games
			Promises.AddPostWorldUnloadEachPromise( () => {
				lock( CustomEntityManager.MyLock ) {
					this.EntitiesByIndexes.Clear();
					this.EntitiesByComponentType.Clear();
				}
			} );

			// Bind 'data dump' hotkey
			DataDumper.SetDumpSource( "CustomEntityList", () => {
				lock( CustomEntityManager.MyLock ) {
					return string.Join( "\n  ", this.EntitiesByIndexes.OrderBy( kv => kv.Key )
								 .Select( kv => kv.Key + ": " + kv.Value.ToString() ) );
				}
			} );

			// Refresh entity owners
			Promises.AddValidatedPromise<PlayerLogicPromiseArguments>( PlayerLogic.ServerConnectValidator, ( args ) => {
				foreach( var ent in this.EntitiesByIndexes.Values ) {
					ent.RefreshOwnerWho();
				}
				return true;
			} );
		}


		~CustomEntityManager() {
			if( !Main.dedServ ) {
				Main.OnPostDraw += CustomEntityManager._PostDrawAll;
			}
			Main.OnTick -= CustomEntityManager._Update;
		}
	}
}
