using HamstarHelpers.TmlHelpers.CommandsHelpers;
using HamstarHelpers.TmlHelpers.ModHelpers;
using HamstarHelpers.Utilities.Web;
using HamstarHelpers.WebHelpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Terraria.ModLoader;


namespace HamstarHelpers.Commands {
	class ModIssueReportCommand : ModCommand {
		public override CommandType Type { get { return CommandType.Chat; } }
		public override string Command { get { return "hhmodissuereport"; } }
		public override string Usage { get { return "/hhmodissuereport 4 \"issue title\" \"issue description text\""; } }
		public override string Description { get { return "Reports an issue for a mod. Only works for mods setup with Hamstar's Helpers to do so (see Control Panel)."+
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

			var worker = new BackgroundWorker();
			string output = "Done?";

			worker.DoWork += delegate ( object sender, DoWorkEventArgs e_args ) {
				try {
					output = GithubModIssueReports.ReportIssue( mods[mod_idx - 1], title, body );
				} catch( Exception e ) {
					caller.Reply( e.Message, Color.Red );
				}
			};
			worker.RunWorkerCompleted += delegate ( object sender, RunWorkerCompletedEventArgs e_args ) {
				if( output != "Done?" ) {
					caller.Reply( output, Color.GreenYellow );
				} else {
					caller.Reply( "Issue report was not sent", Color.Red );
				}
			};
			worker.RunWorkerAsync();
		}
	}
}
