using HamstarHelpers.DebugHelpers;
using System.IO;
using Terraria;


namespace HamstarHelpers.Utilities.Network {
	public abstract partial class PacketProtocol {
		[System.Obsolete( "use either SetClientDefaults or SetServerDefaults", false )]
		public virtual void SetDefaults() {
			//throw new NotImplementedException();
		}


		[System.Obsolete( "use WriteData( BinaryWriter )", true )]
		public virtual void WriteData( BinaryWriter writer, PacketProtocol me ) {
			this.WriteData( writer );
		}


		[System.Obsolete( "use either QuickSendToServer(), QuickSendToClient(int, int), or QuickSyncToOtherClients()", true )]
		public static void QuickSendData<T>( int to_who, int ignore_who, bool sync_to_clients )
				where T : PacketProtocol, new() {
			if( Main.netMode == 1 ) {
				if( sync_to_clients ) {
					PacketProtocol.QuickSyncToOtherClients<T>();
				} else {
					PacketProtocol.QuickSendToServer<T>();
				}
			} else if( Main.netMode == 2 ) {
				PacketProtocol.QuickSendToClient<T>( to_who, ignore_who );
			}
		}

		[System.Obsolete( "use QuickRequestFromServer() or QuickRequestFromClient(int, int)", true )]
		public static void QuickSendRequest<T>( int to_who, int ignore_who )
				where T : PacketProtocol, new() {
			if( Main.netMode == 1 ) {
				PacketProtocol.QuickRequestFromServer<T>();
			} else if( Main.netMode == 2 ) {
				PacketProtocol.QuickRequestFromClient<T>( to_who, ignore_who );
			}
		}


		[System.Obsolete( "use QuickRequestFromServer() or QuickRequestFromClient(int, int)", true )]
		public void SendRequest( int to_who, int ignore_who ) {
			this.SendRequestOnly( -1, -1 );
		}

		[System.Obsolete( "use SendDataToServer(bool) or SendDataToClient(int, int)", true )]
		public void SendData( int to_who, int ignore_who, bool sync_to_clients ) {
			if( Main.netMode == 1 ) {
				this.SendToServer( sync_to_clients );
			} else if( Main.netMode == 2 ) {
				this.SendToClient( to_who, ignore_who );
			}
		}
	}
}
