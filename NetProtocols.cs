using HamstarHelpers.Utilities.Network;
using Terraria;


namespace HamstarHelpers {
	class HHModSettingsProtocol : PacketProtocol {
		public HamstarHelpersConfigData Data;

		////////////////

		public HHModSettingsProtocol() { }

		public override void SetServerDefaults() {
			this.Data = HamstarHelpersMod.Instance.Config;
		}

		public override void ReceiveOnClient() {
			var mymod = HamstarHelpersMod.Instance;
			mymod.Config.LoadFromNetwork( mymod, this.Data );
		}
	}



	class HHModDataProtocol : PacketProtocol {
		public int HalfDays;

		////////////////

		public HHModDataProtocol() { }

		public override void SetServerDefaults() {
			HamstarHelpersMod.Instance.WorldHelpers.SaveForNetwork( this );
		}

		public override void ReceiveOnClient() {
			var myplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>();

			HamstarHelpersMod.Instance.WorldHelpers.LoadFromNetwork( this.HalfDays );

			myplayer.Logic.FinishModDataSync();
		}
	}



	class HHPlayerPermaDeathProtocol : PacketProtocol {
		public int PlayerWho;
		public string Msg;

		////////////////

		public HHPlayerPermaDeathProtocol() { }

		internal HHPlayerPermaDeathProtocol( int player_who, string msg ) {
			this.PlayerWho = player_who;
			this.Msg = msg;
		}

		////////////////

		public override void ReceiveOnClient() {
			Player player = Main.player[ this.PlayerWho ];

			PlayerHelpers.PlayerHelpers.ApplyPermaDeath( player, this.Msg );
		}
	}
}
