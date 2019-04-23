using HamstarHelpers.Helpers.TmlHelpers.ModHelpers;
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

			var mymod = ModHelpersMod.Instance;

			foreach( var kv in mymod.ModFeaturesHelpers.ConfigMods ) {
				ModFeaturesHelpers.ReloadConfigFromFile( kv.Value );
			}
		}



		////////////////
		
		public override CommandType Type => CommandType.World;
		public override string Command => "mh-configs-refresh";
		public override string Usage => "/" + this.Command;
		public override string Description => "Refreshes all mod config files (single-player only). Only works for mods setup with Mod Helpers to do so (see Control Panel).";


		////////////////
		
		public override void Action( CommandCaller caller, string input, string[] args ) {
			if( Main.netMode != 0 ) {
				throw new UsageException( "Not single player.", Color.Red );
			}

			var mymod = ModHelpersMod.Instance;

			ConfigsRefreshCommand.RefreshConfigs();

			string modNames = string.Join( ", ", mymod.ModFeaturesHelpers.ConfigMods.Keys.ToArray() );
			caller.Reply( "Mod configs reloaded for " + modNames, Color.Lime );
		}
	}
}
