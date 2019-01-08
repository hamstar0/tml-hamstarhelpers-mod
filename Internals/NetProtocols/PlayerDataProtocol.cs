using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Components.Network;
using System;
using System.Collections.Generic;
using Terraria;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;


namespace HamstarHelpers.Internals.NetProtocols {
	class PlayerDataProtocol : PacketProtocolSentToEither {
		protected class MyFactory : PacketProtocolData.Factory<PlayerDataProtocol> {
			private readonly int PlayerWho;
			private readonly ISet<int> PermaBuffsById;
			private readonly ISet<int> HasBuffIds;
			private readonly IDictionary<int, int> EquipSlotsToItemTypes;


			////////////////

			public MyFactory( ISet<int> permaBuffsById, ISet<int> hasBuffIds, IDictionary<int, int> equipSlotsToItemTypes ) {
				this.PlayerWho = Main.myPlayer;
				this.PermaBuffsById = permaBuffsById;
				this.HasBuffIds = hasBuffIds;
				this.EquipSlotsToItemTypes = equipSlotsToItemTypes;
			}

			////

			protected override void Initialize( PlayerDataProtocol data ) {
				data.PlayerWho = this.PlayerWho;
				data.PermaBuffsById = this.PermaBuffsById;
				data.HasBuffIds = this.HasBuffIds;
				data.EquipSlotsToItemTypes = this.EquipSlotsToItemTypes;
			}
		}
		


		////////////////

		public static void SyncToEveryone( ISet<int> permaBuffsById, ISet<int> hasBuffIds, IDictionary<int, int> equipSlotsToItemTypes ) {
			if( Main.netMode != 1 ) { throw new Exception( "Not client" ); }

			var factory = new MyFactory( permaBuffsById, hasBuffIds, equipSlotsToItemTypes );
			PlayerDataProtocol protocol = factory.Create();
			
			protocol.SendToServer( true );
		}



		////////////////

		public int PlayerWho = 255;
		public ISet<int> PermaBuffsById = new HashSet<int>();
		public ISet<int> HasBuffIds = new HashSet<int>();
		public IDictionary<int, int> EquipSlotsToItemTypes = new Dictionary<int, int>();



		////////////////

		protected PlayerDataProtocol( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }


		protected override void SetServerDefaults( int fromWho ) { }


		////////////////

		protected override void ReceiveOnServer( int fromWho ) {
			Player player = Main.player[ fromWho ];
			var myplayer = player.GetModPlayer<ModHelpersPlayer>();
			
			myplayer.Logic.NetReceiveDataServer( this.PermaBuffsById, this.HasBuffIds, this.EquipSlotsToItemTypes );
		}

		protected override void ReceiveOnClient() {
			if( this.PlayerWho < 0 || this.PlayerWho >= Main.player.Length ) {
				//throw new HamstarException( "ModHelpers.PlayerDataProtocol.ReceiveWithClient - Invalid player index " + this.PlayerWho );
				throw new HamstarException( "Invalid player index " + this.PlayerWho );
			}

			Player player = Main.player[ this.PlayerWho ];
			if( player == null || !player.active ) {
				//LogHelpers.Log( "ModHelpers.PlayerDataProtocol.ReceiveWithClient - Inactive player indexed as " + this.PlayerWho );
				LogHelpers.Log( DebugHelpers.GetCurrentContext()+" - Inactive player indexed as " + this.PlayerWho );
				return;
			}

			var myplayer = player.GetModPlayer<ModHelpersPlayer>();

			myplayer.Logic.NetReceiveDataClient( this.PermaBuffsById, this.HasBuffIds, this.EquipSlotsToItemTypes );
		}
	}
}
