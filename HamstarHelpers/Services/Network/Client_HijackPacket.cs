using System;
using System.IO;
using Terraria;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Libraries.TModLoader;


namespace HamstarHelpers.Services.Network {
	/// <summary>
	/// Supplies assorted server informations and tools.
	/// </summary>
	public partial class Client : ILoadable {
		/// <summary>
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="data"></param>
		public delegate void TileSectionPacketSubscriber( int tileX, int tileY, int width, int height, BinaryReader data );



		////////////////

		/// <summary>
		/// Allows intercepting tile sections as they're received.
		/// </summary>
		/// <param name="callback"></param>
		public static void SubscribeToTileSectionPackets( TileSectionPacketSubscriber callback ) {
			var client = TmlLibraries.SafelyGetInstance<Client>();
			client.TileSectionPacketSubs.Add( callback );
		}
	}
}
