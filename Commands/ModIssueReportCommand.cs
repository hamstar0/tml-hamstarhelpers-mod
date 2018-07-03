using HamstarHelpers.Helpers.TmlHelpers.CommandsHelpers;
using HamstarHelpers.Helpers.TmlHelpers.ModHelpers;
using HamstarHelpers.Internals.WebRequests;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;


namespace HamstarHelpers.Commands {
	class ModIssueReportCommand : ModCommand {
		public override CommandType Type { get { return CommandType.Chat | CommandType.Console; } }
		public override string Command { get { return "hhmodissue"; } }
		public override string Usage { get { return "/hhmodissue 4 \"issue title\" \"issue description text\""; } }
		public override string Description { get { return "Reports an issue for a mod. Only works for mods setup to do so (see Control Panel)."+
					"\n   Parameters: <mod list index> \"<quote-wrapped issue title>\" \"<quote-wrapped issue description>\""; } }


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			IList<Mod> mods = ModHelpers.GetAllMods().ToList();
			int arg_idx = 1;

			string title = CommandsHelpers.GetQuotedStringFromArgsAt( args, arg_idx, out arg_idx );
			if( arg_idx == -1 ) {
				caller.Reply( "Invalid issue report title string", Color.Red );
				return;
			}

			string body = CommandsHelpers.GetQuotedStringFromArgsAt( args, arg_idx, out arg_idx );
			if( arg_idx == -1 ) {
				caller.Reply( "Invalid issue report description string", Color.Red );
				return;
			}

			int mod_idx;
			if( !int.TryParse( args[0], out mod_idx ) ) {
				caller.Reply( args[arg_idx] + " is not an integer", Color.Red );
				return;
			}
			if( mod_idx <= 0 || mod_idx > mods.Count ) {
				caller.Reply( args[arg_idx] + " is not a mod entry; out of range", Color.Red );
				return;
			}

			Action<string> on_success = delegate ( string output ) {
				if( output != "Done?" ) {
					caller.Reply( output, Color.GreenYellow );
				} else {
					caller.Reply( "Issue report was not sent", Color.Red );
				}
			};
			Action<Exception, string> on_fail = ( e, output ) => {
				caller.Reply( e.Message, Color.Red );
			};

			GithubModIssueReports.ReportIssue( mods[mod_idx - 1], title, body, on_success, on_fail );
		}
	}
}
