using HamstarHelpers.Helpers.DebugHelpers;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;


namespace HamstarHelpers.Components.Network {
	public abstract partial class PacketProtocol {
		private void ReceiveWithClientBase( BinaryReader reader, int from_who ) {
			this.ReceiveWithEitherBase( reader, from_who );

			this.ReceiveWithClient();
		}

		private void ReceiveWithServerBase( BinaryReader reader, int from_who ) {
			this.ReceiveWithEitherBase( reader, from_who );

			this.ReceiveWithServer( from_who );
		}


		private void ReceiveWithEitherBase( BinaryReader reader, int from_who ) {
			HamstarHelpersMod mymod = HamstarHelpersMod.Instance;
			Type mytype = this.GetType();

			try {
				this.ReadStream( reader );
			} catch( Exception e ) {
				LogHelpers.Log( "Stream read error - " + e.ToString() );
				return;
			}

			if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
				string name = mytype.Namespace + "." + mytype.Name;
				string json_str = JsonConvert.SerializeObject( this );
				LogHelpers.Log( "<" + name + " ReceiveBaseEither: " + json_str );
			}

			foreach( FieldInfo my_field in mytype.GetFields() ) {
				FieldInfo your_field = mytype.GetField( my_field.Name );

				if( your_field == null ) {
					string name = mytype.Namespace + "." + mytype.Name;
					LogHelpers.Log( "Missing " + name + " protocol field for " + my_field.Name );
					return;
				}

				object val = your_field.GetValue( this );

				if( val == null ) {
					string name = mytype.Namespace + "." + mytype.Name;
					LogHelpers.Log( "Missing " + name + " protocol value for " + your_field.Name );
					return;
				}
				my_field.SetValue( this, val );
			}
		}


		////////

		private void ReceiveRequestWithClientBase() {
			HamstarHelpersMod mymod = HamstarHelpersMod.Instance;

			if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
				Type mytype = this.GetType();
				string name = mytype.Namespace + "." + mytype.Name;
				LogHelpers.Log( "<" + name + " ReceiveBaseRequestOnClient..." );
			}
			
			this.SetClientDefaults();

			bool skip_send = false;
			var method_info = this.GetType().GetMethod( "ReceiveRequestOnClient" );
			
			skip_send = this.ReceiveRequestWithClient();

			if( !skip_send ) {
				this.SendToServer( false );
			}
		}

		private void ReceiveRequestWithServerBase( int from_who ) {
			HamstarHelpersMod mymod = HamstarHelpersMod.Instance;

			if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
				Type mytype = this.GetType();
				string name = mytype.Namespace + "." + mytype.Name;
				LogHelpers.Log( "<" + name + " ReceiveBaseRequestOnServer..." );
			}
			
			this.SetServerDefaults();

			bool skip_send = false;
			var method_info = this.GetType().GetMethod( "ReceiveRequestOnServer" );
			
			skip_send = this.ReceiveRequestWithServer( from_who );

			if( !skip_send ) {
				this.SendToClient( from_who, -1 );
			}
		}


		////////////////

		/// <summary>
		/// Runs when data received on client (class's own fields).
		/// </summary>
		protected virtual void ReceiveWithClient() {
			throw new NotImplementedException( "No ReceiveWithClient" );
		}
		/// <summary>
		/// Runs when data received on server (class's own fields).
		/// </summary>
		/// <param name="from_who">Main.player index of the player (client) sending us our data.</param>
		protected virtual void ReceiveWithServer( int from_who ) {
			throw new NotImplementedException( "No ReceiveWithServer" );
		}


		/// <summary>
		/// Runs when a request is received for the client to send data to the server. Expects
		/// `SetClientDefaults()` to be implemented.
		/// </summary>
		/// <returns>True to indicate the request is being handled manually.</returns>
		protected virtual bool ReceiveRequestWithClient() {
			return false;
		}
		/// <summary>
		/// Runs when a request is received for the server to send data to the client. Expects
		/// `SetServerDefaults()` to be implemented.
		/// </summary>
		/// <param name="from_who">Main.player index of player (client) sending this request.</param>
		/// <returns>True to indicate the request is being handled manually.</returns>
		protected virtual bool ReceiveRequestWithServer( int from_who ) {
			return false;
		}
	}
}
