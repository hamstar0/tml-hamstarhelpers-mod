using HamstarHelpers.TmlHelpers;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Commands {
	public class ConfigsRefreshCommand : ModCommand {
		public static string RefreshConfigs() {
			var mymod = HamstarHelpersMod.Instance;

			foreach( var kv in mymod.ModMetaDataManager.ConfigMods ) {
				ModMetaDataManager.ReloadConfigFromFile( kv.Value );
			}

			string mod_names = string.Join( ", ", mymod.ModMetaDataManager.ConfigMods.Keys.ToArray() );

			return "Mod configs reloaded for " + mod_names;
		}



		////////////////

		public override CommandType Type { get { return CommandType.World; } }
		public override string Command { get { return "hhconfigsrefresh"; } }
		public override string Usage { get { return "/hhconfigsrefresh"; } }
		public override string Description { get { return "Refreshes all config files (setup to do so with Hamstar's Helpers)."; } }


		////////////////
		
		public override void Action( CommandCaller caller, string input, string[] args ) {
			if( Main.netMode != 0 ) {
				throw new UsageException( "Command not available in multiplayer.", Color.Red );
			}

			string output = ConfigsRefreshCommand.RefreshConfigs();

			caller.Reply( output, Color.Yellow );
		}
	}
}
