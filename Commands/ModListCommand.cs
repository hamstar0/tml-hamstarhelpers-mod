using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Helpers.TmlHelpers.ModHelpers;
using HamstarHelpers.Services.Tml;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;


namespace HamstarHelpers.Commands {
	class ModListCommand : ModCommand {
		public override CommandType Type => CommandType.Chat | CommandType.Console;
		public override string Command => "mh-modlist";
		public override string Usage => "/" + this.Command + " true";
		public override string Description => "Lists mods, but with more information."
			+ "\n   Parameters: <verbose>";


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			if( args.Length == 0 ) {
				throw new UsageException( "No arguments supplied." );
			}

			bool isVerbose;
			if( !bool.TryParse(args[0], out isVerbose) ) {
				throw new UsageException( "Invalid 'verbose' argument supplied (must be boolean)." );
			}

			IList<string> reply = new List<string>( ModLoader.LoadedMods.Length );
			IDictionary<BuildPropertiesEditor, Mod> modList = ModListHelpers.GetModsByBuildInfo();

			foreach( var kv in modList ) {
				BuildPropertiesEditor editor = kv.Key;
				Mod mod = kv.Value;

				string info = mod.DisplayName + " v" + mod.Version + " by " + editor.Author;
				if( editor.Side != ModSide.Both ) {
					info += " (" + Enum.GetName(typeof(ModSide), editor.Side) + " only)";
				}

				if( isVerbose ) {
					if( editor.ModReferences.Count > 0 ) {
						IEnumerable<string> depMods = editor.ModReferences.SafeSelect( ( kv2 ) => {
							string depMod = kv2.Key;
							if( kv2.Value != default( Version ) ) {
								depMod += "@" + kv2.Value.ToString();
							}
							return depMod;
						} );

						info += ", mod dependencies: [" + string.Join(", ", depMods) + "]";
					}

					if( editor.DllReferences.Length > 0 ) {
						info += ", dll dependencies: [" + string.Join(", ", editor.DllReferences) + "]";
					}
				}

				reply.Add( info );
			}

			caller.Reply( string.Join("\n", reply) );
		}
	}
}
