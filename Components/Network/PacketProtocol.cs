using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using System;
using System.Collections.Generic;
using System.Text;


namespace HamstarHelpers.Components.Network {
	/// <summary>
	/// Implement to define a network protocol. Protocols define what data to transmit, and how and where it can be transmitted.
	/// </summary>
	public abstract partial class PacketProtocol : PacketProtocolData {
		protected PacketProtocol( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }
		
		public override void OnInitialize() { }	// Validations are handled internally



		/// <summary>
		/// Gets a random integer as a code representing a given protocol (by name) to identify its
		/// network packets.
		/// </summary>
		/// <param name="str">A protocol's name. Internally uses class names.</param>
		/// <returns>Random integer code.</returns>
		public static int GetPacketCode( string str ) {
			byte[] bytes = Encoding.UTF8.GetBytes( str );
			int code = 0;
			int pos = 0;

			for( int i=0; i<bytes.Length; i++ ) {
				code ^= (int)bytes[i] << pos;
				pos = pos >= 24 ? 0 : pos + 8;
			}

			return code;
		}


		internal static IDictionary<int, Type> GetProtocolTypes() {
			IEnumerable<Type> protocolTypes = ReflectionHelpers.GetAllAvailableSubTypes( typeof(PacketProtocol) );
			IDictionary<int, Type> protocolTypeMap = new Dictionary<int, Type>();

			foreach( Type subclass in protocolTypes ) {
				//ConstructorInfo ctorInfo = subclass.GetConstructor( BindingFlags.Instance | BindingFlags.NonPublic, null,
				//	new Type[] { typeof(PacketProtocolDataConstructorLock) }, null );
				//if( ctorInfo == null ) {
				//	throw new NotImplementedException( "Missing internal constructor for " + subclass.Name );
				//}

				if( ModHelpersMod.Instance.Config.DebugModeNetInfo ) {
					string name = subclass.Namespace + "." + subclass.Name;
					LogHelpers.Log( "PacketProtocol.GetProtocols() - " + name );
				}

				try {
					string name = subclass.Namespace + "." + subclass.Name;
					int code = PacketProtocol.GetPacketCode( name );

					protocolTypeMap[ code ] = subclass;
				} catch( Exception e ) {
					LogHelpers.Log( subclass.Name + " - " + e.Message );
				}
			}

			return protocolTypeMap;
		}



		////////////////

		/// <summary>
		/// Indicates whether send packets will be logged if the config specifies to do so. Defaults to true.
		/// </summary>
		[PacketProtocolIgnore]
		public virtual bool IsVerbose { get { return true; } }



		////////////////
		
		/// <summary>
		/// Returns qualified name of current packet class.
		/// </summary>
		public string GetPacketName() {
			var mytype = this.GetType();
			return mytype.Namespace + "." + mytype.Name;
		}


		////////////////

		/// <summary>
		/// Overridden for initializing the class to create a reply in a request to the client.
		/// </summary>
		protected virtual void SetClientDefaults() {
			throw new NotImplementedException( "No SetClientDefaults implemented" );
		}

		/// <summary>
		/// Overridden for initializing the class to create a reply in a request to the server.
		/// </summary>
		protected virtual void SetServerDefaults( int toWho ) {
			throw new NotImplementedException( "No SetServerDefaults(int) implemented" );
		}




		[Obsolete( "use SetServerDefaults( int toWho )", false )]
		protected virtual void SetServerDefaults() {
			throw new NotImplementedException( "No SetServerDefaults(int)" );
		}
	}
}
