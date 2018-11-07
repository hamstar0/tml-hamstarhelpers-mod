using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntityManager {
		public static int GetID( Type ent_type ) {
		}


		public static Type GetTypeByID( int type_id ) {

		}


		////////////////

		internal static void LoadAs( string type_name, CustomEntity ent ) {
			Type ent_type = Type.GetType( type_name );
			if( ent_type == null ) {
				throw new HamstarException( "!ModHelpers.CustomEntityManager.LoadAs - Invalid custom entity of type "+type_name );
			}

			CustomEntity typed_ent = ent.CloneAsType( ent_type );

			typed_ent.FinishInitialize();
		}


		////////////////

		public static CustomEntity Create( Type ent_type ) {
			if( !ent_type.IsSubclassOf(typeof(CustomEntity)) ) { throw new HamstarException( "Not a CustomEntity." ); }

			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;

			var ent = (CustomEntity)Activator.CreateInstance( ent_type,
				BindingFlags.NonPublic | BindingFlags.Instance,
				null,
				new object[] { ModHelpersMod.Instance.PacketProtocolCtorLock },
				null );

			CustomEntityManager.SetEntityByWho( mngr.EntitiesByIndexes.Count, ent );

			return ent;
		}


		////////////////

		public static void RemoveEntity( CustomEntity ent ) {
			if( ent == null ) { throw new HamstarException( "Null ent not allowed." ); }

			CustomEntityManager.RemoveEntityByWho( ent.Core.whoAmI );
		}

		public static void RemoveEntityByWho( int who ) {
			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;

			if( !mngr.EntitiesByIndexes.ContainsKey( who ) ) { return; }

			Type comp_type;
			Type base_type = typeof( CustomEntityComponent );

			lock( CustomEntityManager.MyLock ) {
				IList<CustomEntityComponent> ent_components = mngr.EntitiesByIndexes[who].Components;

				foreach( CustomEntityComponent component in ent_components ) {
					comp_type = component.GetType();
					do {
						if( mngr.EntitiesByComponentType.ContainsKey( comp_type ) ) {
							mngr.EntitiesByComponentType[comp_type].Remove( who );
						}

						comp_type = comp_type.BaseType;
					} while( comp_type != base_type );
				}

				mngr.EntitiesByIndexes.Remove( who );
			}
		}


		public static void ClearAllEntities() {
			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;

			lock( CustomEntityManager.MyLock ) {
				mngr.EntitiesByIndexes.Clear();
				mngr.EntitiesByComponentType.Clear();
			}
		}
	}
}
