using System;
using System.Collections.Generic;
using NetSerializer;
using Terraria;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Services.Network.NetIO.PayloadTypes;


namespace HamstarHelpers.Services.Network.NetIO {
	/// <summary>
	/// Provides functions to neatly send data (via. ModPacket) to server, clients, or both. Abstracts away serialization and
	/// routing.
	/// </summary>
	public partial class NetIO : ILoadable {
		private Serializer Serializer;



		////////////////

		void ILoadable.OnModsLoad() {
			IEnumerable<Type> payloadTypes = ReflectionHelpers.GetAllAvailableSubTypesFromMods( typeof(NetProtocolPayload) );

			this.Serializer = new Serializer( payloadTypes );
		}

		void ILoadable.OnPostModsLoad() { }

		void ILoadable.OnModsUnload() { }
	}
}
