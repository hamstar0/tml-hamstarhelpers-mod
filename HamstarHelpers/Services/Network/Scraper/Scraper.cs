using System;
using System.Collections.Generic;


namespace HamstarHelpers.Services.Network.Scraper {
	/// <summary>
	/// Implements a rudimentary network scraper.
	/// 
	/// Note: As of yet, outgoing byte data is not yet captured; only `NetMessage.SendData(...)` parameter values.
	/// </summary>
	public class Scraper {
		/// <summary></summary>
		public static bool IsScrappingSentData = false;

		/// <summary></summary>
		public static bool IsScrappingReceivedData = false;


		////////////////

		/// <summary></summary>
		public static IList<ScrapedSentData> SentData { get; } = new List<ScrapedSentData>();

		/// <summary></summary>
		public static IList<ScrapedReceivedData> ReceivedData { get; } = new List<ScrapedReceivedData>();
	}
}
