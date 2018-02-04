using HamstarHelpers.Utilities.Network;
using Terraria;


namespace HamstarHelpers {
	public class ModSettingsProtocol : PacketProtocol {
		public override string GetName() { return "ModSettings"; }

		////////////////

		public HamstarHelpersConfigData Data;

		public ModSettingsProtocol() { }

		////////////////

		public override void SetDefaults() {
			var mymod = HamstarHelpersMod.Instance;

			this.Data = mymod.Config;
		}

		public override void ReceiveOnClient() {
			var mymod = HamstarHelpersMod.Instance;

			mymod.Config.LoadFromNetwork( mymod, this.Data );
		}
	}



	public class ModDataProtocol : PacketProtocol {
		public override string GetName() { return "ModData"; }

		////////////////

		public int HalfDays;

		public ModDataProtocol() { }

		////////////////

		public override void SetDefaults() {
			var myworld = HamstarHelpersMod.Instance.GetModWorld<HamstarHelpersWorld>();

			myworld.WorldLogic.SaveForNetwork( this );
		}

		public override void ReceiveOnClient() {
			var myworld = HamstarHelpersMod.Instance.GetModWorld<HamstarHelpersWorld>();

			myworld.WorldLogic.LoadFromNetwork( this.HalfDays );
		}
	}



	public class PlayerPermaDeathProtocol : PacketProtocol {
		public override string GetName() { return "PlayerPermaDeath"; }

		////////////////

		public int PlayerWho;
		public string Msg;

		public PlayerPermaDeathProtocol() { }

		////////////////

		internal PlayerPermaDeathProtocol( int player_who, string msg ) {
			this.PlayerWho = player_who;
			this.Msg = msg;
		}

		////////////////

		public override void ReceiveOnClient() {
			Player player = Main.player[ this.PlayerWho ];

			PlayerHelpers.PlayerHelpers.ApplyPermaDeath( player, this.Msg );
		}
	}



	abstract public class AbstractPlayerDataProtocol : PacketProtocol {
		public int PlayerWho;
		
		protected AbstractPlayerDataProtocol( int player_who ) {
			this.PlayerWho = player_who;
		}
	}
}
