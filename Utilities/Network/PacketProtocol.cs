using HamstarHelpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Utilities.Network {
	public abstract partial class PacketProtocol {
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


		internal static IDictionary<int, Type> GetProtocols() {
			IDictionary<int, Type> protocols = new Dictionary<int, Type>();

			//var subclasses = from assembly in AppDomain.CurrentDomain.GetAssemblies()
			var mod_types = ModLoader.LoadedMods.Select( mod => mod.GetType() );
			var assemblies = mod_types.Select( mod_type => mod_type.Assembly );
			var subclasses = from assembly in assemblies
							 from type in assembly.GetTypes()
							 where type.IsSubclassOf( typeof( PacketProtocol ) ) && !type.IsAbstract
							 select type;

			foreach( Type subclass in subclasses ) {
				if( HamstarHelpersMod.Instance.Config.DebugModeNetInfo ) {
					LogHelpers.Log( "PacketProtocol.GetProtocols() - " + subclass.Name );
				}

				try {
					string name = subclass.Namespace + "." + subclass.Name;
					int code = PacketProtocol.GetPacketCode( name );

					protocols[ code ] = subclass;
				} catch( Exception e ) {
					LogHelpers.Log( subclass.Name + " - " + e.Message );
				}
			}

			return protocols;
		}


		////////////////

		internal static void HandlePacket( BinaryReader reader, int player_who ) {
			var mymod = HamstarHelpersMod.Instance;

			int protocol_hash = reader.ReadInt32();
			bool is_request = reader.ReadBoolean();
			bool is_synced_to_clients = Main.netMode == 2 ? reader.ReadBoolean() : false;

			if( !mymod.PacketProtocols.ContainsKey( protocol_hash ) ) {
				throw new Exception( "Unrecognized packet." );
			}

			Type protocol_type = mymod.PacketProtocols[ protocol_hash ];

			try {
				PacketProtocol protocol = (PacketProtocol)Activator.CreateInstance( protocol_type );

				if( is_request ) {
					protocol.ReceiveRequest( player_who );
				} else {
					protocol.Receive( reader, player_who );

					if( Main.netMode == 2 ) {
						if( is_synced_to_clients ) {
							protocol.SendToClient( -1, player_who );
						}
					}
				}
			} catch( Exception e ) {
				throw new Exception( protocol_type.Name + " - " + e.ToString() );
			}
		}



		////////////////

		/// <summary>
		/// Indicates whether send packets will be logged if the config specifies to do so. Defaults to true.
		/// </summary>
		public virtual bool IsVerbose { get { return true; } }

		private IOrderedEnumerable<FieldInfo> _OrderedFields = null;
		private IOrderedEnumerable<FieldInfo> OrderedFields {
			get {
				if( this._OrderedFields == null ) {
					Type mytype = this.GetType();
					FieldInfo[] fields = mytype.GetFields( BindingFlags.Public | BindingFlags.Instance );
					this._OrderedFields = fields.OrderByDescending( x => x.Name );  //Where( f => f.FieldType.IsPrimitive )
				}
				return this._OrderedFields;
			}
		}


		////////////////

		public PacketProtocol() { }

		////////////////

		public virtual void SetClientDefaults() {
			throw new NotImplementedException();
		}

		public virtual void SetServerDefaults() {
			throw new NotImplementedException();
		}
	}
}
