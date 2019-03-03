using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DotNetHelpers;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.MiscHelpers {
	public static partial class InfoHelpers {
		[Obsolete("use RenderMarkdownModTable", true)]
		public static string RenderModTable( string[] mods ) {
			return InfoHelpers.RenderMarkdownModTable( mods );
		}

		public static string RenderMarkdownModTable( string[] mods ) {
			int len = mods.Length;

			string output = "| Mods:  | - | - |";
			output += "\n| :--- | :--- | :--- |";

			for( int i = 0; i < len; i++ ) {
				output += '\n';
				output += "| `" + mods[i] + "` | ";
				output += (++i < len ? "`"+mods[i]+"`" : "-") + " | ";
				output += (++i < len ? "`"+mods[i]+"`" : "-") + " |";
			}

			return output;
		}


		////

		public static string RenderMarkdownPlayersTable() {
			IDictionary<string, string> playerInfos = null;
			string columns = "";
			int cols = 0;

			for( int i=0; i<Main.player.Length; i++ ) {
				Player plr = Main.player[i];
				if( plr == null || !plr.active ) { continue; }

				playerInfos = InfoHelpers.GetPlayerInfo(plr);
				cols = playerInfos.Count > cols ? playerInfos.Count : cols;

				playerInfos["Name"] = FormattingHelpers.SanitizeMarkdown( playerInfos["Name"] );
				
				columns += "| " + string.Join(" | ", playerInfos.Values) + " |";
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

			string playerLabel = "**Player "+FormattingHelpers.SanitizeMarkdown(player.name)+"'s ("+player.whoAmI+") equipment:**";

			string equipsLabels = cols > 0 ? string.Join( " | ", playerEquips.Keys ) : "-";
			string header = "| " + equipsLabels + " |";

			string subheader = "|";
			if( cols > 0 ) {
				for( int i = 0; i < cols; i++ ) {
					subheader += " :--- |";
				}
			} else {
				subheader += " :--- |";
			}

			string equips = string.Join( " | ", playerEquips.Values.SafeSelect(e=>FormattingHelpers.SanitizeMarkdown(e)) );
			string equipsCols = cols > 0 ? equips : "-";
			string columns = "| " + equipsCols + " |";

			return playerLabel + "\n \n" + header + "\n" + subheader + "\n" + columns;
		}
	}
}
