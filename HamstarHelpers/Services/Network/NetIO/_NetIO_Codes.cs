using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Terraria;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Services.Network.NetIO.PayloadTypes;


namespace HamstarHelpers.Services.Network.NetIO {
	/*/// <summary>
	/// Provides functions to neatly send data (via. ModPacket) to server, clients, or both. Abstracts away serialization and
	/// routing.
	/// </summary>
	public partial class NetIO : ILoadable {
		internal static Type[] GetProtocolTypes() {
			IEnumerable<Type> protocolTypes = ReflectionHelpers.GetAllAvailableSubTypesFromMods( typeof( NetIOPayload ) );
			IList<Type> filteredProtocolTypes = new List<Type>();

			foreach( Type subclassType in protocolTypes ) {
				ConstructorInfo ctorInfo = subclassType.GetConstructor(
					bindingAttr: BindingFlags.Instance | BindingFlags.NonPublic,
					binder: null,
					types: new Type[] { },
					modifiers: null
				);
				if( ctorInfo == null ) {
					throw new ModHelpersException( "Missing private constructor for " + subclassType.Name
						+ " (" + subclassType.Namespace + ")" );
				}
				if( ctorInfo.IsFamily ) {
					throw new ModHelpersException( "Invalid constructor for " + subclassType.Name
						+ " (" + subclassType.Namespace + "); must be private, not protected." );
				}

				if( ModHelpersConfig.Instance.DebugModeNetInfo ) {
					string name = subclassType.Namespace + "." + subclassType.Name;
					LogHelpers.Alert( name );
				}

				filteredProtocolTypes.Add( subclassType );
			}

			return filteredProtocolTypes
				.OrderBy( t => t.Namespace + "." + t.Name )
				.ToArray();
		}



		////

		private void MapProtocolTypes() {
			Type[] types = NetIO.GetProtocolTypes();

			for( int i=0; i<types.Length; i++ ) {
				this.ProtocolIdsByType[ types[i] ] = i;
				this.ProtocolTypeByIds[ i ] = types[i];
			}
		}
	}*/
}
