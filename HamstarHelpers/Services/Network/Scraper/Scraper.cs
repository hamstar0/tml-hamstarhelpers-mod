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
		internal static void AddSentData( ScrapedSentData data ) {
			var scraper = ModContent.GetInstance<Scraper>();

			scraper.SentData.Add( data );

			foreach( Action<ScrapedSentData> callback in scraper.OnSent ) {
				callback( data );
			}
		}

		internal static void AddReceiveData( ScrapedReceivedData data ) {
			var scraper = ModContent.GetInstance<Scraper>();

			scraper.ReceivedData.Add( data );

			foreach( Action<ScrapedReceivedData> callback in scraper.OnReceived ) {
				callback( data );
			}
		}



		////////////////

		private List<ScrapedSentData> SentData { get; } = new List<ScrapedSentData>();

		private List<ScrapedReceivedData> ReceivedData { get; } = new List<ScrapedReceivedData>();

		////

		private List<Action<ScrapedSentData>> OnSent { get; } = new List<Action<ScrapedSentData>>();

		private List<Action<ScrapedReceivedData>> OnReceived { get; } = new List<Action<ScrapedReceivedData>>();



		////////////////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnModsUnload() { }

		void ILoadable.OnPostModsLoad() { }
	}
}
