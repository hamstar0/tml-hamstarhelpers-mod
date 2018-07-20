using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;


namespace HamstarHelpers.Components.Network {
	public class PacketProtocolData {
		private IOrderedEnumerable<FieldInfo> _OrderedFields = null;

		internal IOrderedEnumerable<FieldInfo> OrderedFields {
			get {
				if( this._OrderedFields == null ) {
					Type mytype = this.GetType();
					FieldInfo[] fields = mytype.GetFields( BindingFlags.Public | BindingFlags.Instance );

					this._OrderedFields = fields.OrderByDescending( x => x.Name );  //Where( f => f.FieldType.IsPrimitive )
				}
				return this._OrderedFields;
			}
		}
	}




	public abstract partial class PacketProtocol : PacketProtocolData {
		protected static readonly object MyLock = new object();


		////////////////

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
			IEnumerable<Type> protocol_types = ReflectionHelpers.GetAllAvailableSubTypes( typeof( PacketProtocol ) );
			IDictionary<int, Type> protocol_type_map = new Dictionary<int, Type>();

			foreach( Type subclass in protocol_types ) {
				if( HamstarHelpersMod.Instance.Config.DebugModeNetInfo ) {
					string name = subclass.Namespace + "." + subclass.Name;
					LogHelpers.Log( "PacketProtocol.GetProtocols() - " + name );
				}

				try {
					string name = subclass.Namespace + "." + subclass.Name;
					int code = PacketProtocol.GetPacketCode( name );

					protocol_type_map[ code ] = subclass;
				} catch( Exception e ) {
					LogHelpers.Log( subclass.Name + " - " + e.Message );
				}
			}

			return protocol_type_map;
		}



		////////////////

		/// <summary>
		/// Indicates whether send packets will be logged if the config specifies to do so. Defaults to true.
		/// </summary>
		public virtual bool IsVerbose { get { return true; } }


		////////////////

		public PacketProtocol() { }


		////////////////

		public string GetPacketName() {
			var mytype = this.GetType();
			return mytype.Namespace + "." + mytype.Name;
		}


		////////////////

		public virtual void SetClientDefaults() {
			throw new NotImplementedException( "No SetClientDefaults" );
		}

		public virtual void SetServerDefaults() {
			throw new NotImplementedException( "No SetServerDefaults" );
		}
	}
}
