using System;
using System.Collections.Generic;
using NetSerializer;
using Terraria;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.DotNET.Serialization;
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
			IEnumerable<Type> payloadTypes = ReflectionHelpers.GetAllAvailableSubTypesFromMods(
				typeof( NetProtocolPayload )
			).SafeOrderBy( t => t.FullName );
			var settings = new Settings {
				CustomTypeSerializers = new ITypeSerializer[] { new HashSetSerializer() }
			};

			this.Serializer = new Serializer( payloadTypes, settings );
		}

		void ILoadable.OnPostModsLoad() { }

		void ILoadable.OnModsUnload() { }
	}
}
