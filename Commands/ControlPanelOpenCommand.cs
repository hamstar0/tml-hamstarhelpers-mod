using Terraria.ModLoader;


namespace HamstarHelpers.Commands {
	class ControlPanelOpenCommand : ModCommand {
		public override CommandType Type { get { return CommandType.Chat; } }
		public override string Command { get { return "hh_cp_open"; } }
		public override string Usage { get { return "/hh_cp_open"; } }
		public override string Description { get { return "Opens the Hamstar's Helpers mod Control Panel."; } }


		////////////////
		
		public override void Action( CommandCaller caller, string input, string[] args ) {
			HamstarHelpersMod.Instance.ControlPanel.Open();
		}
	}
}
