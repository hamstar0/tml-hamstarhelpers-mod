using System;
using Terraria.Localization;


namespace HamstarHelpers.Services.Network.Scraper {
	/// <summary>
	/// Represents scraped data for the settings of a sent network packet. The actual payload of the generated
	/// packet is internal to `NetMessage.SendData(...)` (for now).
	/// </summary>
	public class ScrapedSentData {
		/// <summary></summary>
		public int WhoAmI;
		/// <summary></summary>
		public int MsgType;
		/// <summary></summary>
		public int RemoteClient;
		/// <summary></summary>
		public int IgnoreClient;
		/// <summary></summary>
		public NetworkText Text;
		/// <summary></summary>
		public int Number;
		/// <summary></summary>
		public float Number2;
		/// <summary></summary>
		public float Number3;
		/// <summary></summary>
		public float Number4;
		/// <summary></summary>
		public int Number5;
		/// <summary></summary>
		public int Number6;
		/// <summary></summary>
		public int Number7;



		////////////////

		/// <summary></summary>
		/// <param name="whoAmI"></param>
		/// <param name="msgType"></param>
		/// <param name="remoteClient"></param>
		/// <param name="ignoreClient"></param>
		/// <param name="text"></param>
		/// <param name="number"></param>
		/// <param name="number2"></param>
		/// <param name="number3"></param>
		/// <param name="number4"></param>
		/// <param name="number5"></param>
		/// <param name="number6"></param>
		/// <param name="number7"></param>
		internal ScrapedSentData(
					int whoAmI,
					int msgType,
					int remoteClient,
					int ignoreClient,
					NetworkText text,
					int number,
					float number2,
					float number3,
					float number4,
					int number5,
					int number6,
					int number7 ) {
			this.WhoAmI = whoAmI;
			this.MsgType = msgType;
			this.RemoteClient = remoteClient;
			this.IgnoreClient = ignoreClient;
			this.Text = text;
			this.Number = number;
			this.Number2 = number2;
			this.Number3 = number3;
			this.Number4 = number4;
			this.Number5 = number5;
			this.Number6 = number6;
			this.Number7 = number7;
		}
	}
}
