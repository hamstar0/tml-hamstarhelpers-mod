using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntityManager {
		public static Type GetTypeById( int typeId ) {
			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;

			if( !mngr.TypeIdEnts.ContainsKey( typeId ) ) {
				throw new HamstarException( "!ModHelpers.CustomEntityManager.GetTypeById - No CustomEntity of type id "+typeId );
			}
			return mngr.TypeIdEnts[typeId];
		}
		
		public static Type GetTypeByName( string name ) {
			int typeId = -1;

			try {
				typeId = CustomEntityManager.GetIdByTypeName( name );
				return CustomEntityManager.GetTypeById( typeId );
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
