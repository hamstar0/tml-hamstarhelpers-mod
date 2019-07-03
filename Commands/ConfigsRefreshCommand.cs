using HamstarHelpers.Helpers.ModHelpers;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Commands {
	/// @private
	public class ConfigsRefreshCommand : ModCommand {
		/// @private
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

		/// @private
		public override CommandType Type => CommandType.World;
		/// @private
		public override string Command => "mh-configs-refresh";
		/// @private
		public override string Usage => "/" + this.Command;
		/// @private
		public override string Description => "Refreshes all mod config files (single-player only). Only works for mods setup with Mod Helpers to do so (see Control Panel).";


		////////////////

		/// @private
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
