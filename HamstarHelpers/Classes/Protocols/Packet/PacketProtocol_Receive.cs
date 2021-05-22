using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using HamstarHelpers.Classes.Protocols.Stream;
using HamstarHelpers.Libraries.Debug;


namespace HamstarHelpers.Classes.Protocols.Packet {
	public abstract partial class PacketProtocol : StreamProtocol {
		private void ReceiveWithEitherBase( BinaryReader reader, int fromWho ) {
			var mymod = ModHelpersMod.Instance;
			Type mytype = this.GetType();
			string protNamespace = mytype.Namespace;
			string protName = mytype.Name;

			try {
				this.ReadStream( reader );
			} catch( Exception e ) {
				string name = protNamespace + "." + protName;
				LogLibraries.Warn( "Stream read error for "+name+" - " + e.ToString() );
				//reader.BaseStream.Position = 0;
				//TODO: Output what remains of stram
				return;
			}

			if( ModHelpersConfig.Instance.DebugModeNetInfo && this.IsVerbose ) {
				string name = protNamespace + "." + protName;
				string jsonStr = JsonConvert.SerializeObject( this );
				LogLibraries.Log( "<" + name + " ReceiveWithEitherBase: " + jsonStr );
			}

			foreach( FieldInfo myField in mytype.GetFields() ) {
				FieldInfo yourField = mytype.GetField( myField.Name );

				if( yourField == null ) {
					string name = protNamespace + "." + protName;
					LogLibraries.Warn( "Missing " + name + " protocol field for " + myField.Name );
					return;
				}

				object val = yourField.GetValue( this );

				if( val == null ) {
					string name = protNamespace + "." + protName;
					LogLibraries.Warn( "Missing " + name + " protocol value for " + yourField.Name );
					return;
				}
				myField.SetValue( this, val );
			}
			
			mymod.PacketProtocolMngr.FulfillRequest( protName );	// If any
		}


		private void ReceiveWithClientBase( BinaryReader reader, int fromWho ) {
			this.ReceiveWithEitherBase( reader, fromWho );
			this.ReceiveWithClient();
		}

		private void ReceiveWithServerBase( BinaryReader reader, int fromWho, bool isSyncedToClients ) {
			this.ReceiveWithEitherBase( reader, fromWho );
#pragma warning disable CS0618 // Type or member is obsolete
			this.ReceiveWithServer( fromWho );
#pragma warning restore CS0618 // Type or member is obsolete
			this.ReceiveWithServer( fromWho, isSyncedToClients );
		}


		////////

		private void ReceiveRequestWithClientBase() {
			if( ModHelpersConfig.Instance.DebugModeNetInfo && this.IsVerbose ) {
				Type mytype = this.GetType();
				string name = mytype.Namespace + "." + mytype.Name;
				LogLibraries.Log( "<" + name + " ReceiveRequestWithClientBase..." );
			}
			
			this.SetClientDefaults();

			bool skipSend = this.ReceiveRequestWithClient();

			if( !skipSend ) {
				this.SendToServer( false );
			}
		}


		private void ReceiveRequestWithServerBase( int fromWho ) {
			if( ModHelpersConfig.Instance.DebugModeNetInfo && this.IsVerbose ) {
				Type mytype = this.GetType();
				string name = mytype.Namespace + "." + mytype.Name;
				LogLibraries.Log( "<" + name + " ReceiveRequestWithServerBase..." );
			}
			
			this.SetServerDefaults( fromWho );

			bool skipSend = this.ReceiveRequestWithServer( fromWho );

			if( !skipSend ) {
				this.SendToClient( fromWho, -1 );
			}
		}
	}
}
