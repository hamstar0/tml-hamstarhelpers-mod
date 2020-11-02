using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Timers;
using HamstarHelpers.Services.Dialogue;


namespace HamstarHelpers {
	/// @private
	partial class ModHelpersNPC : GlobalNPC {
		public override void GetChat( NPC npc, ref string chat ) {
			this.GetModdedDialogue( npc, ref chat );
		}

		public override void OnChatButtonClicked( NPC npc, bool firstButton ) {
			if( npc.type == NPCID.Guide ) {
				if( firstButton ) {
					Timers.SetTimer( "ModHelpersGuideHelp", 1, false, () => {
						this.GetNonDynamicModdedDialogue( npc, ref Main.npcChatText );
						return false;
					} );
				}
			}
		}


		////////////////

		private bool GetModdedDialogue( NPC npc, ref string chat ) {
			bool isModdedChat = this.GetNonDynamicModdedDialogue( npc, ref chat );

			DynamicDialogueHandler handler = DialogueEditor.GetDynamicDialogueHandler( npc.type );
			string hiChat = handler?.GetDialogue.Invoke( chat );

			if( hiChat != null ) {
				chat = hiChat;
				return true;
			}

			return isModdedChat;
		}

		private bool GetNonDynamicModdedDialogue( NPC npc, ref string chat ) {
			bool? isNewChat;

			while( true ) {
				isNewChat = DialogueEditor.GetChat( npc, ref chat );
				if( isNewChat.HasValue ) {
					break;
				}

				chat = npc.GetChat();
			}

			return isNewChat.Value;
		}
	}
}
