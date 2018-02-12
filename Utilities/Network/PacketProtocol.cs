using HamstarHelpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Utilities.Network {
	public abstract partial class PacketProtocol {
		protected static readonly object MyLock = new object();


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
					string name = subclass.Name;
					protocols[ name.GetHashCode() ] = subclass;
				} catch( Exception e ) {
					LogHelpers.Log( subclass.Name + " - " + e.Message );
				}
			}

			return protocols;
		}


		internal static void HandlePacket( BinaryReader reader, int player_who ) {
			var mymod = HamstarHelpersMod.Instance;

			int protocol_hash = reader.ReadInt32();
			bool is_request = reader.ReadBoolean();
			bool is_broadcast = reader.ReadBoolean();

			if( !mymod.PacketProtocols.ContainsKey( protocol_hash ) ) {
				throw new Exception( "Unrecognized packet." );
			}

			Type protocol_type = mymod.PacketProtocols[protocol_hash];

			try {
				PacketProtocol protocol = (PacketProtocol)Activator.CreateInstance( protocol_type );

				if( is_request ) {
					protocol.ReceiveRequest( player_who );
				} else {
					protocol.Receive( reader, player_who );

					if( is_broadcast && Main.netMode == 2 ) {
						protocol.SendData( -1, player_who, false );
					}
				}
			} catch( Exception e ) {
				throw new Exception( protocol_type.Name + " - " + e.ToString() );
			}
		}



		////////////////

		[System.Obsolete( "use either PacketProtocol.SetClientDefaults or PacketProtocol.SetServerDefaults", false )]
		public virtual void SetDefaults() {
			//throw new NotImplementedException();
		}

		public virtual void SetClientDefaults() {
			throw new NotImplementedException();
		}
		public virtual void SetServerDefaults() {
			throw new NotImplementedException();
		}
	}
}
