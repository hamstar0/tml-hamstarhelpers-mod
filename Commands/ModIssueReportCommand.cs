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
		public override CommandType Type => CommandType.Chat | CommandType.Console;
		public override string Command => "mh-mod-issue";
		public override string Usage => "/" + this.Command + " 4 \"issue title\" \"issue description text\"";
		public override string Description => "Reports an issue for a mod. Only works for mods setup to do so (see Control Panel)." +
					"\n   Parameters: <mod list index> \"<quote-wrapped issue title>\" \"<quote-wrapped issue description>\"";


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			IList<Mod> mods = ModListHelpers.GetAllLoadedModsPreferredOrder().ToList();
			int argIdx = 1;

			string title = CommandsHelpers.GetQuotedStringFromArgsAt( args, argIdx, out argIdx );
			if( argIdx == -1 ) {
				caller.Reply( "Invalid issue report title string", Color.Red );
				return;
			}

			string body = CommandsHelpers.GetQuotedStringFromArgsAt( args, argIdx, out argIdx );
			if( argIdx == -1 ) {
				caller.Reply( "Invalid issue report description string", Color.Red );
				return;
			}

			int modIdx;
			if( !int.TryParse( args[0], out modIdx ) ) {
				caller.Reply( args[argIdx] + " is not an integer", Color.Red );
				return;
			}
			if( modIdx <= 0 || modIdx > mods.Count ) {
				caller.Reply( args[argIdx] + " is not a mod entry; out of range", Color.Red );
				return;
			}

			Action<string> onSuccess = delegate ( string output ) {
				if( output != "Done?" ) {
					caller.Reply( output, Color.Lime );
				} else {
					caller.Reply( "Issue report was not sent", Color.Red );
				}
			};
			Action<Exception, string> onFail = ( e, output ) => {
				caller.Reply( e.Message, Color.Red );
			};

			PostGithubModIssueReports.ReportIssue( mods[modIdx - 1], title, body, onSuccess, onFail );
		}
	}
}
