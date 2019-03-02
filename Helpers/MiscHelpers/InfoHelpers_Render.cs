using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DotNetHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Helpers.MiscHelpers {
	public static partial class InfoHelpers {
		[Obsolete("use RenderMarkdownModTable", true)]
		public static string RenderModTable( string[] mods ) {
			return InfoHelpers.RenderMarkdownModTable( mods );
		}

		public static string RenderMarkdownModTable( string[] mods ) {
			int len = mods.Length;
			string[] sanMods = mods.Select( m => FormattingHelpers.SanitizeMarkdown(m) ).ToArray();

			string output = "| Mods:  | - | - |";
			output += "\n| :--- | :--- | :--- |";

			for( int i = 0; i < len; i++ ) {
				output += '\n';
				output += "| " + sanMods[i] + " | " + (++i < len ? sanMods[i] : "-") + " | " + (++i < len ? sanMods[i] : "-") + " |";
			}

			return output;
		}


		////

		public static string RenderMarkdownPlayerTable() {
			IDictionary<string, string> playerInfos = null;
			string columns = "";
			int cols = 0;

			for( int i=0; i<Main.player.Length; i++ ) {
				Player plr = Main.player[i];
				if( plr == null || !plr.active ) { continue; }

				playerInfos = InfoHelpers.GetPlayerInfo(plr);
				cols = playerInfos.Count > cols ? playerInfos.Count : cols;

				playerInfos["Name"] = "`" + playerInfos["Name"] + "`";

				IEnumerable<string> data = playerInfos.Values.Select( v => FormattingHelpers.SanitizeMarkdown(v) );
				columns += "| " + string.Join(" | ", data) + " |";
			}

			string header = "| " + string.Join(" | ", playerInfos.Keys) + " |";

			string subheader = "|";
			for( int i = 0; i < cols; i++ ) {
				subheader += " :--- |";
			}

			return header+"\n"+subheader+"\n"+columns;
		}


		public static string RenderMarkdownPlayerEquipsTable( Player player ) {
			IDictionary<string, string> playerEquips = InfoHelpers.GetPlayerEquipment( player );
			int cols = playerEquips.Count;

			string label = "*Player "+player.name+"'s ("+player.whoAmI+") equipment:*";

			string header = "| " + string.Join( " | ", playerEquips.Keys ) + " |";

			string subheader = "|";
			for( int i = 0; i < cols; i++ ) {
				subheader += " :--- |";
			}

			IEnumerable<string> equips = playerEquips.Values.Select( v => FormattingHelpers.SanitizeMarkdown(v) );
			string columns = "| " + string.Join( " | ", equips ) + " |";

			return label + "\n \n" + header + "\n" + subheader + "\n" + columns;
		}
	}
}
