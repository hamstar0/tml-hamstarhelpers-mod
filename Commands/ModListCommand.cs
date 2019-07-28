using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.TModLoader.Mods;
using HamstarHelpers.Services.TML;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;


namespace HamstarHelpers.Commands {
	/// @private
	public class ModListCommand : ModCommand {
		/// @private
		public static string GetBasicModInfo( Mod mod, BuildPropertiesViewer editor ) {
			string info = mod.DisplayName + " v" + mod.Version + " by " + editor.Author;
			if( editor.Side != ModSide.Both ) {
				info += " (" + Enum.GetName( typeof( ModSide ), editor.Side ) + " only)";
			}

			return info;
		}

		/// @private
		public static string GetVerboseModInfo( Mod mod, BuildPropertiesViewer editor ) {
			string info = "";
			
			if( editor.ModReferences.Count > 0 ) {
				IEnumerable<string> depMods = editor.ModReferences.SafeSelect( ( kv2 ) => {
					string depMod = kv2.Key;
					if( kv2.Value != default( Version ) ) {
						depMod += "@" + kv2.Value.ToString();
					}
					return depMod;
				} );

				info += "mod dependencies: [" + string.Join( ", ", depMods ) + "]";
			}

			if( editor.DllReferences.Length > 0 ) {
				info += ", dll dependencies: [" + string.Join( ", ", editor.DllReferences ) + "]";
			}

			return info;
		}



		////////////////

		/// @private
		public override CommandType Type => CommandType.Chat | CommandType.Console;
		/// @private
		public override string Command => "mh-modlist";
		/// @private
		public override string Usage => "/" + this.Command + " true";
		/// @private
		public override string Description => "Lists mods, but with more information."
			+ "\n   Parameters: <verbose>";


		////////////////

		/// @private
		public override void Action( CommandCaller caller, string input, string[] args ) {
			if( args.Length == 0 ) {
				throw new UsageException( "No arguments supplied." );
			}

			bool isVerbose;
			if( !bool.TryParse(args[0], out isVerbose) ) {
				throw new UsageException( "Invalid 'verbose' argument supplied (must be boolean)." );
			}

			IList<string> reply = new List<string>( ModLoader.Mods.Length );
			IDictionary<BuildPropertiesViewer, Mod> modList = ModListHelpers.GetLoadedModsAndBuildInfo();

			foreach( var kv in modList ) {
				string modInfo = ModListCommand.GetBasicModInfo( kv.Value, kv.Key );

				if( isVerbose ) {
					string verboseModInfo = ModListCommand.GetVerboseModInfo( kv.Value, kv.Key );
					if( !string.IsNullOrEmpty(verboseModInfo) ) {
						modInfo += ", " + verboseModInfo;
					}
				}

				reply.Add( modInfo );
			}

			caller.Reply( string.Join("\n", reply) );
		}
	}
}
