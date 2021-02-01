using System;
using System.IO;


namespace HamstarHelpers.Services.Network.Scraper {
	/// <summary>
	/// Represents scraped data for the settings of a received network packet. Interpretation of raw byte data is
	/// up to the user.
	/// </summary>
	public class ScrapedReceivedData {
		/// <summary></summary>
		public int MessageType;
		/// <summary></summary>
		public byte[] Data;
		/// <summary></summary>
		public int PlayerNumber;



		////////////////

		/// <summary></summary>
		/// <param name="messageType"></param>
		/// <param name="reader"></param>
		/// <param name="playerNumber"></param>
		public ScrapedReceivedData( byte messageType, BinaryReader reader, int playerNumber ) {
			long pos = reader.BaseStream.Position;

			this.MessageType = messageType;
			this.Data = reader.ReadBytes( (int)reader.BaseStream.Length );
			this.PlayerNumber = playerNumber;

			reader.BaseStream.Position = pos;
		}
	}
}
