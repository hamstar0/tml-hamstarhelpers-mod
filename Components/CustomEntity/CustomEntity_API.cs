using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public abstract partial class CustomEntity : PacketProtocolData {
		[PacketProtocolIgnore]
		[JsonIgnore]
		private Player OwnerPlayer => this.OwnerPlayerWho == -1 ? null : Main.player[this.OwnerPlayerWho];

		[JsonProperty]
		private string[] ComponentNames => this.Components.SafeSelect( c => c.GetType().Name ).ToArray();

		[PacketProtocolIgnore]
		[JsonIgnore]
		public abstract bool SyncFromClient { get; }
		[PacketProtocolIgnore]
		[JsonIgnore]
		public abstract bool SyncFromServer { get; }

		[PacketProtocolIgnore]
		[JsonIgnore]
		public virtual bool IsInitialized {
			get {
				if( this.Core == null ) { return false; }
				if( this.Components.Count == 0 ) { return false; }
				return true;
			}
		}



		////////////////

		protected abstract CustomEntityCore CreateCore( CustomEntityConstructor factory );

		protected abstract IList<CustomEntityComponent> CreateComponents( CustomEntityConstructor factory );

		public abstract CustomEntityCore CreateCoreTemplate();

		public abstract IList<CustomEntityComponent> CreateComponentsTemplate();
	}
}
