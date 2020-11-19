using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using Terraria;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Services.Maps;


namespace HamstarHelpers.Services.Dialogue {
	/// <summary>
	/// Provides a service for adding or removing town NPC chats (based on weight values).
	/// </summary>
	public partial class DialogueEditor : ILoadable {
		internal static void UpdateAlertIconsOnMap() {
			var dialogEdit = TmlHelpers.SafelyGetInstance<DialogueEditor>();
			IDictionary<int, DynamicDialogueHandler> handlers = dialogEdit.DynamicHandlers;
			if( handlers.Count == 0 ) {
				return;
			}

			var uniques = new HashSet<int>();
			IDictionary<int, int> townNpcWhos = Main.npc
				.SafeWhere( n => {
					if( n?.active != true ) { return false; }
					if( !n.townNPC ) { return false; }

					if( uniques.Contains( n.type ) ) { return false; }
					uniques.Add( n.type );
					return true;
				} )
				.ToDictionary( n => n.type, n => n.whoAmI );

			foreach( (int townNpcType, int townNpcWho) in townNpcWhos ) {
				if( handlers.ContainsKey(townNpcType) && handlers[townNpcType].IsShowingAlert() ) {
					NPC npc = Main.npc[townNpcWho];

					MapMarkers.AddFullScreenMapMarker(
						tileX: (int)( npc.position.X / 16f ),
						tileY: (int)( npc.position.Y / 16f ),
						label: "ModHelpersDialogueAlert_" + townNpcType,
						icon: Main.chatTexture
					);
				} else {
					MapMarkers.RemoveFullScreenMapMarker( "ModHelpersDialogueAlert_" + townNpcType );
				}
			}
		}



		////////////////

		private IDictionary<int, IList<(float Weight, string Chat)>> AddedChats = new Dictionary<int, IList<(float, string)>>();
		private IDictionary<int, IList<string>> RemovedChatFlatPatterns = new Dictionary<int, IList<string>>();

		internal IDictionary<int, DynamicDialogueHandler> DynamicHandlers = new ConcurrentDictionary<int, DynamicDialogueHandler>();



		////////////////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnPostModsLoad() { }

		void ILoadable.OnModsUnload() { }
	}
}
