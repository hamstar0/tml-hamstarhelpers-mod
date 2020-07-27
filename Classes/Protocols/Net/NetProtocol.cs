using System;
using System.Collections.Generic;
using NetSerializer;
using Terraria;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.DotNET.Reflection;


namespace HamstarHelpers.Classes.Protocols.Net {
	/// <summary>
	/// Implement to define a network protocol. Protocols define what data to transmit, and how and where it can be
	/// transmitted.
	/// </summary>
	public partial class NetProtocol : ILoadable {
		private Serializer Serializer;



		////////////////

		void ILoadable.OnModsLoad() {
			IEnumerable<Type> payloadTypes = ReflectionHelpers.GetAllAvailableSubTypesFromMods( typeof(INetProtocolPayload) );

			this.Serializer = new Serializer( payloadTypes );
		}

		void ILoadable.OnPostModsLoad() { }

		void ILoadable.OnModsUnload() { }
	}
}
