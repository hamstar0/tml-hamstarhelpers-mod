using HamstarHelpers.TmlHelpers;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Commands {
	public class ConfigsRefreshCommand : ModCommand {
		public static void RefreshConfigs() {
			if( Main.netMode != 0 ) {
				throw new UsageException( "Cannot refresh configs in multiplayer.", Color.Red );
			}

			var mymod = HamstarHelpersMod.Instance;

			foreach( var kv in mymod.ModMetaDataManager.ConfigMods ) {
				ModMetaDataManager.ReloadConfigFromFile( kv.Value );
			}
		}



		////////////////
		
		public override CommandType Type { get { return CommandType.World; } }
		public override string Command { get { return "hhconfigsrefresh"; } }
		public override string Usage { get { return "/hhconfigsrefresh"; } }
		public override string Description { get { return "Refreshes all mod config files (single-player only). Only works for mods setup with Hamstar's Helpers to do so (see Control Panel)."; } }


		////////////////
		
		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = HamstarHelpersMod.Instance;

			ConfigsRefreshCommand.RefreshConfigs();

			string mod_names = string.Join( ", ", mymod.ModMetaDataManager.ConfigMods.Keys.ToArray() );
			caller.Reply( "Mod configs reloaded for " + mod_names, Color.Yellow );
		}
	}
}
