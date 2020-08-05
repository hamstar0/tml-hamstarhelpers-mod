using System;
using System.IO;
using System.Collections.Generic;
using Terraria;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;


namespace HamstarHelpers.Services.Network {
	/// <summary>
	/// Supplies assorted server informations and tools.
	/// </summary>
	public partial class Client : ILoadable {
		internal IList<TileSectionPacketSubscriber> TileSectionPacketSubs { get; private set; } = new List<TileSectionPacketSubscriber>();



		////////////////

		void ILoadable.OnModsLoad() { }
		void ILoadable.OnModsUnload() { }
		void ILoadable.OnPostModsLoad() { }
	}
}
