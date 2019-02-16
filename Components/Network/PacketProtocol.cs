using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;


namespace HamstarHelpers.Components.Network {
	/// <summary>
	/// Implement to define a network protocol. Protocols define what data to transmit, and how and where it can be transmitted.
	/// </summary>
	public abstract partial class PacketProtocol : PacketProtocolData {
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

			foreach( Type subclassType in protocolTypes ) {
				ConstructorInfo ctorInfo = subclassType.GetConstructor( BindingFlags.Instance | BindingFlags.NonPublic, null,
					new Type[] { typeof( PacketProtocolDataConstructorLock ) }, null );

				if( ctorInfo == null ) {
					ctorInfo = subclassType.GetConstructor( BindingFlags.Instance | BindingFlags.NonPublic, null,
						new Type[] { }, null );

					if( ctorInfo == null ) {
						throw new HamstarException( "Missing private constructor for " + subclassType.Name + " ("+subclassType.Namespace+")" );
					}
					if( ctorInfo.IsFamily ) {
						throw new HamstarException( "Invalid constructor for " + subclassType.Name + " ("+subclassType.Namespace+"); must be private, not protected." );
					}
				}

				if( ModHelpersMod.Instance.Config.DebugModeNetInfo ) {
					string name = subclassType.Namespace + "." + subclassType.Name;
					LogHelpers.Alert( name );
				}

				try {
					string name = subclassType.Namespace + "." + subclassType.Name;
					int code = PacketProtocol.GetPacketCode( name );

					protocolTypeMap[ code ] = subclassType;
				} catch( Exception e ) {
					LogHelpers.Log( subclassType.Name + " - " + e.Message );
				}
			}

			return protocolTypeMap;
		}



		////////////////

		/// <summary>
		/// Indicates whether sent packets will be logged if the config specifies to do so. Defaults to true.
		/// </summary>
		[PacketProtocolIgnore]
		public virtual bool IsVerbose => true;

		/// <summary>
		/// Indicates whether to handle stream encoding and decoding with a separate thread. Defaults to false.
		/// </summary>
		[PacketProtocolIgnore]
		public virtual bool IsAsync => false;



		////////////////

		protected PacketProtocol( PacketProtocolDataConstructorLock _=null ) { }

		////////////////

		protected override void OnClone() { }   // Validations are handled internally
		

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
			throw new HamstarException( "No SetClientDefaults implemented" );
		}

		/// <summary>
		/// Overridden for initializing the class to create a reply in a request to the server.
		/// </summary>
		protected virtual void SetServerDefaults( int toWho ) {
			throw new HamstarException( "No SetServerDefaults(int) implemented" );
		}




		[Obsolete( "use SetServerDefaults( int toWho )", false )]
		protected virtual void SetServerDefaults() {
			throw new HamstarException( "No SetServerDefaults(int)" );
		}
	}
}
