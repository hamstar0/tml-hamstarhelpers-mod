using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.NPCChat;
using HamstarHelpers.Services.Timers;


namespace HamstarHelpers {
	/// @private
	partial class ModHelpersNPC : GlobalNPC {
		public override void GetChat( NPC npc, ref string chat ) {
			this.GetChatModded( npc, ref chat );
		}

		public override void OnChatButtonClicked( NPC npc, bool firstButton ) {
			if( npc.type == NPCID.Guide ) {
				if( firstButton ) {
					Timers.SetTimer( "ModHelpersGuideHelp", 1, false, () => {
						this.GetChatModdedWithoutPriorityChats( npc, ref Main.npcChatText );
						return false;
					} );
				}
			}
		}

		////

		private bool GetChatModded( NPC npc, ref string chat ) {
			ProcessMessage hiChatFunc = NPCChat.GetPriorityChat( npc.type );
			string hiChat = hiChatFunc?.Invoke( chat, out bool _ );

			if( hiChat != null ) {
				chat = hiChat;
				return true;
			}

			return this.GetChatModdedWithoutPriorityChats( npc, ref chat );
		}

		private bool GetChatModdedWithoutPriorityChats( NPC npc, ref string chat ) {
			bool? isNewChat;

			while( true) {
				isNewChat = NPCChat.GetChat( npc, ref chat );
				if( isNewChat.HasValue ) {
					break;
				}

				chat = npc.GetChat();
			}

			return isNewChat.Value;
		}
	}
}
