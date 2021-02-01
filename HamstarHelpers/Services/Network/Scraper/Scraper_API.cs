using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Loadable;


namespace HamstarHelpers.Services.Network.Scraper {
	/// <summary>
	/// Implements a rudimentary network scraper.
	/// 
	/// Note: As of yet, outgoing byte data is not yet captured; only `NetMessage.SendData(...)` parameter values.
	/// </summary>
	public partial class Scraper : ILoadable {
		/// <summary></summary>
		public static bool IsScrappingSentData = false;

		/// <summary></summary>
		public static bool IsScrappingReceivedData = false;


		////////////////

		/// <summary></summary>
		public static IReadOnlyList<ScrapedSentData> SentDataView
			=> ModContent.GetInstance<Scraper>().SentData.AsReadOnly();

		/// <summary></summary>
		public static IReadOnlyList<ScrapedReceivedData> ReceivedDataView
			=> ModContent.GetInstance<Scraper>().ReceivedData.AsReadOnly();



		////////////////

		/// <summary></summary>
		/// <param name="listener"></param>
		public static void AddSendDataListener( Action<ScrapedSentData> listener ) {
			var scraper = ModContent.GetInstance<Scraper>();
			scraper.OnSent.Add( listener );
		}

		/// <summary></summary>
		/// <param name="listener"></param>
		public static void AddReceiveDataListener( Action<ScrapedReceivedData> listener ) {
			var scraper = ModContent.GetInstance<Scraper>();
			scraper.OnReceived.Add( listener );
		}


		/// <summary></summary>
		/// <param name="listener"></param>
		public static bool RemoveSendDataListener( Action<ScrapedSentData> listener ) {
			var scraper = ModContent.GetInstance<Scraper>();
			return scraper.OnSent.Remove( listener );
		}

		/// <summary></summary>
		/// <param name="listener"></param>
		public static bool RemoveReceiveDataListener( Action<ScrapedReceivedData> listener ) {
			var scraper = ModContent.GetInstance<Scraper>();
			return scraper.OnReceived.Remove( listener );
		}
	}
}
