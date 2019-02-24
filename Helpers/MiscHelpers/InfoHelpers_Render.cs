using HamstarHelpers.Components.Errors;
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
			string[] sanMods = mods.Select( m => m.Replace( "|", "\\|" ) ).ToArray();

			string output = "| Mods:  | - | - |";
			output += "\n| :--- | :--- | :--- |";

			for( int i = 0; i < len; i++ ) {
				output += '\n';
				output += "| " + sanMods[i] + " | " + ( ++i < len ? sanMods[i] : "-" ) + " | " + ( ++i < len ? sanMods[i] : "-" ) + " |";
			}

			return output;
		}


		////

		public static string RenderMarkdownPlayerTable() {
			IDictionary<string, string> playerInfos;
			string output = "";
			int cols = 0;

			for( int i=0; i<Main.player.Length; i++ ) {
				Player plr = Main.player[i];
				if( plr == null || !plr.active ) { continue; }

				playerInfos = InfoHelpers.GetPlayerInfo(plr);
				cols = playerInfos.Count > cols ? playerInfos.Count : cols;

				playerInfos["Name"] = "`" + playerInfos["Name"] + "`";

				output += "| " + string.Join(" | ", playerInfos.Values) + " |";
			}

			string header = "| Players:  |";
			for( int i = 1; i < cols; i++ ) {
				header += " - |";
			}

			string subheader = "|";
			for( int i = 0; i < cols; i++ ) {
				subheader += " :--- |";
			}

			return header+"\n"+subheader+"\n"+output;
		}


		public static string RenderMarkdownPlayerEquipsTable() {
			IDictionary<string, string> playerEquips;
			string output = "";
			int cols = 0;

			for( int i = 0; i < Main.player.Length; i++ ) {
				Player plr = Main.player[i];
				if( plr == null || !plr.active ) { continue; }

				playerEquips = InfoHelpers.GetPlayerEquipment( plr );
				cols = playerEquips.Count > cols ? playerEquips.Count : cols;

				output += "| " + string.Join( " | ", playerEquips.Values ) + " |";
			}

			string header = "| Player equipment:  |";
			for( int i = 1; i < cols; i++ ) {
				header += " - |";
			}

			string subheader = "|";
			for( int i = 0; i < cols; i++ ) {
				subheader += " :--- |";
			}

			return header + "\n" + subheader + "\n" + output;
		}
	}
}
