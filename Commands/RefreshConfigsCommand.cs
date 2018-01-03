using HamstarHelpers.TmlHelpers;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Commands {
	class RefreshConfigsCommand : ModCommand {
		public static string RefreshConfigs() {
			foreach( var kv in ModMetaDataManager.ConfigMods ) {
				ModMetaDataManager.ReloadConfigFromFile( kv.Value );
			}

			string mod_names = string.Join( ", ", ModMetaDataManager.ConfigMods.Keys.ToArray() );

			return "Mod configs reloaded for " + mod_names;
		}



		public override CommandType Type { get { return CommandType.Chat; } }
		public override string Command { get { return "hh_refresh_configs"; } }
		public override string Usage { get { return "/hh_refresh_configs"; } }
		public override string Description { get { return "Refreshes all config files (recognized by Hamstar's Helpers)."; } }


		////////////////
		
		public override void Action( CommandCaller caller, string input, string[] args ) {
			if( Main.netMode != 0 ) {
				caller.Reply( "Command not available in multiplayer.", Color.Yellow );
				return;
			}

			string output = RefreshConfigsCommand.RefreshConfigs();

			caller.Reply( output, Color.Yellow );
		}
	}
}
