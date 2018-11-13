using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntityManager {
		public static Type GetTypeById( int type_id ) {
			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;

			if( !mngr.TypeIdEnts.ContainsKey( type_id ) ) {
				throw new HamstarException( "!ModHelpers.CustomEntityManager.GetTypeById - No CustomEntity of type id "+type_id );
			}
			return mngr.TypeIdEnts[type_id];
		}
		
		public static Type GetTypeByName( string name ) {
			int type_id = -1;

			try {
				type_id = CustomEntityManager.GetIdByTypeName( name );
				return CustomEntityManager.GetTypeById( type_id );
			} catch( HamstarException e ) {
				throw new HamstarException( "!ModHelpers.CustomEntityManager.GetTypeByName - No CustomEntity " + name, e );
			}
		}

		public static int GetIdByTypeName( string name ) {
			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;

			if( !mngr.EntTypeIds.ContainsKey( name ) ) {
				throw new HamstarException( "!ModHelpers.CustomEntityManager.GetIdByTypeName - No CustomEntity of type " + name );
			}
			return mngr.EntTypeIds[ name ];
		}
	}
}
