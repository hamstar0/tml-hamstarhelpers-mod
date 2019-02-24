using HamstarHelpers.Components.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.MiscHelpers {
	public static partial class InfoHelpers {
		public static IList<string> GetGameData( IEnumerable<Mod> mods ) {
			var list = new List<string>();

			var modsList = mods.OrderBy( m => m.Name ).Select( m => m.DisplayName + " " + m.Version.ToString() );
			string[] modsArr = modsList.ToArray();
			bool isDay = Main.dayTime;
			double timeOfDay = Main.time;
			int halfDays = WorldHelpers.WorldStateHelpers.GetElapsedHalfDays();
			string worldSize = WorldHelpers.WorldHelpers.GetSize().ToString();
			string[] worldProg = InfoHelpers.GetWorldProgress().ToArray();
			int activeItems = ItemHelpers.ItemHelpers.GetActive().Count;
			int activeNpcs = NPCHelpers.NPCHelpers.GetActive().Count;
			//string[] playerInfos = InfoHelpers.GetCurrentPlayerInfo().ToArray();
			//string[] playerEquips = InfoHelpers.GetCurrentPlayerEquipment().ToArray();
			int activePlayers = Main.ActivePlayersCount;
			string netmode = Main.netMode == 0 ? "single-player" : "multiplayer";
			bool autopause = Main.autoPause;
			bool autosave = Main.autoSave;
			int lighting = Lighting.lightMode;
			int lightingThreads = Lighting.LightingThreads;
			int frameSkipMode = Main.FrameSkipMode;
			bool isMaximized = Main.screenMaximized;
			int windowWid = Main.screenWidth;
			int windowHei = Main.screenHeight;
			int qualityStyle = Main.qaStyle;
			bool bgOn = Main.BackgroundEnabled;
			bool childSafe = !ChildSafety.Disabled;
			float gameZoom = Main.GameZoomTarget;
			float uiZoom = Main.UIScale;

			list.Add( InfoHelpers.RenderMarkdownModTable( modsArr ) );
			list.Add( InfoHelpers.RenderMarkdownPlayerTable() );
			list.Add( "Is day: " + isDay + ", Time of day/night: " + timeOfDay + ", Elapsed half days: " + halfDays );  //+ ", Total time (seconds): " + Main._drawInterfaceGameTime.TotalGameTime.Seconds;
			list.Add( "World name: " + Main.worldName + ", world size: " + worldSize );
			list.Add( "World progress: " + (worldProg.Length > 0 ? string.Join(", ", worldProg) : "none") );
			list.Add( "Items on ground: " + activeItems + ", Npcs active: " + activeNpcs );
			//list.Add( "Player info: " + string.Join( ", ", playerInfos ) );
			//list.Add( "Player equips: " + (playerEquips.Length > 0 ? string.Join(", ", playerEquips) : "none" ) );
			list.Add( "Player count: " + activePlayers + " (" + netmode + ")" );
			list.Add( "Autopause: " + autopause );
			list.Add( "Autosave: " + autosave );
			list.Add( "Lighting mode: " + lighting );
			list.Add( "Lighting threads: " + lightingThreads );
			list.Add( "Frame skip mode: " + frameSkipMode );
			list.Add( "Is screen maximized: " + isMaximized );
			list.Add( "Screen resolution: " + windowWid + " " + windowHei );
			list.Add( "Quality style: " + qualityStyle );
			list.Add( "Background on: " + bgOn );
			list.Add( "Child safety: " + childSafe );
			list.Add( "Game zoom: " + gameZoom );
			list.Add( "UI zoom: " + uiZoom );

			return list;
		}
	}
}
