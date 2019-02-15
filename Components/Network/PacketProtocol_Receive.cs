using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;


namespace HamstarHelpers.Components.Network {
	public abstract partial class PacketProtocol : PacketProtocolData {
		private void ReceiveWithEitherBase( BinaryReader reader, int fromWho ) {
			var mymod = ModHelpersMod.Instance;
			Type mytype = this.GetType();

			try {
				this.ReadStream( reader );
			} catch( Exception e ) {
				string name = mytype.Namespace + "." + mytype.Name;
				LogHelpers.Warn( "Stream read error for "+name+" - " + e.ToString() );
				//reader.BaseStream.Position = 0;
				//TODO: Output what remains of stram
				return;
			}

			if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
				string name = mytype.Namespace + "." + mytype.Name;
				string jsonStr = JsonConvert.SerializeObject( this );
				LogHelpers.Log( "<" + name + " ReceiveWithEitherBase: " + jsonStr );
			}

			foreach( FieldInfo myField in mytype.GetFields() ) {
				FieldInfo yourField = mytype.GetField( myField.Name );

				if( yourField == null ) {
					string name = mytype.Namespace + "." + mytype.Name;
					LogHelpers.Warn( "Missing " + name + " protocol field for " + myField.Name );
					return;
				}

				object val = yourField.GetValue( this );

				if( val == null ) {
					string name = mytype.Namespace + "." + mytype.Name;
					LogHelpers.Warn( "Missing " + name + " protocol value for " + yourField.Name );
					return;
				}
				myField.SetValue( this, val );
			}

			string packetName = this.GetType().Name;
		}


		private void ReceiveWithClientBase( BinaryReader reader, int fromWho ) {
			this.ReceiveWithEitherBase( reader, fromWho );
			this.ReceiveWithClient();
		}

		private void ReceiveWithServerBase( BinaryReader reader, int fromWho ) {
			this.ReceiveWithEitherBase( reader, fromWho );
			this.ReceiveWithServer( fromWho );
		}


		////////

		private void ReceiveRequestWithClientBase() {
			var mymod = ModHelpersMod.Instance;

			if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
				Type mytype = this.GetType();
				string name = mytype.Namespace + "." + mytype.Name;
				LogHelpers.Log( "<" + name + " ReceiveBaseRequestOnClient..." );
			}
			
			this.SetClientDefaults();

			bool skipSend = false;
			//var methodInfo = this.GetType().GetMethod( "ReceiveRequestOnClient" );
			
			skipSend = this.ReceiveRequestWithClient();

			if( !skipSend ) {
				this.SendToServer( false );
			}
		}


		private void ReceiveRequestWithServerBase( int fromWho ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
				Type mytype = this.GetType();
				string name = mytype.Namespace + "." + mytype.Name;
				LogHelpers.Log( "<" + name + " ReceiveBaseRequestOnServer..." );
			}

			try {
				this.SetServerDefaults( fromWho );
			} catch( NotImplementedException ) {
				this.SetServerDefaults();
			}

			bool skipSend = false;
			var methodInfo = this.GetType().GetMethod( "ReceiveRequestOnServer" );
			
			skipSend = this.ReceiveRequestWithServer( fromWho );

			if( !skipSend ) {
				this.SendToClient( fromWho, -1 );
			}
		}
	}
}
