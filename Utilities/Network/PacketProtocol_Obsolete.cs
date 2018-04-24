using HamstarHelpers.DebugHelpers;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using Terraria;


namespace HamstarHelpers.Utilities.Network {
	public abstract partial class PacketProtocol {
		[System.Obsolete( "use either SetClientDefaults or SetServerDefaults", false )]
		public virtual void SetDefaults() {
			//throw new NotImplementedException();
		}


		[System.Obsolete( "use either QuickSendToServer(), QuickSendToClient(int, int), or QuickSyncToServerAndClients()", true )]
		public static void QuickSendData<T>( int to_who, int ignore_who, bool sync_to_clients )
				where T : PacketProtocol, new() {
			if( Main.netMode == 1 ) {
				if( sync_to_clients ) {
					PacketProtocol.QuickSyncToServerAndClients<T>();
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


		[System.Obsolete( "do not use; automatic", true )]
		public virtual PacketProtocol ReadData( BinaryReader reader ) {
			int num = reader.ReadInt32();

			byte[] data = reader.ReadBytes( num );

			Type my_type = this.GetType();
			string json_str = Encoding.UTF8.GetString( data );

			return (PacketProtocol)JsonConvert.DeserializeObject( json_str, my_type );
		}

		[System.Obsolete( "do not use; automatic", true )]
		public virtual void WriteData( BinaryWriter writer, PacketProtocol _ ) {
			string json_str = JsonConvert.SerializeObject( this );
			var data = Encoding.ASCII.GetBytes( json_str );

			writer.Write( (int)data.Length );
			writer.Write( data );
		}



		[System.Obsolete( "use ReceiveWithClient()", false )]
		public virtual void ReceiveOnClient() {
			throw new NotImplementedException();
		}

		[System.Obsolete( "use ReceiveWithServer()", false )]
		public virtual void ReceiveOnServer( int from_who ) {
			throw new NotImplementedException();
		}

		[System.Obsolete( "use ReceiveRequestWithClient()", false )]
		public virtual bool ReceiveRequestOnClient() {
			return false;
		}

		[System.Obsolete( "use ReceiveRequestWithServer()", false )]
		public virtual bool ReceiveRequestOnServer( int from_who ) {
			return false;
		}
	}
}
